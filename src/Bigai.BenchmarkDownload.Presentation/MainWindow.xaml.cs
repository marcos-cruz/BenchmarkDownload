using Bigai.BenchmarkDownload.Presentation.Helpers;
using Bigai.BenchmarkDownload.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Bigai.BenchmarkDownload.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stopwatch _watch;
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events

        private void NormalExecution_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearWindow();

                DisableButtons();

                InitMeasureTime();

                List<WebsiteDataModel> results = DownloadHelper.RunDownloadSync();

                FinalizeProcess(results);

            }
            catch (Exception ex)
            {
                TextResult.Text += $"{ Environment.NewLine }Make sure you have an Internet connection. {ex.Message} { Environment.NewLine }";
            }

            EnableButtons();
        }

        private void ParallelSyncExecution_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearWindow();

                DisableButtons();

                InitMeasureTime();

                List<WebsiteDataModel> results = DownloadHelper.RunDownloadParallelSync();

                FinalizeProcess(results);

            }
            catch (Exception ex)
            {
                TextResult.Text += $"{ Environment.NewLine }Make sure you have an Internet connection. {ex.Message} { Environment.NewLine }";
            }

            EnableButtons();
        }

        private async void AsyncExecution_Click(object sender, RoutedEventArgs e)
        {
            List<WebsiteDataModel> results = null;
            Progress<ProgressReport> progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ReportProgress;
            string messageError = "";

            try
            {
                ClearWindow();

                DisableButtons();

                CancelExecution.IsEnabled = true;

                InitMeasureTime();

                results = await DownloadHelper.RunDownloadAsync(progress, _cancellationToken.Token);

                FinalizeProcess(results);
            }
            catch (OperationCanceledException)
            {
                messageError = $"{ Environment.NewLine }The operation was canceled { Environment.NewLine }";
            }
            catch (Exception ex)
            {
                messageError = $"{ Environment.NewLine }Make sure you have an Internet connection. {ex.Message} { Environment.NewLine }";
            }

            TextResult.Text += messageError;

            EnableButtons();

            CancelExecution.IsEnabled = false;

            ResetCancellationToken();
        }

        private async void ParalelExecutionAsync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearWindow();

                DisableButtons();

                InitMeasureTime();

                var results = await DownloadHelper.RunDownloadParallelAsync();

                FinalizeProcess(results);
            }
            catch (Exception ex)
            {
                TextResult.Text += $"{ Environment.NewLine }Make sure you have an Internet connection. {ex.Message} { Environment.NewLine }";
            }

            EnableButtons();
        }

        private async void ParalelExecutionAsyncProgessBar_Click(object sender, RoutedEventArgs e)
        {
            Progress<ProgressReport> progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ReportProgress;

            try
            {
                ClearWindow();

                DisableButtons();

                InitMeasureTime();

                var results = await DownloadHelper.RunDownloadParallelAsyncV2(progress);

                FinalizeProcess(results);
            }
            catch (Exception ex)
            {
                TextResult.Text += $"{ Environment.NewLine }Make sure you have an Internet connection. {ex.Message} { Environment.NewLine }";
            }

            EnableButtons();
        }

        private void CancelExecution_Click(object sender, RoutedEventArgs e)
        {
            _cancellationToken.Cancel();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Clear messages on the screen.
        /// </summary>
        private void ClearWindow()
        {
            TextResult.Text = "";
            DashboardProgess.Value = 0;
        }

        /// <summary>
        /// Disable buttons.
        /// </summary>
        private void DisableButtons()
        {
            EnableDisableButtons(false);
        }

        /// <summary>
        /// Enable buttons.
        /// </summary>
        private void EnableButtons()
        {
            EnableDisableButtons(true);
        }

        /// <summary>
        /// Determines whether buttons are enabled.
        /// </summary>
        /// <param name="status">flag indicating the status.</param>
        private void EnableDisableButtons(bool status)
        {
            NormalExecution.IsEnabled = status;
            ParallelSyncExecution.IsEnabled = status;
            AsyncExecution.IsEnabled = status;
            ParalelExecutionAsync.IsEnabled = status;
            ParalelExecutionAsyncProgessBar.IsEnabled = status;
        }

        /// <summary>
        /// Start the processing time counter.
        /// </summary>
        private void InitMeasureTime()
        {
            _watch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Start the processing time counter.
        /// </summary>
        /// <returns>Total number of milliseconds measured by the current instance.</returns>
        private long StopMeasureTime()
        {
            _watch.Stop();

            return _watch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Shows the result of the processing and ends the time counter.
        /// </summary>
        /// <param name="results">List with information of downloaded sites.</param>
        private void FinalizeProcess(List<WebsiteDataModel> results)
        {
            TextResult.Text = "";

            PrintResults(results);

            long elapsedMilliseconds = StopMeasureTime();

            TextResult.Text += $"{ Environment.NewLine }Tempo total de execução: { elapsedMilliseconds }";
        }

        /// <summary>
        /// Displays the list with the information of the downloaded sites.
        /// </summary>
        /// <param name="results">List of web sites downloaded.</param>
        private void PrintResults(List<WebsiteDataModel> results)
        {
            int bytesDownloaded = 0;

            if (results != null)
            {
                foreach (var result in results)
                {
                    TextResult.Text += $"{ result.WebsiteUrl } downloaded: { result.WebsiteData.Length } characters long.{ Environment.NewLine }";
                    bytesDownloaded += result.WebsiteData.Length;
                }
            }

            TextResult.Text += $"{ Environment.NewLine }Total of bytes downloaded: { bytesDownloaded } characters long.";
        }

        /// <summary>
        /// Graphically shows how the download is happening.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportProgress(object sender, ProgressReport e)
        {
            TextResult.Text = "";
            DashboardProgess.Value = e.PercentageCompleted;
            PrintResults(e.SitesDownloaded);
        }

        /// <summary>
        /// Initializes a new instance of the System.Threading.CancellationTokenSource class.
        /// </summary>
        private void ResetCancellationToken()
        {
            _cancellationToken.Dispose();
            _cancellationToken = new CancellationTokenSource();
        }

        #endregion
    }
}
