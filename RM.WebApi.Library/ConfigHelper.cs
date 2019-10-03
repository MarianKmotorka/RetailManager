using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.WebApi.Library
{
    public class ConfigHelper 
    {
        /// <summary>
        /// Use like this: Price * TaxRate
        /// </summary>
        public static decimal TaxRate
        {
            get
            {
                var taxRateString = ConfigurationManager.AppSettings["taxRate"];

                if (decimal.TryParse(taxRateString, out decimal taxRate))
                    return taxRate;

                throw new ConfigurationErrorsException("TaxRate configuration is not set up properly");
            }
        }
    }
}
