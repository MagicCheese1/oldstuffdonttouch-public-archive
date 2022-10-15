using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using Botupgrade.DataHandling;

namespace Botupgrade.DataHandling
{
    public class ReloadSettings
    {
        public static void Reload()
        {          
            //Settings.Json  
            string SettingsLocation = Assembly.GetEntryAssembly().Location.Replace("\\Botupgrade.dll", "/data/Settings.json");
            if(!SettingsLocation.EndsWith("/data/Settings.json"))
                SettingsLocation = Assembly.GetEntryAssembly().Location.Replace("/Botupgrade.dll", "/data/Settings.json");
            if(!File.Exists(SettingsLocation))
            {
                GenerateSettings(SettingsLocation);
            }
            string JSON = "";

            JSON = File.ReadAllText(SettingsLocation);

           // using(var Stream = new FileStream(SettingsLocation, FileMode.Open, FileAccess.Read))
           // using(var ReadSettings = new StreamReader(Stream))
           // {
           //     JSON = ReadSettings.ReadToEnd();
           // }
            if(JSON == null || JSON == "")
            {
                GenerateSettings(SettingsLocation);
            }

            Setting Settings = JsonConvert.DeserializeObject<Setting>(JSON);

            ESettings.logChannel       = Settings.logChannel;
            ESettings.token            = Settings.token;      
            ESettings.Tech             = Settings.Tech;
            ESettings.Owner            = Settings.Owner;
            ESettings.ModeratorDC      = Settings.ModeratorDC;
            ESettings.UserAccountsPath = Settings.UserAccountsPath;
            
        }
        public static void GenerateSettings(string SettingsLocation)
        {                    
                Console.WriteLine($" {DateTime.Now} at Local] Settings.json Not found or null");
                Console.WriteLine($" {DateTime.Now} at Local] Generating Settings.json");
                List<Setting> Data = new List<Setting>();
                Data.Add(new Setting(){
                    token = "DiscordAppBotUserToken",
                    logChannel = new List<ulong>(new ulong[2]{0,0}),
                    Tech = new List<ulong>(new ulong[2]{0,0}),
                    Owner = new List<ulong>(new ulong[2]{0,1}),
                    ModeratorDC = new List<ulong>(new ulong[2]{0,2}),
                    Everyone = 100
                });
                string output = JsonConvert.SerializeObject(Data);
                output = output.Substring(1, output.Length - 2);
                File.WriteAllText(SettingsLocation, output);
                Console.WriteLine($" {DateTime.Now} at Local] Settings.json Generated successfully. Please Set it and restart the application.");
                Console.ReadLine();
                Environment.Exit(1);
        }
    }
}