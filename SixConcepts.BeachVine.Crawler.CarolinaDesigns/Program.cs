namespace SixConcepts.BeachVine.Crawler.CarolinaDesigns
{
  using OpenQA.Selenium.PhantomJS;
  using SixConcepts.BeachVine.DomainModel;
  using SixConcepts.BeachVine.Extensions;
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.Linq;
  using System.Threading;

  class Program
  {
    private static TwitterNotifier s_notifier;

    static void Main(string[] args)
    {
      UserAgent = ConfigurationManager.AppSettings["UserAgent"];

      using (var phantomJsService = PhantomJSDriverService.CreateDefaultService())
      {
        var phantomJsOptions = new PhantomJSOptions();
        phantomJsOptions.AddAdditionalCapability("phantomjs.page.settings.userAgent", UserAgent);
        phantomJsOptions.AddAdditionalCapability("phantomjs.page.settings.loadImages", "false");

        s_notifier = new TwitterNotifier();
        s_notifier.Initialize(phantomJsService, phantomJsOptions);

        using (var phantomJsDriverPool = new ResourcePool<PhantomJSDriver>(pool => pool.Count > 5
                                                                                       ? null
                                                                                       : new PhantomJSDriver(phantomJsService, phantomJsOptions)))
        {
          //Obtain all listings
          List<dynamic> listingInfos = GetAllListingInfos(phantomJsDriverPool).ToObject<List<dynamic>>();
          listingInfos.Shuffle();

          var listings =
            listingInfos.Select(listingInfo => GetListing(phantomJsDriverPool, listingInfo.number.Value))
                        .Cast<Listing>()
                        .ToList();

          var first = args.FirstOrDefault();
          if (first != null && first.ToLowerInvariant() == "retrieveimages")
          {
            foreach (var listing in listings)
            {
              RavenHelpers.RetrieveListingImages(listing);
            }
          }
        }
      }
    }

    protected static string UserAgent
    {
      get;
      set;
    }

    protected static dynamic GetAllListingInfos(ResourcePool<PhantomJSDriver> driverPool)
    {
      using (var driverPoolItem = driverPool.GetItem())
      {
        var driver = driverPoolItem.Resource;

        driver.Navigate().GoToUrl("http://www.carolinadesigns.com/our-outer-banks-rentals.aspx");

        if (driver.IsJQueryEnabled() == false)
          driver.JQuerify();

        var result = driver.ExecuteScriptWithResult(@"
var result = [];
jQuery('select[name$=ddlPropNames]')
  .children()
  .each(function(i) { 
    var vals =
      {
        'name': jQuery(this).text(), 
        'number': jQuery(this).attr('value')
      };

    if (vals.number != -1)
      result.push(vals);
  });
return result;
");
        return result;
      }
    }

    protected static Listing GetListing(ResourcePool<PhantomJSDriver> driverPool, string number)
    {
      Listing result;

      using (var driverPoolItem = driverPool.GetItem())
      {
        var driver = driverPoolItem.Resource;

        Listing existingListing;
        using (var session = RavenHelpers.GetSession())
        {
          existingListing = session.Query<Listing>()
                 .FirstOrDefault(l => l.RealtyAgency == "Carolina Designs" && l.RealtorReferenceNumber == number);
        }

        if (existingListing == null)
        {
          driver.Navigate().GoToUrl("http://www.carolinadesigns.com/our-outer-banks-rentals.aspx");

          driver.ExecuteScript(String.Format(@"jQuery('select[name$=ddlPropNames]').val('{0}').trigger('change');",
                                             number));

          //Wait a while for the url to change...
          do
          {
            Console.WriteLine("Waiting for redirect to url...");
            Thread.Sleep(1000);
          } while (driver.Url == "http://www.carolinadesigns.com/our-outer-banks-rentals.aspx");
        }
        else
        {
          if (existingListing.LastVisited.HasValue && existingListing.LastVisited > DateTime.Now.AddMinutes(-20))
          {
            Console.WriteLine("Skipping {0}: Crawled in the last 20 minutes.", existingListing.PropertyName);
            return existingListing;
          }

          driver.Navigate().GoToUrl(existingListing.Url);
        }

        //JQuerify the page...
        if (driver.IsJQueryEnabled() == false)
          driver.JQuerify();

        driver.Uriify();
        driver.Sugarify();

        Console.WriteLine("Crawling " + driver.Url + "...");
        const string crawlScript = @"
var result = {};
var titleRegex = /#(\d+)\s*-\s*(.+)$/g;
var titleFullText = jQuery('h1').text();
var titleMatches = titleRegex.exec(titleFullText);

result.realtorReferenceNumber = titleMatches[1];
result.propertyName = titleMatches[2];
result.description = jQuery('div#MainContent_divDesc').text();
result.type = esmcategory;
result.url = location.href;

result.images = [];
jQuery('#bodyContainer .ad-nav a').each(function() {
  var imgElement = jQuery(this).find('img');

  var image = {};
  image.originalUrl = jQuery(this).attr('href');
  image.title = imgElement.attr('data-description').replace(imgElement.attr('title') + ' ', '');

  result.images.push(image); 
});

//Get rental rates..
var now = new Date();
var ratesSnapshot = [];
jQuery('table.avail-table:eq(0)').find('table:has(tr.table-top)').find('tr:gt(1)').each(function(key, value) {
  var rate = {};
  rate.dateVisited = now.toISOString();
  rate.startDate = Date.create(jQuery(value).find('td:eq(0)').text().trim().replace(/^.*?(?=[0-9]+\/[0-9]+\/[0-9]+)/g,'')).toISOString();
  rate.endDate = Date.create(rate.startDate).addDays(5).toISOString();
  rate.previousRate = jQuery(value).find('td:eq(1)').text().trim();
  rate.previousRate = Number(rate.previousRate.replace(/[^0-9\.]+/g,''));
  rate.currentRate = jQuery(value).find('td:eq(2)').text().trim();
  rate.currentRate = Number(rate.currentRate.replace(/[^0-9\.]+/g,''));
  rate.rentLink = jQuery(value).find('td:eq(0) a').attr('href');
  ratesSnapshot.push(rate);
});

jQuery('table.avail-table:eq(1)').find('table:has(tr.table-top)').find('tr:gt(1)').each(function(key, value) {
  var rate = {};
  rate.dateVisited = now.toISOString();
  rate.startDate = Date.create(jQuery(value).find('td:eq(0)').text().trim().replace(/^.*?(?=[0-9]+\/[0-9]+\/[0-9]+)/g,'')).toISOString();
  rate.endDate = Date.create(rate.startDate).addDays(5).toISOString();
  rate.previousRate = jQuery(value).find('td:eq(1)').text().trim();
  rate.previousRate = Number(rate.previousRate.replace(/[^0-9\.]+/g,''));
  rate.reserveRate = jQuery(value).find('td:eq(2)').text().trim();
  rate.reserveRate = Number(rate.reserveRate.replace(/[^0-9\.]+/g,''));
  rate.rentLink = jQuery(value).find('td:eq(0) a').attr('href');
  ratesSnapshot.push(rate);
});

result.ratesSnapshot = ratesSnapshot;

//Get the Lat/Lon of the listing.
var latLonRegex = /loc:([+-]?[0-9\.]*)\s([+-]?[0-9\.]*)/g;
var mapsUrl = jQuery('div.map-cont iframe').attr('src');
var mapsUri = URI.parse(mapsUrl);
var query = URI.parseQuery(mapsUri.query);

var latLon = latLonRegex.exec(query.q);

result.coordinates = {};
result.coordinates.latitude = latLon[1];
result.coordinates.longitude = latLon[2];

return result;
";

        //Retrieve an existing rental...
        using (var session = RavenHelpers.GetSession())
        {
          result = session.Query<Listing>().FirstOrDefault(l => l.Url == driver.Url);

          if (result == null)
          {
            //Crawl the page...
            result = driver.ExecuteScriptWithResult<Listing>(crawlScript);
            result.FirstVisited = DateTime.Now;
            result.RealtyAgency = "Carolina Designs";
            result.Url = driver.Url;
            session.Store(result);
          }
          else
          {
            result.RatesSnapshot = new List<RentalRateSnapshot>();
            driver.ExecuteScriptPopulateObject(result, crawlScript);
            result.LastVisited = DateTime.Now;
          }

          result.Address.PopulateFromCoordinates(result.Coordinates);

          //result.UpdateRatesFromSnapshot(s_notifier);

          session.SaveChanges();
        }
      }
      Console.WriteLine("Listing Crawled.");
      return result;
    }
  }
}
