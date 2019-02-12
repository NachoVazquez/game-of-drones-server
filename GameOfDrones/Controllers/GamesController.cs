using AutoMapper;
using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Models;
using GameOfDrones.Models.Game;
using GameOfDrones.Models.Round;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace GameOfDrones.Controllers
{
    public class GamesController : BaseApiController<IGameService, Game, int, GameIndexViewModel, GameCreateViewModel,
        GameIndexViewModel, GameIndexViewModel>
    {
        private Serilog.ILogger SeriLogger { get; set; }

        public GamesController(IGameService appService, IMapper mapper, IUnitOfWork unitOfWork) : base(appService,
            mapper, unitOfWork)
        {
            SeriLogger = Log.ForContext<GamesController>();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameWithPlayersById([FromRoute] int id)
        {
            var game = await this.ApplicationService.GetGameWithPlayersByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Game, GameIndexViewModel>(game));
        }

        [HttpPost]
        public async Task<IActionResult> CreateByPlayersName([FromBody] GameCreateViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SeriLogger.Information($"Game being created for Players {data.Player1Name} and {data.Player2Name}");

            if (data.Player1Name == data.Player2Name)
            {
                SeriLogger.Information(
                    $"Game creation failed for Player {data.Player1Name} because a player can't play against himself");
                return Conflict("A player cannot play against himself");
            }

            var created =
                await this.ApplicationService.CreateGameFromPlayerNamesAsync(data.Player1Name, data.Player2Name);

            if (created == null)
            {
                SeriLogger.Information(
                    $"Game creation failed for Players {data.Player1Name} and {data.Player2Name}");
                return StatusCode(500, ServerErrorMessage);
            }

            var gameToReturn = Mapper.Map<Game, GameIndexViewModel>(created);

            SeriLogger.Information(
                $"Game creation success for Players {data.Player1Name} and {data.Player2Name}. New Game has Id = {gameToReturn.Id}");

            return CreatedAtAction("Post", new {created.Id}, gameToReturn);
        }

        [HttpPatch]
        public async Task<IActionResult> CreateRound([FromBody] RoundCreateViewModel roundToCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            SeriLogger.Information($"Round being created for game with id = {roundToCreate.GameId}");

            var gameToAddRound = await this.ApplicationService.SingleOrDefaultAsync(roundToCreate.GameId);

            if (gameToAddRound == null)
            {
                SeriLogger.Information(
                    $"Round creation for game with id = {roundToCreate.GameId} failed because game could not be found");
                return NotFound(roundToCreate.GameId);
            }


            if (roundToCreate.Player1Move == roundToCreate.Player2Move)
            {
                SeriLogger.Information(
                    $"Round creation for game with id = {roundToCreate.GameId} failed because both players tried to the same move");
                return Conflict("This is a tie. For complete a round there must be a winner");
            }

            if (gameToAddRound.Player1RoundsWon >= gameToAddRound.RoundsToWin ||
                gameToAddRound.Player2RoundsWon == gameToAddRound.RoundsToWin)
            {
                SeriLogger.Information(
                    $"Round creation for game with id = {roundToCreate.GameId} failed because game is already over");

                return Conflict("The game was already finished");
            }


            var gameWithNewRound =
                await ApplicationService.CreateRoundAsync(Mapper.Map<RoundCreateViewModel, Round>(roundToCreate));

            if (gameWithNewRound == null)
            {
                SeriLogger.Information(
                    $"Round creation for game with id = {roundToCreate.GameId} failed");
                return StatusCode(500, ServerErrorMessage);
            }

            SeriLogger.Information(
                $"Player {gameWithNewRound.Player1.UserName} played {roundToCreate.Player1Move} in game with Id = {gameWithNewRound.Id}");

            SeriLogger.Information(
                $"Player {gameWithNewRound.Player2.UserName} played {roundToCreate.Player2Move} in game with Id = {gameWithNewRound.Id}");


            var roundNumber = gameWithNewRound.Rounds.Count;

            SeriLogger.Information(
                $"Round for game with id = {roundToCreate.GameId} was won with move {gameWithNewRound.Rounds[roundNumber - 1].WinnerMove}");

            SeriLogger.Information(
                $"There has being played {roundNumber} rounds for game with id = {gameWithNewRound.Id}");

            SeriLogger.Information(
                $"Round creation for game with id = {roundToCreate.GameId} succeed");

            return Ok(Mapper.Map<Game, GameIndexViewModel>(gameWithNewRound));
        }
    }
}