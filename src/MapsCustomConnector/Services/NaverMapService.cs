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
    /// This represents the service entity for Naver Map.
    /// </summary>
    public class NaverMapService : IMapService
    {
        public const string Name = "Naver";

        private readonly MapsSettings _settings;
        private readonly HttpClient _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaverMapService"/> class.
        /// </summary>
        /// <param name="settings"><see cref="MapsSettings"/> instance.</param>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        public NaverMapService(MapsSettings settings, IHttpClientFactory factory)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("naver");
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetMapAsync(HttpRequest req)
        {
            var latitude = req.Query["lat"];
            var longitude = req.Query["long"];
            var zoom = (string)req.Query["zoom"] ?? "13";

            var sb = new StringBuilder();
            sb.Append("https://naveropenapi.apigw.ntruss.com/map-static/v2/raster")
              .Append($"?center={longitude},{latitude}")
              .Append("&w=400")
              .Append("&h=400")
              .Append($"&level={zoom}")
              .Append($"&markers=color:blue|pos:{longitude}%20{latitude}")
              .Append("&format=png")
              .Append("&lang=en");
            var requestUri = new Uri(sb.ToString());

            this._http.DefaultRequestHeaders.Clear();
            this._http.DefaultRequestHeaders.Add("X-NCP-APIGW-API-KEY-ID", this._settings.Naver.ClientId);
            this._http.DefaultRequestHeaders.Add("X-NCP-APIGW-API-KEY", this._settings.Naver.ClientSecret);

            var bytes = await this._http.GetByteArrayAsync(requestUri).ConfigureAwait(false);

            return bytes;
        }
    }
}