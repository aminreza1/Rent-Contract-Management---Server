using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Infrastructure.ExtensionMethods
{
    public static class ConvertManager
    {
        public static int SafeToInt(this string inputString, int defaultValue)
        {
            try
            {
                return int.Parse(inputString);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
