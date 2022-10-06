using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using MapsCustomConnector.Configs;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace MapsCustomConnector.Services
{
    /// <summary>
    /// This represents the service entity for Naver Maps.
    /// </summary>
    public class GoogleMapService : IMapService
    {
        public const string Name = "Google";

        private readonly MapsSettings _settings;
        private readonly HttpClient _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMapService"/> class.
        /// </summary>
        /// <param name="settings"><see cref="MapsSettings"/> instance.</param>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        public GoogleMapService(MapsSettings settings, IHttpClientFactory factory)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("google");
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetMapAsync(HttpRequest req)
        {
            var latitude = req.Query["lat"];
            var longitude = req.Query["long"];
            var zoom = (string)req.Query["zoom"] ?? "14";

            var sb = new StringBuilder();
            sb.Append("https://maps.googleapis.com/maps/api/staticmap")
              .Append($"?center={latitude},{longitude}")
              .Append("&size=400x400")
              .Append($"&zoom={zoom}")
              .Append($"&markers=color:red|{latitude},{longitude}")
              .Append("&format=png32")
              .Append($"&key={this._settings.Google.ApiKey}");
            var requestUri = new Uri(sb.ToString());

            var bytes = await this._http.GetByteArrayAsync(requestUri).ConfigureAwait(false);

            return bytes;
        }
    }
}