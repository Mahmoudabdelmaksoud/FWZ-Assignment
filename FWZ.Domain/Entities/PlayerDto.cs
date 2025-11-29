using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWZ.Domain.Entities
{
    //Declaring player stats as dto "No need to store it as a model and then create a dto for it"
    public class PlayerDto
    {
        public string SummonerName { get; set; } = "";
        public string ChampionName { get; set; } = "";
        public int Gold { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int CS { get; set; }
        public int DamageDealtToChampions { get; set; }
        public int DamageTaken { get; set; }

    }
}
