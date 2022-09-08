using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using MapsCustomConnector.Configs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MapsCustomConnector
{
    /// <summary>
    /// This represents the HTTP trigger entity for Naver Map API.
    /// </summary>
    public class NaverMapsTrigger
    {
        private readonly MapsSettings _settings;
        private readonly HttpClient _http;
        private readonly ILogger<NaverMapsTrigger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaverMapsTrigger"/> class.
        /// </summary>
        /// <param name="settings"><see cref="MapsSettings"/> instance.</param>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        /// <param name="log"><see cref="ILogger{TCategoryName}"/> instance.</param>
        public NaverMapsTrigger(MapsSettings settings, IHttpClientFactory factory, ILogger<NaverMapsTrigger> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("naver");
            this._logger = log.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Invokes the endpoint.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns <see cref="FileContentResult"/> as the <c>image/png</c> format.</returns>
        [FunctionName(nameof(NaverMapsTrigger))]
        [OpenApiOperation(operationId: "GetNaverMap", tags: new[] { "maps" })]
        [OpenApiParameter(name: "lat", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **latitude** parameter")]
        [OpenApiParameter(name: "long", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **longitude** parameter")]
        [OpenApiParameter(name: "zoom", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The **zoom level** parameter &ndash; Default value is `13`")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]), Description = "The map image as an OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "naver")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

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

            var result = await this._http.GetByteArrayAsync(requestUri);

            return new FileContentResult(result, "image/png");
        }
    }
}