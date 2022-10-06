namespace MapsCustomConnector.Services
{
    /// <summary>
    /// This provides interfaces to <see cref="MapServiceFactory"/>.
    /// </summary>
    public interface IMapServiceFactory
    {
        /// <summary>
        /// Gets the <see cref="IMapService"/> instance based on the instance name.
        /// </summary>
        /// <param name="name">Instance name.</param>
        /// <returns>Returns the <see cref="IMapService"/> instance.</returns>
        IMapService GetMapService(string name);
    }
}