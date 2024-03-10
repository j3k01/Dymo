using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

    public async Task<string> RenderLabel(string labelXml)
    {
        var label = $"printerName=&renderParamsXml=&labelXml={Uri.EscapeDataString(labelXml)}&labelSetXml=";

        try
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NODE_TLS_REJECT_UNAUTHORIZED")))
                Environment.SetEnvironmentVariable("NODE_TLS_REJECT_UNAUTHORIZED", "0");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to set environment variable: {ex.Message}");
        }

        return await SendRequest("RenderLabel", HttpMethod.Post, label);
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

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var dymo = new Dymo();
            var status = await dymo.GetStatus();
            Console.WriteLine("Status drukarki Dymo: " + status);
            var printers = await dymo.GetPrinters();
            Console.WriteLine("Dostępne drukarki Dymo: " + printers);

            string labelXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<DieCutLabel Version=""8.0"" Units=""twips"">
  <PaperOrientation>Landscape</PaperOrientation>
  <Id>LargeShipping</Id>
  <PaperName>30256 Shipping</PaperName>
  <DrawCommands>
    <RoundRectangle X=""0"" Y=""0"" Width=""1331"" Height=""2715"" Rx=""270"" Ry=""270""/>
  </DrawCommands>
  <ObjectInfo>
    <TextObject>
      <Name>TEXT</Name>
      <ForeColor Alpha=""255"" Red=""0"" Green=""0"" Blue=""0""/>
      <BackColor Alpha=""0"" Red=""255"" Green=""255"" Blue=""255""/>
      <LinkedObjectName></LinkedObjectName>
      <Rotation>Rotation0</Rotation>
      <IsMirrored>False</IsMirrored>
      <IsVariable>False</IsVariable>
      <HorizontalAlignment>Left</HorizontalAlignment>
      <VerticalAlignment>Middle</VerticalAlignment>
      <TextFitMode>AlwaysFit</TextFitMode>
      <UseFullFontHeight>True</UseFullFontHeight>
      <Verticalized>False</Verticalized>
      <StyledText>
        <Element>
          <String></String>
          <Attributes>
            <Font Family=""Helvetica"" Size=""13"" 
            	Bold=""False"" Italic=""False"" Underline=""False"" Strikeout=""False""/>
            <ForeColor Alpha=""255"" Red=""0"" Green=""0"" Blue=""0""/>
          </Attributes>
        </Element>
        <Element>
          <String>SZAFA</String>
          <Attributes>
            <Font Family=""Helvetica"" Size=""13"" 
            	Bold=""False"" Italic=""False"" Underline=""False"" Strikeout=""False""/>
            <ForeColor Alpha=""255"" Red=""0"" Green=""0"" Blue=""0""/>
          </Attributes>
        </Element>
      </StyledText>
    </TextObject>
    <Bounds X=""335.9998"" Y=""57.6001"" Width=""1337.6"" Height=""1192""/>
  </ObjectInfo>
</DieCutLabel>";

            var print = await dymo.Print("DYMO LabelWriter 450 DUO Label", labelXml);
            Console.WriteLine("Wynik drukowania: " + print);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }
}
