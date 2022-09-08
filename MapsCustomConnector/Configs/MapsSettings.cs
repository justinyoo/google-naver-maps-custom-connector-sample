namespace MapsCustomConnector.Configs
{
    /// <summary>
    /// This represents the app settings entity for map services.
    /// </summary>
    public class MapsSettings
    {
        /// <summary>
        /// Gets the app settings attribute name.
        /// </summary>
        public const string Name = "Maps";

        /// <summary>
        /// Gets or sets the <see cref="GoogleMapsSettings"/> instance.
        /// </summary>
        public virtual GoogleMapsSettings Google { get; set; } = new GoogleMapsSettings();

        /// <summary>
        /// Gets or sets the <see cref="NaverMapsSettings"/> instance.
        /// </summary>
        public virtual NaverMapsSettings Naver { get; set; } = new NaverMapsSettings();
    }

    /// <summary>
    /// This represents the app settings entity for Google Maps API.
    /// </summary>
    public class GoogleMapsSettings
    {
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        public virtual string ApiKey { get; set; }
    }

    /// <summary>
    /// This represents the app settings entity for Nave Map API.
    /// </summary>
    public class NaverMapsSettings
    {
        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public virtual string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public virtual string ClientSecret { get; set; }
    }
}