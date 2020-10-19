using System;
using System.Threading.Tasks;

namespace HuiswerkBot.Services
{
    public class Status
    {
        private static readonly DateTime _runTime;
        
        static Status()
        {
            _runTime = DateTime.Now;
        }

        public static async Task<DateTime> GetRuntime()
            => await Task.Run( () => DateTime.Now.Subtract(TimeSpan.FromTicks(_runTime.Ticks)));

        public static async Task<string> GetUpTime(DateTime runtime)
            => await Task.Run(() => $"{runtime.Hour}:{runtime.Minute}:{runtime.Second}");
        
    }
}