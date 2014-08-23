namespace SixConcepts.BeachVine
{
  using System;
  using SixConcepts.BeachVine.DomainModel;

  public interface IRateChangeNotifier
  {
    void NotifyOfNewRate(Listing listing, DateTime startDate, DateTime endDate, decimal? newRate);

    void NotifyOfNewHoldRate(Listing listing, DateTime startDate, DateTime endDate, decimal? holdRate);

    void NotifyOfRateChange(Listing listing, DateTime startDate, DateTime endDate, decimal? oldRate, decimal? newRate);

    void NotifyOfAvailablityChange(Listing listing, DateTime startDate, DateTime endDate, RentalAvailability oldAvailability, RentalAvailability newAvailability);
  }
}
