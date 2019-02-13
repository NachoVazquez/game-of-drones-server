using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Models.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameOfDrones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private IStatisticsService StatisticsService { get; }

        public StatisticsController(IStatisticsService statisticsService)
        {
            StatisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGlobalStatistics()
        {
            var registeredPlayers = await this.StatisticsService.GetRegisteredPlayersAsync();
            if (registeredPlayers == null)
            {
                return StatusCode(500, "Something went wrong trying to obtain the number of registered players");
            }

            var gamesPlayed = await this.StatisticsService.GetGamesPlayed();

            if (gamesPlayed == null)
            {
                return StatusCode(500, "Something went wrong trying to obtain the number of games played");
            }

            var roundsPlayed = await this.StatisticsService.GetRoundsPlayed();

            if (roundsPlayed == null)
            {
                return StatusCode(500, "Something went wrong trying to obtain the number of rounds played");
            }

            return Ok(new GlobalStatisticsViewModel
            {
                GamesPlayed = gamesPlayed.Value, RegisteredPlayers = registeredPlayers.Value,
                RoundsPlayed = roundsPlayed.Value
            });
        }
    }
}