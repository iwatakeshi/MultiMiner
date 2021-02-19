using MultiMiner.Engine.Data;
using MultiMiner.Utility.OS;
using System;
using System.IO;
using System.Linq;

namespace MultiMiner.Engine
{
    public static class MinerPath
    {
        public static string GetPathToInstalledMiner(MinerDescriptor miner)
        {
            string executablePath;
            
            switch (OSVersionPlatform.GetConcretePlatform())
            {
                case PlatformID.MacOSX:
                    executablePath = GetPathToMinerOnMacOSX(miner.Name, miner.FileName);
                    break;

                //support Unix - there is no bin folder for the executables like on Mac OS X
                case PlatformID.Unix:
                    //file launching is case-sensitive, lower-case the filename
                    executablePath = GetPathToMinerOnLinux(miner.FileName, miner.FileName.ToLower());
                    break;

                default:
                    executablePath = GetPathToMinerOnWindows(miner.Name, miner.FileName);
                    break;
            }

            return executablePath;
        }

        private static string GetPathToMinerOnMacOSX(string minerName, string minerFileName)
        {
            //try local path first
            string executablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Miners/{0}/bin/{1}", 
                minerName, minerFileName));

            if (!File.Exists(executablePath))
                //try global path (Homebrew)
                executablePath = string.Format(@"/usr/local/bin/{0}", minerFileName);
            return executablePath;
        }

        private static string GetPathToMinerOnLinux(string minerName, string minerFileName)
        {
            //try local path first
            string executablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Miners/{0}/bin/{1}",
                minerName, minerFileName));

            if (!File.Exists(executablePath))
                //try /usr/local/bin
                executablePath = string.Format(@"/usr/local/bin/{0}", minerFileName);

            if (!File.Exists(executablePath))
                //try /usr/bin
                executablePath = string.Format(@"/usr/bin/{0}", minerFileName);
            return executablePath;
        }

        private static string GetPathToMinerOnWindows(string minerName, string minerFileName)
        {
            var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Miners");
            var minerDir = Path.Combine(baseDir, minerName);
            var exe = Path.Combine(minerDir, minerFileName + ".exe");

            // Find the executable
            if (!File.Exists(exe) && Directory.Exists(minerDir)) {
                exe = (from file in Directory.GetFiles(minerDir, "*.exe").ToList()
                       where file.ToLower() == string.Join(minerFileName, "exe").ToLower()
                       select file).First();
            }
            return exe;
        }
    }
}
