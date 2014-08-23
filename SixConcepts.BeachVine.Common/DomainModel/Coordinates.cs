namespace SixConcepts.BeachVine.DomainModel
{
  using Raven.Imports.Newtonsoft.Json;

  /// <summary>
    /// A point on the Earth specified by a latitude and longitude.
    /// </summary>
  public class Coordinates
  {
    [JsonProperty("latitude")]
    public double? Latitude
    {
      get;
      set;
    }

    [JsonProperty("longitude")]
    public double? Longitude
    {
      get;
      set;
    }

    [JsonProperty("altitude")]
    public double? Altitude
    {
      get;
      set;
    }
  }
}