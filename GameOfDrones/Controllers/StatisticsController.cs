using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Core.Domain.Models;
using GameOfDrones.Models.Player;
using GameOfDrones.Models.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameOfDrones.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class StatisticsController : ControllerBase
    {
        private IStatisticsService StatisticsService { get; }
        private IMapper Mapper { get; }

        public StatisticsController(IStatisticsService statisticsService, IMapper mapper)
        {
            StatisticsService = statisticsService;
            Mapper = mapper;
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

        [HttpGet("{playerName}")]
        public async Task<IActionResult> GetPlayerStatistics([FromRoute] string playerName)
        {
            var player = await StatisticsService.GetPlayerByUserNameAsync(playerName);

            if (player == null)
                return Ok(null);

            return Ok(Mapper.Map<Player, PlayerStatisticsViewModel>(player));
        }
    }
}