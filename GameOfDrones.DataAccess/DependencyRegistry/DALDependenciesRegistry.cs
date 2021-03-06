﻿using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.DataAccess.Contexts;
using GameOfDrones.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameOfDrones.DataAccess.DependencyRegistry
{
    /// <summary>
    ///     Contains the functionalities to add the services
    ///     that are implemented in the DataAccess layer.
    /// </summary>   
    public static class ServiceCollectionExtension
    {
        public static void AddDataAccessServices(this IServiceCollection service)
        {
            service.AddScoped<DbContext, GameOfDronesContext>();
            service.AddScoped<IUnitOfWork, UnitOfWork.SqlUnitOfWork>();

            service.AddScoped<ISqlDbContext, GameOfDronesContext>();

            service.AddScoped<IGameRepository, GameRepository>();
            service.AddScoped<IRoundRepository, RoundRepository>();
            service.AddScoped<IPlayerRepository, PlayerRepository>();

        }
    }
}