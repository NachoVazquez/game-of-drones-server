using GameOfDrones.Business.ApplicationServices.Base;
using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.Core.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using GameOfDrones.Core.Domain;
using Serilog;
using Serilog.Core;

namespace GameOfDrones.Business.ApplicationServices
{
    /// <inheritdoc cref="IGameService" />
    public class GameService : BaseApplicationService<Game, int>, IGameService
    {
        public IPlayerRepository PlayerRepository { get; set; }
        public IRoundRepository RoundRepository { get; set; }

        public IGameRepository GameRepository { get; set; }

        private ILogger Logger { get; set; }

        public GameService(IGameRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            PlayerRepository = UnitOfWork.PlayerRepository;
            RoundRepository = UnitOfWork.RoundRepository;
            GameRepository = UnitOfWork.GameRepository;

            this.Logger = Log.ForContext<GameService>();
        }

        /// <inheritdoc />
        public async Task<Game> CreateGameFromPlayerNamesAsync(string player1Name, string player2Name,
            int roundsToWin = 3)
        {
            var player1 = await PlayerRepository.FindByUsernameAsync(player1Name);
            if (player1 == null)
            {
                this.Logger.Information($"Creating User with UserName = {player1Name}");
                player1 = await this.PlayerRepository.AddAsync(new Player
                    {UserName = player1Name, GamesWon = 0, GamesLost = 0, RoundsLost = 0, RoundsWon = 0});


                if (player1 == null)
                {
                    this.Logger.Information($"Creation of User with UserName = {player1Name} failed");
                    return null;
                }

                this.Logger.Information($"Creation of User with UserName = {player1Name} succeed");
            }

            var player2 = await PlayerRepository.FindByUsernameAsync(player2Name);
            if (player2 == null)
            {
                this.Logger.Information($"Creating User with UserName = {player2Name}");
                player2 = await this.PlayerRepository.AddAsync(new Player
                    {UserName = player2Name, GamesWon = 0, GamesLost = 0, RoundsLost = 0, RoundsWon = 0});


                if (player2 == null)
                {
                    this.Logger.Information($"Creation of User with UserName = {player2Name} failed");
                    return null;
                }

                this.Logger.Information($"Creation of User with UserName = {player2Name} succeed");
            }

            var gameToCreate = new Game
            {
                Player1Id = player1.Id, Player2Id = player2.Id, Player1RoundsWon = 0, Player2RoundsWon = 0,
                RoundsToWin = roundsToWin
            };

            var createdGame = await this.Repository.AddAsync(gameToCreate);

            try
            {
                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                return null;
            }

            createdGame.Player1 = player1;
            createdGame.Player2 = player2;


            return createdGame;
        }

        /// <inheritdoc />
        public async Task<Game> CreateRoundAsync(Round roundToCreate)
        {
            if (roundToCreate.Player1Move == roundToCreate.Player2Move)
            {
                return null;
            }

            var isPlayer1Winner = IsPlayer1RoundWinner(roundToCreate);

            roundToCreate.WinnerMove = isPlayer1Winner
                ? roundToCreate.Player1Move
                : roundToCreate.Player2Move;


            var createdRound = await RoundRepository.AddAsync(roundToCreate);

            var game = await GameRepository.GetGameWithPlayersByIdAsync(createdRound.GameId);

            this.Logger.Information(
                $"Round {game.Rounds.Count} of game with Id: {roundToCreate.GameId} was won by {(isPlayer1Winner ? game.Player1.UserName : game.Player2.UserName)}");


            var winner = isPlayer1Winner ? game.Player1 : game.Player2;
            var looser = !isPlayer1Winner ? game.Player1 : game.Player2;

            winner.RoundsWon++;
            looser.RoundsLost++;


            await this.PlayerRepository.UpdateAsync(winner);
            await this.PlayerRepository.UpdateAsync(looser);

            game = isPlayer1Winner
                ? winner.GamesAsPlayer1.First(gameX => game.Id == gameX.Id)
                : winner.GamesAsPlayer2.First(gameX => game.Id == gameX.Id);

            game.Player1RoundsWon += isPlayer1Winner ? 1 : 0;
            game.Player2RoundsWon += !isPlayer1Winner ? 1 : 0;

            if (game.RoundsToWin == (isPlayer1Winner ? game.Player1RoundsWon : game.Player2RoundsWon))
            {
                winner.GamesWon++;
                looser.RoundsLost++;
            }

            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            var res = await GameRepository.GetGameWithPlayersByIdAsync(createdRound.GameId);

            return res;
        }

        /// <inheritdoc />
        public async Task<Game> GetGameWithPlayersByIdAsync(int id)
        {
            return await GameRepository.GetGameWithPlayersByIdAsync(id);
        }


        private bool IsPlayer1RoundWinner(Round roundToCreate)
        {
            switch (roundToCreate.Player1Move)
            {
                case PlayerMoves.Rock:
                    return roundToCreate.Player2Move == PlayerMoves.Scissor;
                case PlayerMoves.Scissor:
                    return roundToCreate.Player2Move == PlayerMoves.Paper;
                case PlayerMoves.Paper:
                    return roundToCreate.Player2Move == PlayerMoves.Rock;
                default:
                    throw new ArgumentException("Unsupported Move");
            }
        }
    }
}