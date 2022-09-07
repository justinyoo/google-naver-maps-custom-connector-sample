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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace MapsCustomConnector
{
    /// <summary>
    /// This represents the HTTP trigger entity for Google Maps API.
    /// </summary>
    public class GoogleMapsTrigger
    {
        private readonly MapsSettings _settings;
        private readonly HttpClient _http;
        private readonly ILogger<GoogleMapsTrigger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMapsTrigger"/> class.
        /// </summary>
        /// <param name="settings"><see cref="MapsSettings"/> instance.</param>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        /// <param name="log"><see cref="ILogger{TCategoryName}"/> instance.</param>
        public GoogleMapsTrigger(MapsSettings settings, IHttpClientFactory factory, ILogger<GoogleMapsTrigger> log)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("google");
            this._logger = log.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Invokes the endpoint.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns <see cref="FileContentResult"/> as the <c>image/png</c> format.</returns>
        [FunctionName(nameof(GoogleMapsTrigger))]
        [OpenApiOperation(operationId: "GetGoogleMap", tags: new[] { "maps" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "lat", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **latitude** parameter")]
        [OpenApiParameter(name: "long", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **longitude** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "google")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var latitude = req.Query["lat"];
            var longitude = req.Query["long"];

            var sb = new StringBuilder();
            sb.Append("https://maps.googleapis.com/maps/api/staticmap")
              .Append($"?center={latitude},{longitude}")
              .Append("&size=400x400")
              .Append("&zoom=14")
              .Append($"&markers=color:red|{latitude},{longitude}")
              .Append("&format=png32")
              .Append($"&key={this._settings.Google.ApiKey}");
            var requestUri = new Uri(sb.ToString());

            var result = await this._http.GetByteArrayAsync(requestUri).ConfigureAwait(false);

            return new FileContentResult(result, "image/png");
        }
    }
}