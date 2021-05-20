/////------------------------------------------------------------------------
////<copyright file="LoggerManager.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////--------------------------------------------------------------------------

namespace LoggerService
{
    using System;
    using NLog;

    /// <summary>
    /// Logger manager class
    /// </summary>
    public class LoggerManager : ILoggerManager
    {
        /// <summary>
        /// Takes the current class log
        /// </summary>
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Debug Log
        /// </summary>
        /// <param name="message">Debug message</param>
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Error Log
        /// </summary>
        /// <param name="message">Error message</param>
        public void LogError(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// Log Information
        /// </summary>
        /// <param name="message">Info Message</param>
        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Warning Log
        /// </summary>
        /// <param name="message">Warning message</param>
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
