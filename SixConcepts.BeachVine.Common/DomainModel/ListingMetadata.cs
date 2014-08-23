using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SixConcepts.BeachVine.DomainModel
{
  public class ListingMetadata
  {
    public string Id
    {
      get;
      set;
    }

    public string ListingId
    {
      get;
      set;
    }

    public ZillowPropertyData ZillowPropertyData
    {
      get;
      set;
    }

    public DateTime Created
    {
      get;
      set;
    }

    public DateTime? LastModified
    {
      get;
      set;
    }
  }
}