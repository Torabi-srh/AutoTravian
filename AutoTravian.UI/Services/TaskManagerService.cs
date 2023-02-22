using AutoTravian.Core.Enums;
using AutoTravian.UI.DbContexts;
using AutoTravian.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinkerTaskManager;
using TaskScheduler = ThinkerTaskManager.TaskScheduler;

namespace AutoTravian.UI.Services
{
    internal class TaskManagerService
    {
        public Dictionary<TaskEntity, ITask> Tasks { get; set; }
        public TaskScheduler Scheduler { get; set; }
        public TaskManagerService()
        {
            Tasks = new Dictionary<TaskEntity, ITask>();
            Scheduler = new TaskScheduler();
            RecurringTask task = new RecurringTask(() =>
            {

            }, DateTime.Now, TimeSpan.FromSeconds(1));
            Scheduler.AddTask(task);
        }
        public void Start()
        {
            Scheduler.Start();
        }
        public void Stop()
        {
            Scheduler.Stop();
        }
        public async Task Save()
        {
            using (TasksDbContext context = new())
            {
                foreach (var task in Tasks)
                {
                    context.TasksTable.Add(task.Key);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task Load()
        {
            using (TasksDbContext context = new())
            {
                var data = await context.TasksTable.ToListAsync();
                foreach (var t in data)
                {
                    if (t.EndTime >= DateTime.Now)
                    {
                        continue;
                    }
                    RecurringTask task = new RecurringTask(GetAction(t.Action, t.Arguments?.Split(":")?.ToArray()), t.StartTime, t.Interval);
                    Tasks.Add(t, task);
                    Scheduler.AddTask(task);
                }
            }
        }
        public Action GetAction(string type, string[]? args)
        {
            if (type == "login")
            {
                return async () =>
                {
                    bool l = false;
                    while (!l) l = await App.Api.Login(args?[0] ?? "", args?[1] ?? "");
                };
            }
            else if (type == "resources")
            {
                return async () =>
                {
                    bool l = false;
                    while (!l) l = await App.Api.Resources();
                };
            }
            else if (type == "resourcefields")
            {
                return async () =>
                {
                    bool l = false;
                    while (!l) l = await App.Api.ResourceFields();
                };
            }
            else if (type == "buildings")
            {
                return async () =>
                {
                    bool l = false;
                    while (!l) l = await App.Api.Buildings();
                };
            }
            else if (type == "vilageinfo")
            {
                return async () =>
                {
                    bool l = false;
                    while (!l) l = await App.Api.VilageInfo();
                };
            }
            else if (type == "BuildingUpgrade")
            {
                return async () =>
                {
                    bool l = false;
                    while (!l) l = await App.Api.BuildingUpgrade(args?[0] ?? "");
                };
            }
            else if (type == "trainUnit")
            {
                return async () =>
                {
                    bool l = false;
                    while (!l) l = await App.Api.TrainUnits(args?[0] ?? "", args?[1] ?? "");
                };
            }
            else if (type == "buildBuilding")
            {
                return async () =>
                {
                    bool l = false;
                    BuildingTypes buildingType = (BuildingTypes)Enum.Parse(typeof(BuildingTypes), args?[1] ?? "Empty");
                    while (!l) l = await App.Api.BuildBuilding(args?[0] ?? "", buildingType);
                };
            }
            return () => { };
        }
        public void Add(TaskEntity t)
        {
            RecurringTask task = new RecurringTask(GetAction(t.Action, t.Arguments?.Split(":")?.ToArray()), t.StartTime, t.Interval);
            Tasks.Add(t, task);
            Scheduler.AddTask(task);
        } 
    }
}