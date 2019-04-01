using RoadStatus.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatus.Configuration
{
    public class AppSettingsConfigurationManager : IConfigurationManager
    {
        public string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
