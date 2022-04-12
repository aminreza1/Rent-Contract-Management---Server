using System;
using System.Collections.Generic;

namespace MyRentalWebService.Infrastructure.ExtensionMethods
{
    public static class ExceptionManager
    {
        public static string ToMessageResult(this Exception e)
        {
            if (e == null) return "An unspecified error has occurred";

            var result = new List<string>();
            if (!string.IsNullOrWhiteSpace(e.Message)) result.Add(e.Message);
            if (
                e.InnerException != null &&
                !string.IsNullOrWhiteSpace(e.InnerException.Message)
            ) result.Add(e.InnerException.Message);

            if (
                e.InnerException != null &&
                e.InnerException.InnerException != null &&
                !string
                    .IsNullOrWhiteSpace(e.InnerException.InnerException.Message)
            ) result.Add(e.InnerException.InnerException.Message);

            if (result.Count <= 0) return "An unspecified error has occurred";

            return string.Join('_',result);
        }
    }
}
