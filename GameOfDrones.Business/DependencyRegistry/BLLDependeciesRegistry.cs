using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace GameOfDrones.Business.DependencyRegistry
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
        public static void AddBusinessServices(this IServiceCollection service)
        {
        }
    }
}