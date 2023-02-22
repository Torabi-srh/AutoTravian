using AutoTravian.Core.Data;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoTravian.Core.Util
{
    internal static class Logger
    {
        public static List<Tuple<string, Color>> errors = new List<Tuple<string, Color>>();
        public static void Log(string message, Color color)
        {
            try
            {
                errors.Add(new Tuple<string, Color>(message, color));
            }
            catch (Exception)
            {
            }
        }

        public static bool SaveData(string json)
        {
            JsonParser data = JsonParser.FromJson(json);
            try
            {
                if (data.Response.Error == true)
                {
                    Log("ERROR: " + data.Response.ErrorMsg.ToString(), Color.Red);
                }
                string savejson = JsonConvert.SerializeObject(data);
                return true;
            }
            catch (Exception e)
            {
                Log(e.Message, Color.Black);
                return false;
            }
        }

        public static bool GetErrorMessage(string json)
        {
            JsonParser data;
            try
            {
                data = JsonParser.FromJson(json);
                if (data.Response.Error.Equals(true))
                {
                    return true;
                }
                else if (data.Response.Data.Options.DialogOptions != null)
                {
                    Log("Not enough gold!", Color.DarkGoldenrod);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Log(e.Message, Color.Black);
                return true;
            }
        }

    }
}
