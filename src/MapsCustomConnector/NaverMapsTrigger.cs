using System;
using System.Net;
using System.Threading.Tasks;

using MapsCustomConnector.Models;
using MapsCustomConnector.Services;

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
        private readonly IMapService _service;
        private readonly ILogger<NaverMapsTrigger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaverMapsTrigger"/> class.
        /// </summary>
        /// <param name="factory"><see cref="IMapServiceFactory"/> instance.</param>
        /// <param name="log"><see cref="ILogger{TCategoryName}"/> instance.</param>
        public NaverMapsTrigger(IMapServiceFactory factory, ILogger<NaverMapsTrigger> log)
        {
            this._service = factory.ThrowIfNullOrDefault().GetMapService(NaverMapService.Name);
            this._logger = log.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Invokes the endpoint that returns the base64-encoded map image.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns <see cref="OkObjectResult"/> that contains the base64-encoded image.</returns>
        [FunctionName(nameof(NaverMapsTrigger.GetNaverMap))]
        [OpenApiOperation(operationId: nameof(NaverMapsTrigger.GetNaverMap), tags: new[] { "naver" })]
        [OpenApiParameter(name: "lat", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **latitude** parameter")]
        [OpenApiParameter(name: "long", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **longitude** parameter")]
        [OpenApiParameter(name: "zoom", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The **zoom level** parameter &ndash; Default value is `13`")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(MapData), Description = "The base64-encoded map image as an OK response")]
        public async Task<IActionResult> GetNaverMap(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "naver")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            var bytes = await this._service.GetMapAsync(req).ConfigureAwait(false);
            var result = new MapData() { Base64Image = $"data:image/png;base64,{Convert.ToBase64String(bytes)}" };

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Invokes the endpoint that returns the map image as the binary format.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns <see cref="FileContentResult"/> as the <c>image/png</c> format.</returns>
        [FunctionName(nameof(NaverMapsTrigger.GetNaverMapImage))]
        [OpenApiOperation(operationId: nameof(NaverMapsTrigger.GetNaverMapImage), tags: new[] { "naver" })]
        [OpenApiParameter(name: "lat", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **latitude** parameter")]
        [OpenApiParameter(name: "long", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **longitude** parameter")]
        [OpenApiParameter(name: "zoom", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The **zoom level** parameter &ndash; Default value is `13`")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/png", bodyType: typeof(byte[]), Description = "The map image as an OK response")]
        public async Task<IActionResult> GetNaverMapImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "naver/image")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            var bytes = await this._service.GetMapAsync(req).ConfigureAwait(false);

            return new FileContentResult(bytes, "image/png");
        }
    }
}