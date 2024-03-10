using DymoTest;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var printer = new Printer("DYMO LabelWriter 450 DUO Label");
            var status = await printer.GetStatus();
            Console.WriteLine("Status drukarki Dymo: " + status);
            var printers = await printer.GetPrinters();
            Console.WriteLine("Dostępne drukarki Dymo: " + printers);

            string labelText = "TEXT";
            var label = new Label(labelText);
            var printResult = await printer.Print(label);
            Console.WriteLine("Wynik drukowania: " + printResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }
}
