namespace SixConcepts.BeachVine.DomainModel
{
  using System;
  using System.Collections.Generic;
  using Raven.Imports.Newtonsoft.Json;

  /// <summary>
  /// A Realty listing.
  /// </summary>
  public class Listing
  {
    public Listing()
    {
      Address = new Address();
      Rates = new List<RentalRate>();
      RatesSnapshot = new List<RentalRateSnapshot>();
    }

    public string Id
    {
      get;
      set;
    }

    /// <summary>
    /// Original Url of the listing.
    /// </summary>
    [JsonProperty("url")]
    public string Url
    {
      get;
      set;
    }

    /// <summary>
    /// The type of listing.
    /// </summary>
    /// <example>
    /// ForSale,
    /// Rent,
    /// Could be Hosted Rental, Sponsored, etc.
    /// </example>
    [JsonProperty("type")]
    public string Type
    {
      get;
      set;
    }

    /// <summary>
    /// The realty agency that is hosting the listing.
    /// </summary>
    [JsonProperty("realtyAgency")]
    public string RealtyAgency
    {
        get;
        set;
    }

    /// <summary>
    /// The realtor specific reference number (e.g. Carolina Designs uses a straight number, other sites use combinations of numbers and letters.)
    /// </summary>
    [JsonProperty("realtorReferenceNumber")]
    public string RealtorReferenceNumber
    {
      get;
      set;
    }

    /// <summary>
    /// The name of the listing.
    /// </summary>
    [JsonProperty("propertyName")]
    public string PropertyName
    {
      get;
      set;
    }

    /// <summary>
    /// A short summary of the listing.
    /// </summary>
    [JsonProperty("summary")]
    public string Summary
    {
      get;
      set;
    }

    /// <summary>
    /// The rich-text description of the listing.
    /// </summary>
    [JsonProperty("description")]
    public string Description
    {
      get;
      set;
    }

    /// <summary>
    /// A textual description of the bedrooms that the listing provides.
    /// </summary>
    [JsonProperty("bedroomsDescription")]
    public string BedroomsDescription
    {
      get;
      set;
    }

    /// <summary>
    /// A textual description of the bathrooms that the listing provides.
    /// </summary>
    [JsonProperty("bathroomsDescription")]
    public string BathroomsDescription
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the type of property: House, Condo, etc.
    /// </summary>
    [JsonProperty("propertyType")]
    public string PropertyType
    {
      get;
      set;
    }

    /// <summary>
    /// Location relative to the beach...
    /// Oceanfront, etc...
    /// </summary>
    [JsonProperty("location")]
    public string Location
    {
      get;
      set;
    }

    [JsonProperty("address")]
    public Address Address
    {
      get;
      set;
    }

    [JsonProperty("coordinates")]
    public Coordinates Coordinates
    {
      get;
      set;
    }

    [JsonProperty("mainImage")]
    public ListingImage MainImage
    {
      get;
      set;
    }

    [JsonProperty("images")]
    public IList<ListingImage> Images
    {
      get;
      set;
    }

    [JsonProperty("ratesSnapshot")]
    public IList<RentalRateSnapshot> RatesSnapshot
    {
      get;
      set;
    }

    [JsonProperty("rates")]
    public IList<RentalRate> Rates
    {
      get;
      set;
    }

    [JsonProperty("amenities")]
    public IList<ListingAmenity> Amenities
    {
      get;
      set;
    }

    [JsonProperty("bedrooms")]
    public IList<Bedroom> Bedrooms
    {
      get;
      set;
    }

    [JsonProperty("bathrooms")]
    public IList<Bathroom> Bathrooms
    {
      get;
      set;
    }

    [JsonProperty("firstVisited")]
    public DateTimeOffset? FirstVisited
    {
      get;
      set;
    }

    [JsonProperty("lastVisited")]
    public DateTimeOffset? LastVisited
    {
      get;
      set;
    }
  }
}