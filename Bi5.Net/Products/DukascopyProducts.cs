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
                { "USDJPY", new Product("USDJPY", 3, "FX") },
                { "BRENTCMDUSD", new Product("BRENTCMDUSD", 3, "CFD") },
                { "LIGHTCMDUSD", new Product("LIGHTCMDUSD", 3, "CFD") },
                { "DEUIDXEUR", new Product("DEUIDXEUR", 3, "CFD") },
                { "GBRIDXGBP", new Product("GBRIDXGBP", 3, "CFD") },
                { "USA30IDXUSD", new Product("USA30IDXUSD", 3, "CFD") },
                { "AUSIDXAUD", new Product("AUSIDXAUD", 3, "CFD") },
                { "JPNIDXJPY", new Product("JPNIDXJPY", 3, "CFD") },
                { "EUSIDXEUR", new Product("EUSIDXEUR", 3, "CFD") },
                { "BUNDTREUR", new Product("BUNDTREUR", 3, "CFD") },
                { "UKGILTTRGBP", new Product("UKGILTTRGBP", 3, "CFD") },
                { "USTBONDTRUSD", new Product("USTBONDTRUSD", 3, "CFD") },
                
                
                

            }
        ){}
    }
}