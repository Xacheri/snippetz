using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace snippetz
{
    [Serializable]
    internal class ActionContainer
    {
        [JsonPropertyName("command")]
        public string command { get; set; }
        public ActionContainer() { }
        public ActionContainer(string cmd) {
            command = cmd;
        }
        public void Execute()
        {
            string c = command;
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + c)
            {
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            Process process = Process.Start(psi);
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            Console.WriteLine(output);

            if (error.Length > 0)
            {
                Console.WriteLine("Snippet Error:");
                Console.WriteLine(error);
            }
        }
    }
}
