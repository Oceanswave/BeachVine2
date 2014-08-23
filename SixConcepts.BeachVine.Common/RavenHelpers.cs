namespace SixConcepts.BeachVine
{
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using Raven.Abstractions;
  using Raven.Client;
  using Raven.Client.Document;
  using System;
  using System.Net;
  using System.Text.RegularExpressions;
  using SixConcepts.BeachVine.DomainModel;

  public static class RavenHelpers
  {
    public static string CleanseId(string id)
    {
      var rgx = new Regex("[^a-zA-Z0-9/_]", RegexOptions.Compiled);
      id = rgx.Replace(id, String.Empty);
      return id;
    }

    public static DocumentStore GetDocumentStore()
    {
      var documentStore = new DocumentStore
        {
          Url = "http://sixconcepts-ravendb-01.cloudapp.net",
          //ApiKey = "RavenDBUser/5gKXMQEk4Pd",
          DefaultDatabase = "BeachVine",
          Credentials = new NetworkCredential
            {
              Domain = "SixConcepts",
              UserName = "RavenDBUser",
              Password = "R00tB33r!"
            },
        };

      documentStore.Initialize();
      return documentStore;
    }

    public static IDocumentSession GetSession()
    {
      var documentStore = GetDocumentStore();

      return documentStore.OpenSession(new OpenSessionOptions
      {
        Credentials = new NetworkCredential
        {
          Domain = "SixConcepts",
          UserName = "RavenDBUser",
          Password = "R00tB33r!"
        },
      });
    }

    public static IAsyncDocumentSession GetAsyncSession()
    {
      var documentStore = GetDocumentStore();

      return documentStore.OpenAsyncSession(new OpenSessionOptions
      {
        Credentials = new NetworkCredential
        {
          Domain = "SixConcepts",
          UserName = "RavenDBUser",
          Password = "R00tB33r!"
        },
      });
    }

    public static void RetrieveListingImages(Listing result)
    {
      Console.WriteLine("Retriving images...");

      var webClient = new WebClient();
      var webResources = new List<WebResource>();
      using (var session = GetSession())
      {
        session.Advanced.MaxNumberOfRequestsPerSession = result.Images.Count() + 1;

        foreach (var img in result.Images)
        {
          var webResource = session.Query<WebResource>().FirstOrDefault(l => l.Url == img.OriginalUrl);
          if (webResource == null)
          {
            webResource = new WebResource
              {
                DateAdded = SystemTime.UtcNow,
                Url = img.OriginalUrl
              };

            session.Store(webResource);
            
          }

          if (webResource.DateRetrieved == null)
            webResources.Add(webResource);
        }

        session.SaveChanges();
      }

      foreach (var resource in webResources)
      {
        try
        {
          var sw = Stopwatch.StartNew();
          var url = new UriBuilder("http:" + resource.Url);

          Console.WriteLine("Retrieving image at {0}", url.Uri);
          var data = webClient.DownloadData(url.Uri.ToString());
          using (var ms = new MemoryStream(data))
          {
            var ds = GetDocumentStore();
            try
            {
              ds.DatabaseCommands.PutAttachment(resource.Id, null, ms, null);
            }
            catch (Exception)
            {
              //Do Nothing!!
            }

          }

          using (var session = GetSession())
          {
            var resourceToUpdate = session.Load<WebResource>(resource.Id);
            resourceToUpdate.DateRetrieved = SystemTime.UtcNow;
            session.SaveChanges();
          }

          sw.Stop();
          Console.WriteLine("Retrieved image at {0} in {1}", url.Uri, sw.Elapsed);
        }
        catch (Exception)
        {
          Console.WriteLine("Unable to download {0}", resource.Url);
        }
      }
    }
  }
}
