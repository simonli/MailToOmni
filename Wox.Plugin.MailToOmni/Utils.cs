using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Wox.Plugin.MailToOmni
{
    public static class Utils
    {
        public static Configuration GetConfig()
        {
            //获取调用当前正在执行的方法的方法的 Assembly  
            var assembly = Assembly.GetCallingAssembly();
            var path = $"{assembly.Location}.config";

            if (File.Exists(path) == false)
            {
                var msg = $"{path}路径下的文件未找到 ";
                throw new FileNotFoundException(msg);
            }

            try
            {
                var configFile = new ExeConfigurationFileMap {ExeConfigFilename = path};
                var config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
                return config;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
