using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.ReportManager.Services
{
    internal class InputService
    {
        private readonly ILogger<InputService> _logger;
        public InputService(ILogger<InputService> logger)
        {
            _logger = logger;
        }

        public string GetStr(string message)
        {
            _logger.LogInformation(message);
            return Console.ReadLine();
        }

        public T GetEnum<T>(string message) where T : Enum
        {
            while (true)
            {
                _logger.LogInformation(message);
                var str = Console.ReadLine();
                if (Enum.TryParse(typeof(T), str, true, out var res))
                {
                    return (T)res;
                }
            }
        }

        public T GetEnum<T>() where T : struct, Enum
        {
            var lst = Enum.GetValues<T>();
            string pick_option = "\n";
            foreach (var val in lst)
            {
                pick_option += val.ToString("D") + ") " + val + "\n";
            }
            return GetEnum<T>(pick_option);
        }

        public int GetInt(string message)
        {
            while (true)
            {
                _logger.LogInformation(message);
                var str = Console.ReadLine();
                if (int.TryParse(str, out var res))
                {
                    return res;
                }
            }
        }

        public void Log(string message)
        {
            _logger.LogInformation(message);
        }
        public void LogError(string message)
        {
            _logger.LogError(message);
        }
    }
}
