﻿using AutoTravian.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTravian.UI.DbContexts
{
    internal class BuildingDbContext : DbContext
    {
        public DbSet<BuildingEntity> BuildingTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=AutoTravian.db");
        }
    }
}
