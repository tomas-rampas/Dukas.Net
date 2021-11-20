using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bi5.Net.Models;
using static Bi5.Net.Products.Names;

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
                { AUDNZD, new Product(AUDNZD, 5, "FX") },
                { AUDUSD, new Product(AUDUSD, 5, "FX") },
                { AUDJPY, new Product(AUDJPY, 3, "FX") },
                { GBPAUD, new Product(GBPAUD, 5, "FX") },
                { GBPUSD, new Product(GBPUSD, 5, "FX") },
                { GBPJPY, new Product(GBPJPY, 3, "FX") },
                { EURGBP, new Product(EURGBP, 5, "FX") },
                { EURAUD, new Product(EURAUD, 5, "FX") },
                { EURCAD, new Product(EURCAD, 5, "FX") },
                { EURCHF, new Product(EURCHF, 5, "FX") },
                { EURJPY, new Product(EURJPY, 3, "FX") },
                { EURUSD, new Product(EURUSD, 5, "FX") },
                { NZDJPY, new Product(NZDJPY, 3, "FX") },
                { NZDUSD, new Product(NZDUSD, 5, "FX") },
                { USDJPY, new Product(USDJPY, 3, "FX") },
                { USDCAD, new Product(USDCAD, 5, "FX") },
                { USDCHF, new Product(USDCHF, 5, "FX") },

                { BRENTCMDUSD, new Product(BRENTCMDUSD, 3, "CFD") },
                { LIGHTCMDUSD, new Product(LIGHTCMDUSD, 3, "CFD") },

                { XAGUSD, new Product(XAGUSD, 3, "CFD") },
                { XAUUSD, new Product(XAUUSD, 3, "CFD") },

                { DEUIDXEUR, new Product(DEUIDXEUR, 3, "CFD") },
                { GBRIDXGBP, new Product(GBRIDXGBP, 3, "CFD") },
                { USA30IDXUSD, new Product(USA30IDXUSD, 3, "CFD") },
                { AUSIDXAUD, new Product(AUSIDXAUD, 3, "CFD") },
                { JPNIDXJPY, new Product(JPNIDXJPY, 3, "CFD") },
                { EUSIDXEUR, new Product(EUSIDXEUR, 3, "CFD") },

                { BUNDTREUR, new Product(BUNDTREUR, 3, "CFD") },
                { UKGILTTRGBP, new Product(UKGILTTRGBP, 3, "CFD") },
                { USTBONDTRUSD, new Product(USTBONDTRUSD, 3, "CFD") },
            }
        )
        {
        }
    }
}