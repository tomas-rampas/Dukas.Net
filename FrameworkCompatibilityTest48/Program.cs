using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bi5.Net;
using Bi5.Net.Models;

namespace FrameworkCompatibilityTest48
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Testing Bi5.Net compatibility with .NET Framework 4.8");
            
            try
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "Bi5NetTest48");
                Directory.CreateDirectory(tempDir);
                
                // Create a basic loader configuration
                var config = new LoaderConfig(
                    startDate: new DateTime(2025, 1, 1),
                    endDate: new DateTime(2025, 1, 2),
                    products: new List<string> { "EURUSD" },
                    timeFrameMajorScale: DateTimePart.Minute,
                    timeFrameMinorScale: 60, // 1 hour timeframe
                    outputFolder: tempDir,
                    quoteSide: QuoteSide.Bid,
                    fileScale: FileScale.Hour,
                    writeHeader: true,
                    gzipResult: false
                );
                
                Console.WriteLine($"Initialized LoaderConfig successfully for EURUSD");
                
                
                // Create a loader instance to test if it can be instantiated
                var loader = new Loader(config);
                Console.WriteLine("Successfully created Loader instance");
                await loader.GetAndFlush();
                Console.WriteLine("Compatibility test passed: Bi5.Net can be used with .NET Framework 4.8");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during compatibility test: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
