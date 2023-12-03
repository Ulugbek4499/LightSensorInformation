using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using SensorEmulator;

public class Program
{
    private const string ServerUrl = "https://localhost:7253/devices/{deviceId}/telemetry";

    public static async Task Main(string[] args)
    {
        string deviceId = "1";

        while (false)
        {
            double illuminance = GenerateSimulatedIlluminance();

            var telemetryData = new List<TelemetryEntry>
            {
                new TelemetryEntry { Illuminance = illuminance, Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
            };

            await SendTelemetryDataAsync(deviceId, telemetryData);

            Thread.Sleep(TimeSpan.FromSeconds(15));
        }

        await GetStatisticsAsync(deviceId);
    }

    private static double GenerateSimulatedIlluminance()
    {
        // Simulated logic to generate illumination values
        // For example, increasing illuminance in the first half of the day and decreasing in the second half
        int currentHour = DateTime.UtcNow.Hour;
        return currentHour < 12 ? 100.0 + currentHour : 200.0 - currentHour;
    }

    private static async Task SendTelemetryDataAsync(string deviceId, List<TelemetryEntry> telemetryData)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                string url = ServerUrl.Replace("{deviceId}", deviceId);

                // Convert telemetry data to JSON
                string jsonTelemetryData =JsonConvert.SerializeObject(telemetryData);

                // Prepare content
                var content = new StringContent(jsonTelemetryData, Encoding.UTF8, "application/json");

                // Send HTTP POST request to the server
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                Console.WriteLine($"Telemetry data sent successfully: {jsonTelemetryData}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending telemetry data: {ex.Message}");
        }
    }

    private static async Task GetStatisticsAsync(string deviceId)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                string url = $"https://localhost:7253/devices/{deviceId}/statistics";

                // Send HTTP GET request to the server
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Parse and display the statistics
                var statistics = JsonConvert.DeserializeObject<List<StatisticsEntry>>(responseContent);

                Console.WriteLine("Statistics:");
                foreach (var entry in statistics)
                {
                    Console.WriteLine($"Date: {entry.Date}, Max Illuminance: {entry.MaxIlluminance}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting statistics: {ex.Message}");
        }
    }
}