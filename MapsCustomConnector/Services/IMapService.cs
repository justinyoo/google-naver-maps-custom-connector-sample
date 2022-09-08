using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace MapsCustomConnector.Services
{
    /// <summary>
    /// This provides interfaces to map services.
    /// </summary>
    public interface IMapService
    {
        /// <summary>
        /// Gets the map image as the byte array format.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns the map image as the byte array format.</returns>
        Task<byte[]> GetMapAsync(HttpRequest req);
    }
}