using GameOfDrones.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones.Core.Abstractions.DataAccess
{
    public interface IPlayerRepository: IRepository<Player, int>
    {
        Task<bool> ExistByUsernameAsync(string userName);

        Task<Player> FindByUsernameAsync(string userName);
    }
}
