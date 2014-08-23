namespace SixConcepts.BeachVine.DomainModel
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;

  /// <summary>
  /// Represents a real estate agency.
  /// </summary>
  public class Agency
  {
    /// <summary>
    /// Agency Id (Internal)
    /// </summary>
    public string Id
    {
      get;
      set;
    }


    /// <summary>
    /// Name of the agency.
    /// </summary>
    public string Name
    {
      get;
      set;
    }


    /// <summary>
    /// Description of the agency.
    /// </summary>
    public string Description
    {
      get;
      set;
    }

    /// <summary>
    /// Url of the agency's web site.
    /// </summary>
    public string SiteUrl
    {
      get;
      set;
    }
  }
}