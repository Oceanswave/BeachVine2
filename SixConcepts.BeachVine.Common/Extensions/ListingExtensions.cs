namespace SixConcepts.BeachVine.Extensions
{
  using System.Collections.Generic;
  using SixConcepts.BeachVine.DomainModel;
  using System;
  using System.Linq;

  public static class ListingExtensions
  {
    public static void UpdateRatesFromSnapshot(this Listing listing, IRateChangeNotifier rateChangeNotifier)
    {
      if (listing.Rates == null)
        listing.Rates = new List<RentalRate>();

      //Look for new rates and update existing rates.
      foreach (var currentRate in listing.RatesSnapshot)
      {
        //Using
        var existingRate =
          listing.Rates.SingleOrDefault(r => r.StartDate == currentRate.StartDate && r.EndDate == currentRate.EndDate);

        //If we didn't find an existing rate, create a new one and add it to the listing.
        if (existingRate == null)
        {
          var newRate = new RentalRate
            {
              StartDate = currentRate.StartDate,
              EndDate = currentRate.EndDate,
              Remarks = currentRate.Remarks,
              RentLink = currentRate.RentLink,
              Status = RentalAvailability.Available
            };

          if (currentRate.PreviousRate.HasValue && currentRate.PreviousRate.Value > 0)
          {
            newRate.RateHistory.Add(new RentalRateHistory
              {
                DateRateUpdated = DateTime.UtcNow,
                Rate = currentRate.PreviousRate
              });

            //if (rateChangeNotifier != null)
            //  rateChangeNotifier.NotifyOfNewRate(listing, currentRate.StartDate, currentRate.EndDate, currentRate.PreviousRate.Value);
          }

          if (currentRate.CurrentRate.HasValue && currentRate.CurrentRate.Value > 0)
          {
            newRate.RateHistory.Add(new RentalRateHistory
              {
                DateRateUpdated = DateTime.UtcNow,
                Rate = currentRate.CurrentRate
              });

            if (rateChangeNotifier != null)
              rateChangeNotifier.NotifyOfNewRate(listing, currentRate.StartDate, currentRate.EndDate, currentRate.CurrentRate.Value);
          }

          if (currentRate.ReserveRate.HasValue && currentRate.ReserveRate.Value > 0)
          {
            newRate.RateHistory.Add(new RentalRateHistory
              {
                DateRateUpdated = DateTime.UtcNow,
                Rate = currentRate.ReserveRate
              });

            if (rateChangeNotifier != null)
              rateChangeNotifier.NotifyOfNewHoldRate(listing, currentRate.StartDate, currentRate.EndDate, currentRate.ReserveRate.Value);
          }

          listing.Rates.Add(newRate);
        }
        else
        {
          //If we found an existing one, and the rate doesn't match the most recent, add the rate to the stack.
          var mostRecentRate = existingRate.RateHistory.Last();
          if (currentRate.CurrentRate.HasValue && currentRate.CurrentRate > 0 && mostRecentRate.Rate != currentRate.CurrentRate)
          {
            existingRate.RateHistory.Add(new RentalRateHistory
              {
                DateRateUpdated = DateTime.UtcNow,
                Rate = currentRate.CurrentRate
              });

            if (rateChangeNotifier != null && existingRate.Status != RentalAvailability.Available)
              rateChangeNotifier.NotifyOfAvailablityChange(listing, currentRate.StartDate, currentRate.EndDate, existingRate.Status, RentalAvailability.Available);

            existingRate.Status = RentalAvailability.Available;
          }
        }
      }

      //Look for any rates that no longer exist in the snapshot and mark those as rented.
      foreach (var rate in listing.Rates.Where(r => r.Status == RentalAvailability.Available))
      {
        var rateInSnapshot =
          listing.RatesSnapshot.SingleOrDefault(r => r.StartDate == rate.StartDate && r.EndDate == rate.EndDate);

        if (rateInSnapshot != null)
          continue;

        if (rateChangeNotifier != null && rate.Status != RentalAvailability.Available)
          rateChangeNotifier.NotifyOfAvailablityChange(listing, rate.StartDate, rate.EndDate, rate.Status, RentalAvailability.Rented);

        rate.Status = RentalAvailability.Rented;
      }
    }
  }
}
