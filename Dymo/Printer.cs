using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DymoTest
{
    public class Printer
    {
        private readonly string _name;
        private readonly Dymo _dymo;

        public Printer(string name)
        {
            _name = name;
            _dymo = new Dymo();
        }

        public async Task<string> Print(Label label)
        {
            try
            {
                return await _dymo.Print(_name, label.Xml);
            }
            catch (HttpRequestException ex)
            {
                return $"Error occurred: {ex.Message}";
            }
        }

        public async Task<string> GetStatus()
        {
            try
            {
                return await _dymo.GetStatus();
            }
            catch (HttpRequestException ex)
            {
                return $"Error occurred: {ex.Message}";
            }
        }

        public async Task<string> GetPrinters()
        {
            try
            {
                return await _dymo.GetPrinters();
            }
            catch (HttpRequestException ex)
            {
                return $"Error occurred: {ex.Message}";
            }
        }
    }
}
