using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace DefaultNamespace
{
    public class ApiClient
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string tenantId;
        private readonly string campusApiUrl;
        private string? token;
        private static readonly HttpClient httpClient = new HttpClient();

        public ApiClient(string clientId, string clientSecret, string tenantId, string campusApiUrl)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.tenantId = tenantId;
            this.campusApiUrl = campusApiUrl;
            this.token = null;
            Console.WriteLine("campusApiUrl: " + campusApiUrl);
        }

        public async Task AuthenticateAsync()
        {
            var url = $"https://login.microsoftonline.com/{this.tenantId}/oauth2/v2.0/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", this.clientId),
                new KeyValuePair<string, string>("client_secret", this.clientSecret),
                new KeyValuePair<string, string>("scope", "api://41425158-23d9-456c-a471-51e7669411fa/.default")
            });

            try
            {
                var response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseData);
                this.token = jsonDocument.RootElement.GetProperty("access_token").GetString();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Authentication failed: " + ex.Message);
                throw new Exception("Authentication failed", ex);
            }
        }

        public async Task<dynamic> GetObservationsAsync(string sensorId, string startTime, string endTime, string sensorType)
        {
            if (this.token == null)
            {
                await AuthenticateAsync();
            }

            var url = $"{this.campusApiUrl}/observations/sensors?";
            if (startTime != null)
            {
                startTime = HttpUtility.UrlEncode(startTime);
                url += $"&start_time={startTime}";
            }
            if (endTime != null)
            {
                endTime = HttpUtility.UrlEncode(endTime);
                url += $"&end_time={endTime}";
            }
            if (sensorType != null)
            {
                url += $"&sensor_type={sensorType}";
            }
            if (sensorId != null)
            {
                url += $"&id={sensorId}";
            }
            url = url.Replace("?&", "?");
            Console.WriteLine("Fetching observations from: " + url);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonDocument.Parse(responseData).RootElement;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to fetch observations: " + ex.Message);
                throw new Exception("Failed to fetch observations", ex);
            }
        }
    }
}