using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public class Logger
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger("AswLogger");
        private static readonly log4net.ILog _reqLogger = log4net.LogManager.GetLogger("ReqLogger");

        public static void Info(object message)
        {
            _logger.Info(message);
        }
        public static void Error(object message)
        {
            _logger.Error(message);
        }
        public static void Req(object message)
        {
            _reqLogger.Error(message);
        }
    }
}