namespace SixConcepts.BeachVine.DomainModel
{
  using Raven.Imports.Newtonsoft.Json;

  public class Neighborhood
  {
    [JsonProperty("name")]
    public string Name
    {
      get;
      set;
    }

    [JsonProperty("city")]
    public string City
    {
      get;
      set;
    }

    [JsonProperty("description")]
    public string Description
    {
      get;
      set;
    }
  }
}