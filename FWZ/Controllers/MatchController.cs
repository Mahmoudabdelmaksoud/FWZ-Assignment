using FWZ.Application.Contracts;
using FWZ.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FWZ.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class MatchController : ControllerBase
        {
            // inject service object
            private readonly IMatchService _matchService;

            public MatchController(IMatchService matchService)
            {
                _matchService = matchService;
            }

            //api/Match/{matchId}
            [HttpGet("{matchId}")]

            //method takes the matchid, and the player number "in case he chose it" as parameters
            public async Task<ActionResult<IEnumerable<PlayerDto>>> GetMatch(string matchId, [FromQuery] int? PlayerNumber)
            {
                var players = await _matchService.GetParsedMatchAsync(matchId); //calling GetParsedMatchAsync from MatchService to return the players

                if (PlayerNumber.HasValue && PlayerNumber.Value >= 1 && PlayerNumber.Value <= 10) //check the user input of player number
                {
                    players = new List<PlayerDto> { players.ElementAt(PlayerNumber.Value - 1) }; //choose the player user chose
                }

                return Ok(players); // retunr the player or the players
            }

        }
    }