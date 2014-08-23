namespace SixConcepts.BeachVine.DomainModel
{
  using Raven.Imports.Newtonsoft.Json;

  public class Image
  {
    [JsonProperty("title")]
    public string Title
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

    [JsonProperty("category")]
    public string Category
    {
      get;
      set;
    }

    [JsonProperty("originalUrl")]
    public string OriginalUrl
    {
      get;
      set;
    }
  }
}
