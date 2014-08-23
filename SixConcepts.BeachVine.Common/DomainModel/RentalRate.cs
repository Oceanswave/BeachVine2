namespace SixConcepts.BeachVine.DomainModel
{
  using System;
  using System.Collections.Generic;
  using Raven.Imports.Newtonsoft.Json;

  public class RentalRateSnapshot
  {
    [JsonProperty("dateVisited")]
    public DateTime DateVisited
    {
      get;
      set;
    }
    [JsonProperty("startDate")]
    public DateTime StartDate
    {
      get;
      set;
    }

    [JsonProperty("endDate")]
    public DateTime EndDate
    {
      get;
      set;
    }

    [JsonProperty("previousRate")]
    public decimal? PreviousRate
    {
      get;
      set;
    }

    [JsonProperty("currentRate")]
    public decimal? CurrentRate
    {
      get;
      set;
    }

    [JsonProperty("reserveRate")]
    public decimal? ReserveRate
    {
      get;
      set;
    }

    [JsonProperty("remarks")]
    public string Remarks
    {
      get;
      set;
    }

    [JsonProperty("rentLink")]
    public string RentLink
    {
      get;
      set;
    }
  }

  public class RentalRate
  {
    public RentalRate()
    {
      RateHistory = new List<RentalRateHistory>();
    }

    [JsonProperty("startDate")]
    public DateTime StartDate
    {
      get;
      set;
    }

    [JsonProperty("endDate")]
    public DateTime EndDate
    {
      get;
      set;
    }

    [JsonProperty("rateHistory")]
    public List<RentalRateHistory> RateHistory
    {
      get;
      set;
    }

    [JsonProperty("status")]
    public RentalAvailability Status
    {
      get;
      set;
    }

    [JsonProperty("remarks")]
    public string Remarks
    {
      get;
      set;
    }

    [JsonProperty("rentLink")]
    public string RentLink
    {
      get;
      set;
    }
  }

  public class RentalRateHistory
  {
    [JsonProperty("dateRateUpdated")]
    public DateTime DateRateUpdated
    {
      get;
      set;
    }

    [JsonProperty("rate")]
    public decimal? Rate
    {
      get;
      set;
    }

  }
}