using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service
{
    public class FileLogger : ILogger
    {
        private readonly string _mainPath;

        public FileLogger(string path)
        {
            _mainPath = path;
        }

        /// <summary>
        /// writes an log message to .txt file
        /// </summary>
        /// <param name="message">log message</param>
        /// <returns></returns>
        public async Task WriteAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new NullReferenceException();

            CreateFolder(_mainPath);

            var filePath = _mainPath + "\\Log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            if (!File.Exists(filePath))
            {
                using var streamWriter = File.CreateText(filePath);
                await streamWriter.WriteLineAsync(message);
            }
            else
            {
                using var streamWriter = File.AppendText(filePath);
                await streamWriter.WriteLineAsync(message);
            }
        }

        /// <summary>
        /// Creates a folder
        /// </summary>
        /// <param name="path">path of folder</param>
        private static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
