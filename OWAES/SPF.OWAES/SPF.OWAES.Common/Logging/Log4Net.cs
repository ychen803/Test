using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;


namespace SPF.OWAES.Common.Logging
{
    public class Log4Net
    {  
        public static ILog GetLog(Type type)
        {
            ILog log = null;
            try
            {
                log = LogManager.GetLogger(type);
                log4net.Config.XmlConfigurator.Configure();
                return log;
            }
            catch (Exception)
            {               
            }
            return log;
        }

        public static void LogInfo(Type type, string logMessage)
        {
            ILog log = GetLog(type);
            log.Info(logMessage);
        }

        public static void LogWarning(Type type, string logMessage)
        {
            ILog log = GetLog(type);
            log.Warn(logMessage);            
        }

        public static void LogDebug(Type type, string logMessage)
        {
            ILog log = GetLog(type);
            log.Debug(logMessage);
        }

        public static void LogFatal(Type type, string logMessage)
        {
            ILog log = GetLog(type);
            log.Fatal(logMessage);
        }

        public static void LogError(Type type, string logMessage)
        {
            ILog log = GetLog(type);
            log.Error(logMessage);
        }

        public static void LogAttributes<T>(T TObject, ILog logger) where T : class
        {
            if (logger != null)
            {
                Type t = TObject.GetType(); 
                PropertyInfo[] pi = t.GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    logger.Debug(p.Name + " : " + p.GetValue(TObject, null));
                }
            }
        }
    }
}
