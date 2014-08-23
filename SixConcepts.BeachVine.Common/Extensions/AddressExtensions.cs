namespace SixConcepts.BeachVine.Extensions
{
  using GoogleMapsApi;
  using GoogleMapsApi.Entities.Common;
  using GoogleMapsApi.Entities.Geocoding.Request;
  using GoogleMapsApi.Entities.Geocoding.Response;
  using SixConcepts.BeachVine.DomainModel;
  using System.Threading.Tasks;
  using System.Linq;

  public static class AddressExtensions
  {
    public static async Task UpdateCoordinates(this Address address)
    {
      var response = await GoogleMaps.Geocode.QueryAsync(new GeocodingRequest
        {
          //Address = FormattedAddress
        });
    }

    public static void PopulateFromCoordinates(this Address address, Coordinates coordinates)
    {
      if (coordinates == null || coordinates.Latitude == null || coordinates.Longitude == null)
        return;

      var response = GoogleMaps.Geocode.Query(new GeocodingRequest
        {
          Location = new Location(coordinates.Latitude.Value, coordinates.Longitude.Value)
        });

      var firstResult = response.Results.FirstOrDefault();
      if (firstResult == null)
        return;

      string value;
      if (TryGetComponentLongName(firstResult, "street_number", out value))
        address.StreetNumber = value;

      if (TryGetComponentLongName(firstResult, "route", out value))
        address.Route = value;

      if (TryGetComponentLongName(firstResult, "locality", out value))
        address.Locality = value;

      if (TryGetComponentLongName(firstResult, "administrative_area_level_3", out value))
        address.AdministrativeArea3 = value;

      if (TryGetComponentLongName(firstResult, "administrative_area_level_2", out value))
        address.AdministrativeArea2 = value;

      if (TryGetComponentLongName(firstResult, "administrative_area_level_1", out value))
        address.AdministrativeArea1 = value;

      if (TryGetComponentLongName(firstResult, "postal_code", out value))
        address.PostalCode = value;

      if (TryGetComponentLongName(firstResult, "country", out value))
        address.CountryRegion = value;

      if (TryGetComponentLongName(firstResult, "neighborhood", out value))
        address.Neighborhood = value;

      if (TryGetComponentLongName(firstResult, "point_of_interest", out value))
        address.Landmark = value;

      address.FormattedAddress = firstResult.FormattedAddress;
    }

    private static bool TryGetComponentLongName(Result result, string componentName, out string value)
    {
      var component = result.AddressComponents.FirstOrDefault(r => r.Types.Any(t => t == componentName));
      if (component != null && string.IsNullOrWhiteSpace(component.LongName) == false)
      {
        value = component.LongName;
        return true;
      }

      value = null;
      return false;
    }
  }
}
