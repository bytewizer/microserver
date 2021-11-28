using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

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
        /// Gets the FTP base directory.
        /// </summary>
        public string GetBaseDirectory()
        {
            //Debug.WriteLine($"DEBUG: GET LOCAL DIRECTORY: {_baseDirectory}");
            return _baseDirectory;
        }

        /// <summary>
        /// Gets the FTP local directory.
        /// </summary>
        public string GetLocalDirectory()
        {
            //Debug.WriteLine($"DEBUG: GET LOCAL DIRECTORY: {_localDirectory}");
            return _localDirectory;
        }

        /// <summary>
        /// Gets the FTP working directory.
        /// </summary>
        public string GetWorkingDirectory()
        {
            //Debug.WriteLine($"DEBUG: GET WORKING DIRECTORY: {_workingDirectory}");
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
        /// <param name="path">Absolute or relative FTP path of the directory.</param>
        public IEnumerator EnumerateDirectories(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
               path = GetLocalDirectory();
            }

            var localPath = GetLocalPath(path);

            return Directory.EnumerateDirectories(localPath).GetEnumerator();
        }

        /// <summary>
        /// Enumerate files.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the directory.</param>
        public IEnumerator EnumerateFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = GetLocalDirectory();
            }

            var localPath = GetLocalPath(path);

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

            if (Directory.Exists(fromLocalPath))
            {
                Directory.Move(fromLocalPath, toLocalPath);
                return;
            }

            if (File.Exists(fromLocalPath))
            {
                File.Move(fromLocalPath, toLocalPath);
                return;
            }

            throw new IOException("Invalid path");
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
        /// Gets the size of the file.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        /// <param name="size">The size of the file.</param>
        public bool GetFileSize(string path, out long size)
        {
            string localPath = GetLocalPath(path);

            size = 0;
            if (File.Exists(localPath))
            {
                var fileInfo = new FileInfo(localPath);
                size = fileInfo.Length;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the last write time of the file.
        /// </summary>
        /// <param name="path">Absolute or relative FTP path of the file.</param>
        /// <param name="dateTime">The last write time of the file.</param>
        public bool GetLastWriteTime(string path, out DateTime dateTime)
        {
            string localPath = GetLocalPath(path);

            dateTime = DateTime.MinValue;
            if (File.Exists(localPath))
            {
                var fileInfo = new FileInfo(localPath);
                dateTime = fileInfo.LastWriteTime;
                return true;
            }

            return false;
        }

        private string GetLocalPath(string path)
        {
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

            var localPath = new DirectoryInfo(Path.Combine(basePath, path.Replace("/", "\\"))).FullName;

            //Debug.WriteLine($"DEBUG: GET LOCAL PATH: {localPath}");

            return localPath;
        }

        private string GetFtpPath(string localPath)
        {
            var root = Path.GetPathRoot(localPath).ToCharArray();
            var path = Path.GetFullPath(localPath).TrimStart(root).Replace("\\", "/");
            var ftpPath = $"/{path}";

            //Debug.WriteLine($"DEGUG: GET FTP PATH: {ftpPath}");

            return ftpPath;
        }
    }
}