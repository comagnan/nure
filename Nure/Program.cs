using System;
using System.IO;
using System.Text.Json;
using Nure.Configuration;

namespace Nure
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonString = File.ReadAllText("nure-config.json");
            NureOptions options = JsonSerializer.Deserialize<NureOptions>(jsonString);
            Console.WriteLine(options.ToString());
        }
    }
}
