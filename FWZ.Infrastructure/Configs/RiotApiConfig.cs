using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWZ.Infrastructure.Configs
{
    //API config to use in the service from appsetting.json
    public class RiotApiConfig
    {
        public string ApiKey { get; set; } = ""; // Is added in appsetting.json //Mwgod in case appsetting is deleted, el program mydnesh exception
        public string Region { get; set; } = "europe";
        public string BaseUrl { get; set; } = "https://europe.api.riotgames.com";
    }
}
