using System;
using System.IO;

namespace MicroServer.Net.Http.Files
{
    /// <summary>
    /// Small DTO for Directories
    /// </summary>
    public class DirectoryInformation
    {
        /// <summary>
        /// Gets directory name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets when the directory was modified
        /// </summary>
        public DateTime LastModifiedAtUtc { get; set; }

        /// <summary>
        /// Gets when the directory was modified
        /// </summary>
        public DirectoryInfo Parent { get; set; }

        /// <summary>
        /// Gets when the directory was modified
        /// </summary>
        public DirectoryInfo Root { get; set; }
    }
}