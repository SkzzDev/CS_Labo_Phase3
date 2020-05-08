using Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core
{
    public static class Logs
    {

        public static void Write(string textToWrite)
        {
            string logsPath = Functions.GetLogsPath();
            DateTime now = DateTime.Now;
            string currentTime = "[" + now.Day + "/" + now.Month + "/" + now.Year + " @ " + now.Hour + ":" + now.Minute + ":" + now.Second + "]";
            try {
                if (!File.Exists(logsPath))
                    File.Create(logsPath);
                textToWrite = currentTime + " " + textToWrite;
                using (StreamWriter file = new StreamWriter(logsPath, true)) {
                    file.WriteLine(textToWrite);
                    file.Close();
                }
                Console.WriteLine(textToWrite);
            } catch (Exception e) {
                Console.WriteLine(currentTime + " " + e.Message);
            }
        }

    }
}
