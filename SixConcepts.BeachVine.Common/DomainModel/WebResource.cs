namespace SixConcepts.BeachVine.DomainModel
{
  using System;

  public class WebResource
  {
    public string Id
    {
      get;
      set;
    }

    public string Url
    {
      get;
      set;
    }

    public DateTimeOffset DateAdded
    {
      get;
      set;
    }

    public DateTimeOffset? DateRetrieved
    {
      get;
      set;
    }

    public string MimeType
    {
      get;
      set;
    }
  }
}
