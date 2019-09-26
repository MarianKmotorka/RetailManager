using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.WPF.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        /// <summary>
        /// Use like this: Price * TaxRate
        /// </summary>
        public decimal TaxRate
        {
            get
            {
                var taxRateString = ConfigurationManager.AppSettings["taxRate"];

                if (decimal.TryParse(taxRateString, out decimal taxRate ))
                    return taxRate;

                throw new ConfigurationErrorsException("TaxRate configuration is not set up properly");
            }
        }
    }
}
