using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlockWebsites
{
    public static class SystemPaths {
        /// <summary>
        /// File that stores blocked sites.
        /// </summary>
        private static readonly string _filename = "block.sites.data";

        /// <summary>
        /// System Directory Path (C:\Windows).
        /// </summary>
        private static readonly string _systemRoot = Environment.SystemDirectory;

        /// <summary>
        /// Host Directory Folder Name.
        /// </summary>
        private static readonly string _hostDirectory = Path.GetDirectoryName(HostPath)!;

        /// <summary>
        /// Host Directory Path.
        /// </summary>
        public static string HostPath => Path.Combine(_systemRoot, "drivers", "etc", "hosts");

        /// <summary>
        /// block.sites.data Local Path. 
        /// </summary>
        public static string DataFilePath => Path.Combine(_hostDirectory, _filename);
    }
}
