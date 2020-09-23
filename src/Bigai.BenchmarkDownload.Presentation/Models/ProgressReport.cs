using System.Collections.Generic;

namespace Bigai.BenchmarkDownload.Presentation.Models
{
    /// <summary>
    /// <see cref="ProgressReport"/> contains information about download process.
    /// </summary>
    public class ProgressReport
    {
        public List<WebsiteDataModel> SitesDownloaded { get; set; }
        public int PercentageCompleted { get; set; }

        public ProgressReport()
        {
            SitesDownloaded = new List<WebsiteDataModel>();
            PercentageCompleted = 0;
        }
    }
}
