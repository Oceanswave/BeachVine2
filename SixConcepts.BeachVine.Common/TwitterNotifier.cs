namespace SixConcepts.BeachVine
{
  using OpenQA.Selenium.PhantomJS;
  using SixConcepts.BeachVine.Extensions;
  using System;
  using System.Threading;
  using TweetSharp;

  public class TwitterNotifier : IRateChangeNotifier
  {
    private TwitterService m_service;

    public void Initialize(PhantomJSDriverService service, PhantomJSOptions options)
    {
      Console.WriteLine("Initializing Twitter Notifier...");

      // Pass your credentials to the service
      m_service = new TwitterService("5b52AIam0JKjE7l5GKXtvA", "SRsJ30RhvN3VmCuqVRFdM9RPe2fS8b50glHfjaHnXQ");

      // Step 1 - Retrieve an OAuth Request Token
      var requestToken = m_service.GetRequestToken();

      // Step 2 - Redirect to the OAuth Authorization URL
      var uri = m_service.GetAuthorizationUri(requestToken);

      string verificationCode;
      using (var driver = new PhantomJSDriver(service, options))
      {
        driver.Navigate().GoToUrl(uri);
        driver.JQuerify();
        driver.ExecuteScriptWithResult(@"
jQuery('#username_or_email').val('Beachvine');
jQuery('#password').val('ode2bach');
jQuery('input[value^=Authorize]').click();
");
        Thread.Sleep(2000);

        // Step 3 - Exchange the Request Token for an Access Token

        driver.JQuerify();
        verificationCode = driver.ExecuteScriptWithResult<string>(@"
return jQuery('code').text();
");
      }

      var access = m_service.GetAccessToken(requestToken, verificationCode);

      if (access.UserId == 0)
        throw new InvalidOperationException("Unable to authenticate with twitter...");

      // Step 4 - User authenticates using the Access Token
      m_service.AuthenticateWith(access.Token, access.TokenSecret);
    }

    public void Test()
    {
      m_service.SendTweet(new SendTweetOptions
        {
          Status = "test 1234"
        }, (s, r) => Console.WriteLine(r.StatusDescription));
    }

    public void NotifyOfNewRate(DomainModel.Listing listing, DateTime startDate, DateTime endDate, decimal? newRate)
    {
      m_service.SendTweet(new SendTweetOptions
      {
        Status = String.Format("{0} has new availability for the week of {1:d} for {2:c}", listing.PropertyName, startDate, newRate)
      }, (s, r) => Console.WriteLine("Posted new availibility to twitter."));
    }

    public void NotifyOfNewHoldRate(DomainModel.Listing listing, DateTime startDate, DateTime endDate, decimal? holdRate)
    {
      //Do nothing!! Spam.
      //m_service.SendTweet(new SendTweetOptions
      //{
      //  Status = String.Format("{0} is now available to hold for the week of {1:d} for {2:c}", listing.PropertyName, startDate, holdRate)
      //}, (s, r) => Console.WriteLine("Posted new hold to twitter."));
    }

    public void NotifyOfRateChange(DomainModel.Listing listing, DateTime startDate, DateTime endDate, decimal? oldRate, decimal? newRate)
    {
      m_service.SendTweet(new SendTweetOptions
      {
        Status = String.Format("{0} - for the week of {1:d} the rate has changed from {2:c} to {3:c}", listing.PropertyName, startDate, oldRate, newRate)
      }, (s, r) => Console.WriteLine("Posted rate change to twitter."));
    }

    public void NotifyOfAvailablityChange(DomainModel.Listing listing, DateTime startDate, DateTime endDate, DomainModel.RentalAvailability oldAvailability, DomainModel.RentalAvailability newAvailability)
    {
      m_service.SendTweet(new SendTweetOptions
      {
        Status = String.Format("{0} - for the week of {1:d} the listing has changed from {2} to {3}", listing.PropertyName, startDate, oldAvailability, newAvailability)
      }, (s, r) => Console.WriteLine("Posted availibility change to twitter."));
    }
  }
}
