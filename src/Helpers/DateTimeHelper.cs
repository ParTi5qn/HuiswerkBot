using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace HuiswerkBot.Helpers
{
    internal class DateTimeHelper
    {
        public static DateTime FormatDateTime(string input, string format = null)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("C:\\mylog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            if (format != null && DateTime.TryParse(format, out DateTime date)) return date;
            Log.Information("Input: {0}", input);
            date = DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Log.Information("DateTime: {0}", date);
            return date;
        }

        public static DateTime FormatDateTime(DateTime input, string format = null)
        {
            if (format != null && DateTime.TryParse(format, out DateTime date)) return date;
            return DateTime.ParseExact(input.ToString(CultureInfo.InvariantCulture), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}