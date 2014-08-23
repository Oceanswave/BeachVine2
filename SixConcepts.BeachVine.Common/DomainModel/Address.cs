namespace SixConcepts.BeachVine.DomainModel
{
  using Raven.Imports.Newtonsoft.Json;

  /// <summary>
  /// Details about a point on the Earth that has additional location information.
  /// </summary>
  public class Address
  {
    /// <summary>
    /// Indicates the precise street number.
    /// </summary>
    /// <example>
    /// 117
    /// </example>
    [JsonProperty("streetNumber")]
    public string StreetNumber
    {
      get;
      set;
    }

    /// <summary>
    /// Indicates a named route (such as "US 101").
    /// </summary>
    /// <example>
    /// Microsoft Way
    /// </example>
    [JsonProperty("route")]
    public string Route
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the subdivision name in the country or region for an address.
    /// This element is typically treated as the first order administrative subdivision,
    /// but in some cases it is the second, third, or fourth order subdivision in a
    /// country, dependency, or region
    /// </summary>
    /// <example>
    /// WA
    /// </example>
    [JsonProperty("administrativeArea1")]
    public string AdministrativeArea1
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the subdivision name in the country or region for an address.
    /// This element is used when there is another level of subdivision information for a
    /// location, such as the county.
    /// </summary>
    /// <example>
    /// King Co.
    /// </example>
    [JsonProperty("administrativeArea2")]
    public string AdministrativeArea2
    {
      get;
      set;
    }

    /// <summary>
    /// A string indicating a third-order civil entity below the country level.
    /// This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    /// </summary>
    /// <example>
    /// Popular Branch
    /// </example>
    [JsonProperty("administrativeArea3")]
    public string AdministrativeArea3
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the country or region name of an address.
    /// </summary>
    /// <example>
    /// United States
    /// </example>
    [JsonProperty("countryRegion")]
    public string CountryRegion
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the complete address. This address may not include the country or region.
    /// </summary>
    /// <example>
    /// 1 Microsoft Way, Redmond, WA 98052-8300
    /// </example>
    [JsonProperty("formattedAddress")]
    public string FormattedAddress
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the name of the landmark when there is a landmark associated with an address.
    /// </summary>
    /// <example>
    /// Eiffel Tower
    /// </example>
    [JsonProperty("landmark")]
    public string Landmark
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the populated place for the address. This typically refers to a city,
    /// but may refer to a suburb or a neighborhood in certain countries.
    /// </summary>
    /// <example>
    /// Seattle
    /// </example>
    [JsonProperty("locality")]
    public string Locality
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the neighborhood for an address.
    /// </summary>
    /// <example>
    /// Ballard
    /// </example>
    [JsonProperty("neighborhood")]
    public string Neighborhood
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the area for an address.
    /// </summary>
    /// <example>
    /// Westside
    /// Oceanfront
    /// </example>
    [JsonProperty("area")]
    public string Area
    {
      get;
      set;
    }

    /// <summary>
    /// A string specifying the post code, postal code, or ZIP Code of an address.
    /// </summary>
    /// <example>
    /// 98178
    /// </example>
    [JsonProperty("postalCode")]
    public string PostalCode
    {
      get;
      set;
    }
  }
}