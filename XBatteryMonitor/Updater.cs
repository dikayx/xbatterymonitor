using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XBatteryMonitor
{
    public class Updater
    {
        private const string GitHubReleasesUrl = "https://api.github.com/repos/dikayx/xbatterymonitor/releases";
        private const string UserAgent = "XBatteryMonitor";

        public async Task<bool> IsUpdateAvailable()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            var response = await client.GetStringAsync(GitHubReleasesUrl);
            var releases = JsonSerializer.Deserialize<List<GithubRelease>>(response);

            if (releases == null || !releases.Any())
                throw new Exception("No releases found.");

            var latestRelease = releases.FirstOrDefault();
            if (latestRelease == null)
                throw new Exception("No valid releases found.");

            var latestVersionString = latestRelease.TagName?.TrimStart('v').Split('+')[0]; // Remove "v" and metadata
            if (string.IsNullOrWhiteSpace(latestVersionString))
                throw new FormatException($"Invalid release tag: {latestRelease.TagName}");

            if (!Version.TryParse(latestVersionString, out var latestVersion))
                throw new FormatException($"Failed to parse version: {latestVersionString}");

            var currentVersionString = Application.ProductVersion.Split('+')[0];
            if (!Version.TryParse(currentVersionString, out var currentVersion))
                throw new FormatException($"Invalid Application.ProductVersion format: {Application.ProductVersion}");

            return latestVersion > currentVersion;
        }

        public async Task DownloadAndInstallUpdate()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            try
            {
                var response = await client.GetStringAsync(GitHubReleasesUrl);
                var releases = JsonSerializer.Deserialize<List<GithubRelease>>(response);

                var latestRelease = releases?.FirstOrDefault();
                if (latestRelease == null) throw new InvalidOperationException("No release information found.");

                var asset = latestRelease.Assets.FirstOrDefault(a => a.Name.EndsWith(".zip"));
                if (asset == null) throw new InvalidOperationException("No valid update file found.");

                var tempPath = Path.Combine(Path.GetTempPath(), "XBatteryMonitorUpdate");
                Directory.CreateDirectory(tempPath);

                var zipFilePath = Path.Combine(tempPath, asset.Name);
                var msiFilePath = Path.Combine(tempPath, "XBatteryMonitorSetup.msi");

                var zipData = await client.GetByteArrayAsync(asset.BrowserDownloadUrl);
                await File.WriteAllBytesAsync(zipFilePath, zipData);

                ZipFile.ExtractToDirectory(zipFilePath, tempPath, overwriteFiles: true);

                var installerProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = "msiexec",
                    Arguments = $"/i \"{msiFilePath}\" /quiet",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                installerProcess?.WaitForExit();

                var dialogResult = NotificationHandler.ShowMessageBox(
                    "The update was installed successfully. The application will now restart to apply the changes.",
                    "Update Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                 );

                if (dialogResult == DialogResult.OK)
                {
                    RestartApplication();
                }
            }
            catch (Exception ex)
            {
                NotificationHandler.ShowMessageBox(
                    "An error occurred during the update process: " + ex.Message,
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                 );
            }
            finally
            {
                // Clean up
                var tempPath = Path.Combine(Path.GetTempPath(), "XBatteryMonitorUpdate");
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
            }
        }

        private static void RestartApplication()
        {
            var executablePath = Application.ExecutablePath;

            Process.Start(new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = true
            });

            Application.Exit();
        }

        // TODO: Move these classes to separate files
        public class GithubRelease
        {
            [JsonPropertyName("tag_name")]
            public string TagName { get; set; }

            [JsonPropertyName("draft")]
            public bool Draft { get; set; }

            [JsonPropertyName("prerelease")]
            public bool PreRelease { get; set; }

            [JsonPropertyName("assets")]
            public List<GithubAsset> Assets { get; set; }
        }

        public class GithubAsset
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("browser_download_url")]
            public string BrowserDownloadUrl { get; set; }
        }
    }
}
