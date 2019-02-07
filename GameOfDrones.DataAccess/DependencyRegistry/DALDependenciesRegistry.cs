﻿using System;
using System.Collections.Generic;
using System.Text;
using GameOfDrones.Core.Abstractions.DataAccess;
using GameOfDrones.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameOfDrones.DataAccess.DependencyRegistry
{
    /// <summary>
    ///     Contains the functionalities to add the services
    ///     that are implemented in the DataAcces layer.
    /// </summary>
    /// <remarks>
    ///     When a service for the DI will be used, this won't
    ///     be necessary.
    /// </remarks>
    public static class ServiceCollectionExtension
    {
        public static void AddDataAccessServices(this IServiceCollection service)
        {
            service.AddScoped<DbContext, GameOfDronesContext>();
            service.AddScoped<IUnitOfWork, UnitOfWork.SqlUnitOfWork>();

            service.AddScoped<ISqlDbContext, GameOfDronesContext>();
        }
    }
}