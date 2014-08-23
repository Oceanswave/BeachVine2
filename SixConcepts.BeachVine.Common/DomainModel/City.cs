using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SixConcepts.BeachVine.DomainModel
{
  /// <summary>
  /// Represents a city. Contains a description, images and links.
  /// </summary>
  public class City
  {
    public string Id
    {
      get;
      set;
    }

    public string Name
    {
      get;
      set;
    }

    public string Description
    {
      get;
      set;
    }

    public string CityUrl
    {
      get;
      set;
    }
  }
}