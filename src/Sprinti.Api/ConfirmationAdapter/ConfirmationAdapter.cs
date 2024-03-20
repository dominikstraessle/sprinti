using Sprinti.Domain;
using Newtonsoft.Json;
using System.Text;

namespace Sprinti.Api.Button
{
    public class ConfirmationAdapter : IConfirmationAdapter
    {
        private static readonly HttpClient _client = new HttpClient();
        public ILogger<ConfirmationAdapter>? _logger;

        public void Confirmation(CubeConfig cubeConfig)
        {
            var config = cubeConfig;
            var configDictionary = new SortedDictionary<int, Color>();
            foreach (var entry in config.Config)
            {
                configDictionary.Add(entry.Key, entry.Value);
            }
            var ConfigModel = new CubeConfig
            {
                Time = DateTime.Now,
                Config = configDictionary
            };

            string jsonString = JsonConvert.SerializeObject(ConfigModel);
            PostConfig(jsonString);

        }

        private async void PostConfig(string jsonString)
        {
            var Url = @"URL/cubes/teamxx/config";
            using var httpClient = new HttpClient();
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync(Url, content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully posted the config.");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(responseBody);
                }
                else
                {
                    _logger.LogError($"Failed to post the config. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"{e.Message}");
            }

        }

        public async Task<string> StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var url = @"http://52.58.217.104:5000/cubes";
                HttpResponseMessage response = await _client.PostAsync(url, null);
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Request was canceled.", e.Message);
                return "Request was canceled.";
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
                return $"Error: {ex.Message}";
            }
        }

    }
}
