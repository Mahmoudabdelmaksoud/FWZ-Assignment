using FWZ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWZ.Application.Contracts
{
    // Contracting the match service
    public interface IMatchService
    {
        Task<IEnumerable<PlayerDto>> GetParsedMatchAsync(string MatchId);
    }
}
