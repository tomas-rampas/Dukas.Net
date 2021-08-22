using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Bi5.Net.Models;
using Bi5.Net.Net;

namespace Bi5.Net
{
    public class Loader
    {
        private string _dataUrl = "https://datafeed.dukascopy.com/datafeed/{0}/{1:0000}/{2:00}/{3:00}/{4:00}h_ticks.bi5";

        private readonly LoaderConfig _cfg;
        
        protected Loader() {}

        public Loader(LoaderConfig cfg)
        {
            _cfg = cfg;
        }

        public async Task<bool> Get()
        {
            var webFactory = new WebFactory();
            
            //TODO optimize this, it have to run in parallel  
            foreach (var product in _cfg.Products)
            {
                Console.WriteLine($"Loading {product} from {_cfg.StartDate:yyyy-MM-dd HH:mm:ss} to {_cfg.EndDate:yyyy-MM-dd HH:mm:ss}");

                var totalHours = (_cfg.EndDate - _cfg.StartDate).TotalHours;
                var totalHoursAligned = (int) totalHours + Convert.ToInt32(_cfg.EndDate.Ticks % _cfg.StartDate.Ticks > 0);
                Debug.WriteLine($"{totalHours}, {totalHoursAligned}");
                
                for (int i = 0; i <= totalHoursAligned; i++)
                {
                    var date = _cfg.StartDate.AddHours(i);
                    var bi5DataUrl = string.Format(_dataUrl, product, date.Year, date.Month - 1, date.Day, date.Hour);
                    Console.Write(bi5DataUrl);
                    byte[] bi5Data = await webFactory.DownloadFile(bi5DataUrl);
                    Console.WriteLine($"  - Got {bi5Data.Length} bytes... Done");
                }
            }

            return true;
        }
    }
}