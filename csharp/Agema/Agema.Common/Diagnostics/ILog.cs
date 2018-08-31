using System;
using System.Runtime.CompilerServices;

namespace Agema.Common.Diagnostics
{
    /// <summary>
    ///     Borrowed from NLog, until Common.Logging is built for .NET Standard
    /// </summary>
    public interface ILog
    {
        /// <summary>
        ///     Records trace info if enabled.  Accepts an optional operation that returns a string message to save processing if
        ///     trace logging is disabled. Ignore the other parameters, they are received by reflection.
        /// </summary>
        void Trace(Func<string> message = null, [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        ///     Records debug info if enabled.  Accepts an optional operation that returns a string message to save processing if
        ///     debug logging is disabled. Ignore the other parameters, they are received by reflection.
        /// </summary>
        void Debug(Func<string> message = null, [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        ///     Records info if enabled.  Accepts an optional operation that returns a string message to save processing if info
        ///     logging is disabled. Ignore the other parameters, they are received by reflection.
        /// </summary>
        void Info(Func<string> message = null, [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        ///     Records warn info if enabled.  Accepts an operation that returns a string message to save processing if warn
        ///     logging is disabled. Ignore the other parameters, they are received by reflection.
        /// </summary>
        void Warn(Func<string> message, Exception ex = null, [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        ///     Records warn info if enabled for an exception. Ignore the parameters, they are received by reflection.
        /// </summary>
        void Warn(Exception ex, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        ///     Records error info if enabled.  Accepts an operation that returns a string message to save processing if error
        ///     logging is disabled. Ignore the other parameters, they are received by reflection.
        /// </summary>
        void Error(Func<string> message, Exception ex = null, [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        ///     Records error info if enabled for an exception.  Ignore the parameters, they are received by reflection.
        /// </summary>
        void Error(Exception ex, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        ///     Records fatal info if enabled.  Accepts an operation that returns a string message to save processing if fatal
        ///     logging is disabled. Ignore the other parameters, they are received by reflection.
        /// </summary>
        void Fatal(Func<string> message, Exception ex, [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        ///     Records fatal info if enabled for an exception.  Ignore the parameters, they are received by reflection.
        /// </summary>
        void Fatal(Exception ex, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0);
    }
}