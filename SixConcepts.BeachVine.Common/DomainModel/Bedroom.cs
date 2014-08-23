using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SixConcepts.BeachVine.DomainModel
{
  public class Bedroom
  {
    public string Type
    {
      get;
      set;
    }

    public string Description
    {
      get;
      set;
    }

    public IEnumerable<Image> Images
    {
      get;
      set;
    }
  }
}