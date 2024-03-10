using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DymoTest
{
    public class Dymo
    {
        private readonly string _hostname;
        private readonly int _port;
        private readonly HttpClient _httpClient;

        public Dymo(string hostname = "127.0.0.1", int port = 41951)
        {
            _hostname = hostname;
            _port = port;
            _httpClient = new HttpClient();
        }

        private string ApiUrl => $"https://{_hostname}:{_port}/DYMO/DLS/Printing";

        public async Task<string> Print(string printerName, string labelXml, string labelSetXml = "")
        {
            var label = $"printerName={Uri.EscapeDataString(printerName)}&printParamsXml=&labelXml={Uri.EscapeDataString(labelXml)}&labelSetXml={Uri.EscapeDataString(labelSetXml)}";

            try
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NODE_TLS_REJECT_UNAUTHORIZED")))
                    Environment.SetEnvironmentVariable("NODE_TLS_REJECT_UNAUTHORIZED", "0");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to set environment variable: {ex.Message}");
            }

            return await SendRequest("PrintLabel", HttpMethod.Post, label);
        }

        public async Task<string> GetStatus()
        {
            return await SendRequest("StatusConnected", HttpMethod.Get);
        }

        public async Task<string> GetPrinters()
        {
            return await SendRequest("GetPrinters", HttpMethod.Get);
        }

        private async Task<string> SendRequest(string endpoint, HttpMethod method, string content = "")
        {
            using (var request = new HttpRequestMessage(method, $"{ApiUrl}/{endpoint}"))
            {
                if (!string.IsNullOrEmpty(content))
                    request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

                using (var response = await _httpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Error message: {errorMessage}");
                    }

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
