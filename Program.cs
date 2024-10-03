using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DefaultNamespace
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Bind configuration to AzureSettings
            var azureSettings = new AzureSettings();
            configuration.GetSection("Azure").Bind(azureSettings);
            var observationSettings = new ObservationSettings();
            configuration.GetSection("Observation").Bind(observationSettings);

            var apiClient = new ApiClient(
                azureSettings.ClientId,
                azureSettings.ClientSecret,
                azureSettings.TenantId,
                azureSettings.CampusApiUrl
            );

            try
            {
                var observations = await apiClient.GetObservationsAsync(
                    "Sensor-12345678-abcd-1234-cdef-123456789abc",
                    "2023-01-01T00:00:00Z",
                    "2023-01-02T00:00:00Z",
                    observationSettings.SensorType
                );
                Console.WriteLine("Observations: " + observations);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}