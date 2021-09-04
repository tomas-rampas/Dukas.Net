using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bi5.Net.Models;

namespace Bi5.Net.Products
{
    /// <summary>
    /// Dukascopy product catalog
    /// </summary>
    public class DukascopyProducts : ReadOnlyDictionary<string, Product>
    {
        public static DukascopyProducts Catalogue => new DukascopyProducts();

        private DukascopyProducts() : base(
            new Dictionary<string, Product>
            {
                { "EURUSD", new Product("EURUSD", 5, "FX") },
                { "GBPUSD", new Product("EURUSD", 5, "FX") },
                { "AUDUSD", new Product("EURUSD", 5, "FX") },
                { "EURGBP", new Product("EURUSD", 5, "FX") },
                { "EURAUD", new Product("EURUSD", 5, "FX") },
                { "BRENTCMDUSD", new Product("BRENTCMDUSD", 3, "CFD") },
                { "LIGHTCMDUSD", new Product("LIGHTCMDUSD", 3, "CFD") },
            }
        )
        {
        }
    }
}