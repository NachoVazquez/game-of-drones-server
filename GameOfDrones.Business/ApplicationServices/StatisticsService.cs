using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameOfDrones.Core.Abstractions.Business;
using GameOfDrones.Core.Abstractions.DataAccess;

namespace GameOfDrones.Business.ApplicationServices
{
    public class StatisticsService : IStatisticsService
    {
        public IUnitOfWork UnitOfWork { get; }
        private IPlayerRepository PlayersRepository { get; set; }
        private IGameRepository GamesRepository { get; set; }
        private IRoundRepository RoundsRepository { get; set; }

        public StatisticsService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            PlayersRepository = unitOfWork.PlayerRepository;
            GamesRepository = unitOfWork.GameRepository;
            RoundsRepository = unitOfWork.RoundRepository;
        }

        public async Task<long?> GetRegisteredPlayersAsync()
        {
            try
            {
                return await this.PlayersRepository.GetPlayersRegisteredAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<long?> GetGamesPlayed()
        {
            try
            {
                return await this.GamesRepository.GetGamesPlayedAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<long?> GetRoundsPlayed()
        {
            try
            {
                return await this.RoundsRepository.GetRoundsPlayed();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}