using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockWebsites
{
    public static class FileManager
    {
        private static List<string> _websitesToBlock = [];
        private static readonly string _path = SystemPaths.DataFilePath;
        public static int GetListSitesCount => _websitesToBlock.Count / 4;
        private static List<string> WaysToBlock(string site) {
            var list = new List<string>
            {
                // IPv4
                $"0.0.0.0 {site}",
                $"0.0.0.0 www.{site}",
        
                // IPv6
                $":: {site}",
                $":: www.{site}"
            };
            return list;
        }

        /// <summary>
        /// Create block.sites.data file.
        /// </summary>
        public static void CreateDataFile() {
            if (!File.Exists(_path))
            {
                File.Create(_path).Dispose();
            }
        }

        /// <summary>
        /// Read block.sites.data file and if it has info, save it to websitesToBlock without deleting the text.
        /// </summary>
        public static void ReadDataFile(string file) {
            // Reading block.sites.data file
            string[] readDataFile = File.ReadAllLines(file);
            // Adding block.sites.data values to list
            foreach (var line in readDataFile)
            {
                _websitesToBlock.Add(line);
            }
        }
        
        /// <summary>
        /// Display list of blocked sites.
        /// </summary>
        public static void DisplayBlockedSites() {
            if (GetListSitesCount == 0)
            {
                Console.WriteLine("\n--- The block list is empty ---");
                return; 
            }
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║              LIST OF SITES             ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            foreach (var site in _websitesToBlock)
            {
                Console.WriteLine($"- {site}");
            }
        }

        /// <summary>
        /// Check if site has already been blocked - no duplicates.
        /// </summary>
        /// <param name="site">www. site .com</param >
        private static bool CheckIfSiteExistInData(List<string> site) {
            foreach (var item in site)
	        {
               return _websitesToBlock.Contains(item);
	        }
            return false;
        }

        /// <summary>
        /// Add one or many sites at once.
        /// User should write the site name, not all path like 127.0.0.1 www.reddit.com
        /// </summary>
        public static void AddOneOrMultipleSitesToBlock() {
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║                 ADD SITES              ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("Enter the site names you want to block.");
            Console.WriteLine("Note: Do not include 'www.' — just the domain with its extension (e.g., .com, .net, .org).");
            Console.WriteLine("Example: reddit.com facebook.com wikipedia.org");
            Console.Write("Sites: ");

            string nameSite = Console.ReadLine()!;
            if (string.IsNullOrEmpty(nameSite)) return;
            var manySites = nameSite.Split(" ").Where(m => m != "");
            foreach (var site in manySites)
            {
                var _waysToBlock = WaysToBlock(site);

                if (CheckIfSiteExistInData(_waysToBlock))
                {
                    Console.WriteLine($"\n{site} was added before.");
                    continue;
                }

                _websitesToBlock.AddRange(_waysToBlock);
                Console.WriteLine($"\n{site} added!");
                File.WriteAllLines(_path, _websitesToBlock);
            }
        }

        /// <summary>
        /// Delete one or many sites at once.
        /// </summary>
        public static void DeleteSites() {
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║               DELETE SITES             ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("Enter the site names you want to delete.");
            Console.WriteLine("Note: Do not include 'www.' — just the domain with its extension (e.g., .com, .net, .org).");
            Console.WriteLine("Example: reddit.com facebook.com wikipedia.org");
            Console.Write("Sites: ");
            string nameSite = Console.ReadLine()!;
            var manySites = nameSite.Split(" ").Where(m => m != "");
            foreach (var site in manySites)
            {
                var _waysToBlock = WaysToBlock(site);

                if (CheckIfSiteExistInData(_waysToBlock))
                {
                    _websitesToBlock.RemoveAll(line => _waysToBlock.Contains(line));
                    Console.WriteLine($"\nSite {site} has been permanently removed.");
                    File.WriteAllLines(_path, _websitesToBlock);
                    continue;
                }
                    Console.WriteLine($"\n{site} doesnt exist.");
            }
        }

        /// <summary>
        /// Merge the created data file into the hosts file.
        /// </summary>
        private static void MergeToHosts() {
            try
            {
                //text from data file
                string[] mySitesFromData = File.ReadAllLines(SystemPaths.DataFilePath);

                //Get original content from host
                bool ignoreLine = false;
                List<string> StoreCurrentTextFromHost = [];
                foreach (string linea in File.ReadAllLines(SystemPaths.HostPath))
                {
                    if (linea.Contains("# --- BLOCK_SITES_START ---"))
                    {
                        ignoreLine = true;
                        continue;
                    }

                    if (linea.Contains("# --- BLOCK_SITES_END ---"))
                    {
                        ignoreLine = false;
                        continue;
                    }

                    if (ignoreLine == false)
                    {
                        StoreCurrentTextFromHost.Add(linea);
                    }
                }

                // Flag to know where to delete
                StoreCurrentTextFromHost.Add("# --- BLOCK_SITES_START ---");
                StoreCurrentTextFromHost.AddRange(mySitesFromData);
                StoreCurrentTextFromHost.Add("# --- BLOCK_SITES_END ---");

                // Rewrite Host File
                File.WriteAllLines(SystemPaths.HostPath, StoreCurrentTextFromHost);
                Console.WriteLine("Sync successful: Hosts file updated.");

                // To flush dns
                Process.Start(new ProcessStartInfo("ipconfig", "/flushdns")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: Please run the program as Administrator to update Hosts.");
                Console.WriteLine("Sites deleted!");
                _websitesToBlock = [];
            }
        }
        public static void SyncWithHosts() => MergeToHosts();
    }
}
