using System.Text;
using Newtonsoft.Json;
using SensorEmulator;

public class Program
{
    private const string ServerUrl = "https://localhost:7253/devices/{deviceId}/telemetry"; //Here you can change the URL which works for you, you can find that 
                                                                                            // "~\LightSensorInformation\Server\Properties\launchSettings.json"
    public static async Task Main(string[] args)
    {
        string deviceId = "1";              //Here you can change the DeviceId

        while (true)
        {
            double illuminance = GenerateSimulatedIlluminance();

            var telemetryData = new List<TelemetryEntry>
            {
                new TelemetryEntry
                {
                    Illuminance = illuminance,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                }
            };

            await SendTelemetryDataAsync(deviceId, telemetryData);

            Thread.Sleep(TimeSpan.FromMinutes(15));  //Here you can change "FromMinutes" to "FromSeconds" and we can see the results
        }
    }

    private static double GenerateSimulatedIlluminance()
    {
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

                string jsonTelemetryData = JsonConvert.SerializeObject(telemetryData);

                var content = new StringContent(jsonTelemetryData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                response.EnsureSuccessStatusCode();

                Console.WriteLine($"Telemetry data sent successfully: {jsonTelemetryData}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending telemetry data: {ex.Message}");
        }
    }
}