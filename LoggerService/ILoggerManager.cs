/////------------------------------------------------------------------------
////<copyright file="ILoggerManager.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////--------------------------------------------------------------------------

namespace LoggerService
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Logger Manager interface
    /// </summary>
    public interface ILoggerManager
    {
        /// <summary>
        /// Log Information
        /// </summary>
        /// <param name="message">Info Message</param>
        void LogInfo(string message);

        /// <summary>
        /// Warning Log
        /// </summary>
        /// <param name="message">Warning message</param>
        void LogWarn(string message);

        /// <summary>
        /// Debug Log
        /// </summary>
        /// <param name="message">Debug message</param>
        void LogDebug(string message);

        /// <summary>
        /// Error Log
        /// </summary>
        /// <param name="message">Error message</param>
        void LogError(string message);
    }
}
