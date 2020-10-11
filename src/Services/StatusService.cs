using System;
using System.Threading.Tasks;

namespace HuisWerkBot.Services
{
    public class StatusService
    {
        private static readonly DateTime _runTime;
        
        static StatusService()
        {
            _runTime = DateTime.Now;
        }

        public static async Task<DateTime> GetRuntime()
            => DateTime.Now.Subtract(TimeSpan.FromTicks(_runTime.Ticks));

        public static async Task<string> GetUpTime(DateTime runtime)
            => $"{runtime.Hour}:{runtime.Minute}:{runtime.Second}";
        
    }
}