namespace SixConcepts.BeachVine.DomainModel
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;

  /// <summary>
  /// Represents an amentity that a rental house provides.
  /// </summary>
  public class Amenity
  {

    public string Name
    {
      get;
      set;
    }

    public string Category
    {
      get;
      set;
    }
  }
}