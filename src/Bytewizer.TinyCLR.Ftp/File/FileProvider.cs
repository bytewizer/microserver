﻿using System;
using System.IO;
using System.Collections;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Provides the same root directory to all users.
    /// </summary>
    public class FileProvider
    {
        /// <summary>
        /// The root directory for ftp.
        /// </summary>
        private readonly string _baseDirectory;

        /// <summary>
        /// The local working directory path.
        /// </summary>
        private string _localDirectory;

        /// <summary>
        /// The remote working directory path.
        /// </summary>
        private string _workingDirectory = "/";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProvider"/> class.
        /// </summary>
        /// <param name="baseDirectory">The root directory of the user.</param>
        public FileProvider(string baseDirectory)
        {
            if (!Directory.Exists(baseDirectory))
            {
                throw new IOException("Base directory doesn't exist");
            }
            _baseDirectory = baseDirectory;
            _localDirectory = baseDirectory;
        }

        /// <summary>
        /// Gets the FTP working directory.
        /// </summary>
        public string GetLocalDirectory()
        {
            //Debug.WriteLine($"LOCAL DIRECTORY: {_localDirectory}");
            return _localDirectory;
        }

        /// <summary>
        /// Gets the FTP working directory.
        /// </summary>
        public string GetWorkingDirectory()
        {
            //Debug.WriteLine($"WORKING DIRECTORY: {_workingDirectory}");
            return _workingDirectory;
        }

        /// <summary>
        /// Sets the FTP working directory.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of working directory.</param>
        /// <returns>Whether the setting succeeded or not.</returns>
        public bool SetWorkingDirectory(string path)
        {
            try
            {
                var localPath = GetLocalPath(path);
                if (Directory.Exists(localPath))
                {
                    _localDirectory = localPath;
                    _workingDirectory = GetFtpPath(localPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Enumerate directory.
        /// </summary>
        public IEnumerator EnumerateDirectories()
        {
            //var localPath = GetLocalPath(path);

            var localPath = GetLocalDirectory();

            return Directory.EnumerateDirectories(localPath).GetEnumerator();
        }

        /// <summary>
        /// Enumerate files.
        /// </summary>
        public IEnumerator EnumerateFiles()
        {
            //var localPath = GetLocalPath(path);
            var localPath = GetLocalDirectory();

            return Directory.EnumerateFiles(localPath).GetEnumerator();
        }

        /// <summary>
        /// Creates a directory.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the directory.</param>
        public void CreateDirectory(string path)
        {
            var localPath = GetLocalPath(path);

            Directory.CreateDirectory(localPath);
        }

        /// <summary>
        /// Deletes a directory.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the directory.</param>
        public void DeleteDirectory(string path)
        {
            var localPath = GetLocalPath(path);
            
            if (localPath == GetLocalPath(_baseDirectory))
            {
                throw new IOException("User tried to delete base directory.");
            }

            Directory.Delete(localPath, true);
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        public void Delete(string path)
        {
            var localPath = GetLocalPath(path);

            File.Delete(localPath);
        }

        /// <summary>
        /// Renames or moves a file or directory.
        /// </summary>
        /// <param name="fromPath">Absolute or relative FTP path of source file or directory.</param>
        /// <param name="toPath">Absolute or relative FTP path of target file or directory.</param>
        public void Rename(string fromPath, string toPath)
        {
            var fromLocalPath = GetLocalPath(fromPath);
            var toLocalPath = GetLocalPath(toPath);
            
            Directory.Move(fromLocalPath, toLocalPath);
        }

        /// <summary>
        /// Opens a file for reading.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        public FileStream OpenFileForRead(string path)
        {       
            string localPath = GetLocalPath(path);
                
            return File.OpenRead(localPath);
        }

        /// <summary>
        /// Opens a file for append.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        public FileStream OpenFileForAppend(string path)
        {
            string localPath = GetLocalPath(path);

            return File.Open(localPath, FileMode.Append);
        }

        /// <summary>
        /// Opens a file for writing.
        /// If the file already exists, opens it instead.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        /// <param name="mode">The file access mode.</param>
        public FileStream OpenFileForWrite(string path, FileMode mode)
        {
            string localPath = GetLocalPath(path);

            return File.Open(localPath, mode);
        }

        /// <summary>
        /// Creates a new file for writing.
        /// If the file already exists, replace it instead.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        public Stream CreateFileForWrite(string path)
        {
            string localPath = GetLocalPath(path);

            return File.Create(localPath);
        }

        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        public string GetFileSize(string path)
        {
            string localPath = GetLocalPath(path);

            if (File.Exists(localPath))
            {
                var fileInfo = new FileInfo(localPath);
                return fileInfo.Length.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the last write time of the file.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        public DateTime GetLastWriteTime(string path)
        {
            string localPath = GetLocalPath(path);

            if (File.Exists(localPath))
            {
                var fileInfo = new FileInfo(localPath);
                return fileInfo.LastWriteTime;
            }

            return DateTime.MaxValue;
        }

        /// <summary>
        /// Gets the names of files and directories.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        public IEnumerable GetNameListing(string path)
        {
            string localPath = GetLocalPath(path);

            return Directory.EnumerateFileSystemEntries(localPath);
        }

        private string GetLocalPath(string path)
        {
            //Debug.WriteLine($"PATH: {path}");

            if (string.IsNullOrEmpty(path))
            {
                return _baseDirectory;
            }

            string basePath;
            if (path.StartsWith("/"))
            {
                basePath = _baseDirectory;
            }
            else
            {
                basePath = _localDirectory;
            }

            var localPath = Path.Combine(basePath, path.Replace("/", "\\"));

            // Debug.WriteLine($"LOCAL PATH: {localPath}");

            return localPath;
        }

        private string GetFtpPath(string localPath)
        {
            //Debug.WriteLine($"FTP LOCAL PATH: {localPath}");

            var root = Path.GetPathRoot(localPath).ToCharArray();
            var path = Path.GetFullPath(localPath).TrimStart(root).Replace("\\", "/");
            var ftpPath = $"/{path}";

            //Debug.WriteLine($"FTP REMOTE PATH: {ftpPath}");

            return ftpPath;
        }
    }
}