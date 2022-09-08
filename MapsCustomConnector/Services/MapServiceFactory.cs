using System.Collections.Generic;
using System.Net.Http;

using MapsCustomConnector.Configs;

namespace MapsCustomConnector.Services
{
    /// <summary>
    /// This represents the factory entity for <see cref="IMapService"/> instances.
    /// </summary>
    public class MapServiceFactory : IMapServiceFactory
    {
        private readonly Dictionary<string, IMapService> _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapServiceFactory"/> class.
        /// </summary>
        /// <param name="settings"><see cref="MapsSettings"/> instance.</param>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        public MapServiceFactory(MapsSettings settings, IHttpClientFactory factory)
        {
            this._services = new Dictionary<string, IMapService>()
            {
                { GoogleMapService.Name, new GoogleMapService(settings, factory) },
                { NaverMapService.Name, new NaverMapService(settings, factory) },
            };
        }

        /// <inheritdoc/>
        public IMapService GetMapService(string name)
        {
            if (!this._services.ContainsKey(name))
            {
                return null;
            }

            return this._services[name];
        }
    }
}