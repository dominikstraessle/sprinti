using Sprinti.Domain;
using Newtonsoft.Json;
using System.Text;
using Sprinti.Api.ConfirmationAdapter;

namespace Sprinti.Api.Button
{
    public class ConfirmationAdapter : IConfirmationAdapter
    {
        private static readonly HttpClient _client = new HttpClient();
        public ILogger<ConfirmationAdapter>? _logger;
        private ConnectionsOption _connectionOptions;

        public void Confirmation(CubeConfig cubeConfig)
        {
            string jsonString = JsonConvert.SerializeObject(cubeConfig);
            PostConfig(jsonString);

        }

        private async void PostConfig(string jsonString)
        {
            var Url = _connectionOptions + "/config";
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

        public async void StartAsync(CancellationToken cancellationToken)
        {
            if (testConnection == true)
            {
                
                try
                {
                    var url = @"http://52.58.217.104:5000/cubes";
                    HttpResponseMessage response = await _client.PostAsync(url, null);
                    var responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine("Request was canceled.", e.Message);
                    _logger.LogError($"{e.Message}");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Message :{0} ", ex.Message);
                    _logger.LogError($"Error: {ex.Message}");
                }
            }
        }


        public void EndAysnc(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TestConnection(HttpClient httpClient)
        {
            var url = _connectionOptions.TestConnectionString;
            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }catch(HttpRequestException ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
