using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoExtractor
{
    public class RHandler
    {

        public string RunRScript(string gprFilePath, string jsonOutputPath)
        {

            string baseDirectory = AppContext.BaseDirectory;

            // Navigate to the solution folder (assuming the executable is in a subfolder like bin/Debug/net6.0)
            string solutionPath = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\.."));

            // Path to Rscript-Portable.exe
            string rExecutablePath = Path.Join(solutionPath, "R-Portable\\App\\R-Portable\\bin\\Rscript.exe");
            // Path to r script
            string rScriptPath = Path.Join(solutionPath, @"GeoExtractor\program.R");


            // Create a process to run the R script
            Process process = new Process();
            process.StartInfo.FileName = rExecutablePath;
            process.StartInfo.Arguments = $"\"{rScriptPath}\" \"{gprFilePath}\" \"{jsonOutputPath}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            // Start the process
            process.Start();

            // Read the output (optional)
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();

            return output;
        }

    }
}
