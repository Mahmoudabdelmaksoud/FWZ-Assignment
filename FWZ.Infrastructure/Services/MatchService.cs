using FWZ.Application.Contracts;
using FWZ.Domain.Entities;
using FWZ.Infrastructure.Configs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FWZ.Infrastructure.Services
{
    public class MatchService : IMatchService
    {
        // dependency injecting object from IHttpClientFactory to create a client
        private readonly IHttpClientFactory _httpClientFactory;

        // dependency injecting object RiotApiConfig to access the value in appsetting.json
        private readonly RiotApiConfig _config;
        public MatchService(IHttpClientFactory httpClientFactory, IOptions<RiotApiConfig> options) //IOptions navigates through appsetting
        {
            _httpClientFactory = httpClientFactory;
            _config = options.Value; //Key value pairs
        }
        public async Task<IEnumerable<PlayerDto>> GetParsedMatchAsync(string MatchId)
        {
            //Generate request (http client) of Riot Kind
            var client = _httpClientFactory.CreateClient("riot");

            //Creating the token and then passing the ApiKey from appsettings.json
            client.DefaultRequestHeaders.Add("X-Riot-Token", _config.ApiKey);

            //creating a variable to store the url
            var url = $"{_config.BaseUrl}/lol/match/v5/matches/{MatchId}";

            //response of the request
            var response = await client.GetAsync(url); 

            //if the request is not successful, create an empty player dto list and return it to avoid exception
            if (!response.IsSuccessStatusCode)
            {
                return new List<PlayerDto>();
            }

            //if successful, store the response as a string
            var json = await response.Content.ReadAsStringAsync();

            //Parse it into Json
            File.WriteAllText($"raw_{MatchId}.json", json);

            //parse it into JsonDocument
            var doc = JsonDocument.Parse(json); //

            //check if there is an info in the doc
            if (!doc.RootElement.TryGetProperty("info", out var info) ||
                !info.TryGetProperty("participants", out var players)) //if not empty store it into players array
            {
                // if empty return an empty list of dto instead of exception
                return new List<PlayerDto>();
            }

            // loop over the players array
            foreach (var player in players.EnumerateArray())
                Console.WriteLine(player); // print it on the console just to check the json doc is correct (debugging step)

            //create a new list of playerdto to store the info in it
            var PlayerStats = new List<PlayerDto>();

            //loop through players array
            foreach (var p in players.EnumerateArray()) //Looping on Participants array //EnumerateArray to disfunction the array and assign the value inside the loop
            {
                // begin to add to PlayerStats for each player
                //using manual mapping instead of Imapper as it is used for huge datasets and no need for it
                PlayerStats.Add(new PlayerDto 
                {
                    SummonerName = string.IsNullOrWhiteSpace(p.GetProperty("summonerName").GetString()) ? "Confidential" : p.GetProperty("summonerName").GetString(),
                    ChampionName = p.GetProperty("championName").GetString() ?? "",
                    Gold = p.GetProperty("goldEarned").GetInt32(),
                    Kills = p.GetProperty("kills").GetInt32(),
                    Deaths = p.GetProperty("deaths").GetInt32(),
                    Assists = p.GetProperty("assists").GetInt32(),
                    CS = p.GetProperty("totalMinionsKilled").GetInt32(),
                    DamageDealtToChampions = p.GetProperty("totalDamageDealtToChampions").GetInt32(),
                    DamageTaken = p.GetProperty("totalDamageTaken").GetInt32()
                });
            }
            return PlayerStats;
        }

    }
}
