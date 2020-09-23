using Bigai.BenchmarkDownload.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Bigai.BenchmarkDownload.Presentation.Helpers
{
    /// <summary>
    /// <see cref="DownloadHelper"/> contains methods to support download contents.
    /// </summary>
    public static class DownloadHelper
    {
        /// <summary>
        /// This method return a list of websites.
        /// </summary>
        /// <returns>List of websites.</returns>
        public static List<string> GetWebsites()
        {
            return new List<string>
            {
                "https://www.yahoo.com",
                "https://www.google.com",
                "https://www.microsoft.com",
                "https://www.cnn.com",
                "https://www.amazon.com",
                "https://www.facebook.com",
                "https://www.twitter.com",
                "https://www.codeproject.com",
                "https://www.stackoverflow.com"
            };
        }

        /// <summary>
        /// Download a list of sites synchronously.
        /// </summary>
        /// <returns>List with information of downloaded sites.</returns>
        public static List<WebsiteDataModel> RunDownloadSync()
        {
            List<string> websites = GetWebsites();
            List<WebsiteDataModel> output = new List<WebsiteDataModel>();

            try
            {
                foreach (string site in websites)
                {
                    WebsiteDataModel results = DownloadWebSite(site);
                    output.Add(results);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return output;
        }

        /// <summary>
        /// Download a list of sites synchronously and in parallel.
        /// </summary>
        /// <returns>List with information of downloaded sites.</returns>
        public static List<WebsiteDataModel> RunDownloadParallelSync()
        {
            List<string> websites = GetWebsites();
            List<WebsiteDataModel> output = new List<WebsiteDataModel>();

            try
            {
                Parallel.ForEach<string>(websites, (site) =>
                {
                    WebsiteDataModel results = DownloadWebSite(site);
                    output.Add(results);
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return output;
        }

        public static async Task<List<WebsiteDataModel>> RunDownloadAsync(IProgress<ProgressReport> progress, CancellationToken cancellationToken)
        {
            List<string> websites = GetWebsites();
            List<WebsiteDataModel> output = new List<WebsiteDataModel>();
            ProgressReport report = new ProgressReport();

            foreach (string site in websites)
            {
                WebsiteDataModel results = await DownloadWebSiteAsync(site);
                output.Add(results);

                cancellationToken.ThrowIfCancellationRequested();

                report.SitesDownloaded = output;
                report.PercentageCompleted = (output.Count * 100) / websites.Count;
                progress.Report(report);
            }

            return output;
        }

        /// <summary>
        /// Downloads the requested resource as a System.String. The resource to download is specified as a System.String 
        /// containing the URI.
        /// </summary>
        /// <param name="address">A System.String containing the URI to download.</param>
        /// <returns>A instance of <see cref="WebsiteDataModel"/>.</returns>
        private static WebsiteDataModel DownloadWebSite(string address)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            try
            {
                output.WebsiteUrl = address;
                output.WebsiteData = client.DownloadString(address);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return output;
        }

        /// <summary>
        /// Downloads the requested resource as a System.String. The resource to download is specified as a System.String 
        /// containing the URI.
        /// </summary>
        /// <param name="address">A System.String containing the URI to download.</param>
        /// <returns>A Task of <see cref="WebsiteDataModel"/>.</returns>
        private static async Task<WebsiteDataModel> DownloadWebSiteAsync(string address)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            try
            {
                output.WebsiteUrl = address;
                output.WebsiteData = await client.DownloadStringTaskAsync(address);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return output;
        }

        /// <summary>
        /// Download a list of sites asynchronously and in parallel.
        /// </summary>
        /// <returns>List with information of downloaded sites.</returns>
        public static async Task<List<WebsiteDataModel>> RunDownloadParallelAsync()
        {
            List<string> websites = GetWebsites();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();
            WebsiteDataModel[] results;
            try
            {
                foreach (string site in websites)
                {
                    tasks.Add(DownloadWebSiteAsync(site));
                }

                results = await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new List<WebsiteDataModel>(results);
        }

        /// <summary>
        /// Download a list of sites asynchronously and in parallel, and update results in progress bar.
        /// </summary>
        /// <returns>List with information of downloaded sites.</returns>
        public static async Task<List<WebsiteDataModel>> RunDownloadParallelAsyncV2(IProgress<ProgressReport> progress)
        {
            List<string> websites = GetWebsites();
            List<WebsiteDataModel> output = new List<WebsiteDataModel>();
            ProgressReport report = new ProgressReport();

            try
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach<string>(websites, (site) =>
                    {
                        WebsiteDataModel results = DownloadWebSite(site);
                        output.Add(results);

                        report.SitesDownloaded = output;
                        report.PercentageCompleted = (output.Count * 100) / websites.Count;
                        progress.Report(report);

                    });
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return output;
        }
    }
}
