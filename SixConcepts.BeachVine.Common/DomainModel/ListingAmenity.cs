namespace SixConcepts.BeachVine.DomainModel
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;

  public class ListingAmenity : Amenity
  {

    public IList<ListingImage> Images
    {
      get;
      set;
    }
  }
}