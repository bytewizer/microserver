using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;

using Microsoft.SPOT;
using MicroServer.Logging;
using MicroServer.Utilities;

namespace MicroServer
{
    public static class ConfigManager
    {
        private const string NODE_ADD = "add";
        private const string NODE_KEY = "key";
        private const string NODE_VALUE = "value";

        private static Hashtable _cfgSettings;

        static ConfigManager()
        {
            _cfgSettings = new Hashtable();
        }

        public static Hashtable GetSettings()
        {
            return _cfgSettings;
        }

        public static string GetSetting(string key)
        {
            return GetSetting(key, null);
        }

        public static string GetSetting(string key, string defaultValue)
        {
            if (!_cfgSettings.Contains(key))
                return defaultValue;

            return (string)_cfgSettings[key];
        }

        public static void SetSetting(string key, string value)
        {
            _cfgSettings[key] = value;
        }

        public static void SetSettings(Hashtable value)
        {
            _cfgSettings = value;
        }

        public static void Load(string filePath)
        {
            if (!StringUtility.IsNullOrEmpty(Path.GetFullPath(filePath)))
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        using (var configStream = new FileStream(filePath, FileMode.Open))
                        {
                            if (configStream != null)
                            {
                                ConfigManager.Load(configStream);
                                Logger.WriteDebug("MicroServer.ConfigManager", "Configuration settings successfuly loaded");
                            }
                        }
                    }
                    else
                    {
                        Logger.WriteInfo("MicroServer.ConfigManager", "Configuration settings failed to load: '" + filePath + "' not found");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteError("MicroServer.ConfigManager", "Configuration settings failed to load", ex);
                }
            }
        }

        public static void Load(Stream xmlStream)
        {
            _cfgSettings.Clear();

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreWhitespace = true;
            readerSettings.IgnoreComments = false;

            XmlReader xmlReader = XmlReader.Create(xmlStream, readerSettings);
            while (!xmlReader.EOF)
            {
                xmlReader.Read();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (xmlReader.Name == NODE_ADD)
                        {
                            string key = xmlReader.GetAttribute(NODE_KEY);
                            string value = xmlReader.GetAttribute(NODE_VALUE);
                            _cfgSettings.Add(key, value);
                            //Debug.Print(key + "=" + value);
                        }
                        break;
                    default:
                        //Debug.Print(xmlReader.NodeType.ToString());
                        break;
                }
            }
        }

        public static void Save(string filePath)
        {
            if (!StringUtility.IsNullOrEmpty(Path.GetFullPath(filePath)))
            {
                try
                {
                    using (var configStream = new FileStream(filePath, FileMode.Create))
                    {
                        if (configStream != null && File.Exists(filePath))
                        {
                            ConfigManager.Save(configStream);
                            Logger.WriteDebug("MicroServer.ConfigManager", "Configuration settings successfuly loaded");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteError("MicroServer.ConfigManager", "Configuration settings failed to load", ex);
                }
            }
        }

        public static void Save(Stream xmlStream)
        {
            using (StreamWriter xmlWriter = new StreamWriter(xmlStream))
            {
                xmlWriter.WriteLine(@"<?xml version='1.0' encoding='utf-8'?>");
                xmlWriter.WriteLine("<Configuration>");
                xmlWriter.WriteLine("\t<Settings>");

                foreach (DictionaryEntry item in _cfgSettings)
                {
                    string itemLine = "\t\t<add key=\"" + item.Key + "\" value=\"" + item.Value + "\" />";
                    xmlWriter.WriteLine(itemLine);
                }

                xmlWriter.WriteLine("\t</Settings>");
                xmlWriter.Write("</Configuration>");
                xmlWriter.Close();
            }
        }
    }
}
