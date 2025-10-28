// -----------------------------------------------------------------------
// <copyright file="SdlLogger.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Logging;

/// <summary>A logger that uses SDL to output log messages.</summary>
/// <param name="name">The name of the logger.</param>
/// <param name="getCurrentConfig">The configuration for the logger.</param>
public sealed class SdlLogger(string name, Func<SdlLoggerConfiguration> getCurrentConfig) : IDisposable, ILogger
{
    /// <summary>Begins the scope of the <see cref="SdlLogger"/>.</summary>
    /// <param name="state">The state of the logger.</param>
    /// <typeparam name="TState">The type of the state for the logger.</typeparam>
    /// <returns>An <see cref="IDisposable"/> object for the scope, or <see langword="null"/> if the scope couldn't be started.</returns>
    /// <remarks>The <see cref="state"/> CANNOT be <see langword="null"/>.</remarks>
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull => null!;

    /// <summary>Determines whether logging is enabled for the specified log level.</summary>
    /// <param name="logLevel">The log level to check.</param>
    /// <returns><see langword="true"/> if logging is enabled for the specified level; otherwise, <see langword="false"/>.</returns>
    public bool IsEnabled(LogLevel logLevel) => getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

    /// <summary>Logs the given message or exception using the specified log level, event ID, and state information.</summary>
    /// <param name="logLevel">The level of the log message.</param>
    /// <param name="eventId">The identifier for the log event.</param>
    /// <param name="state">The state information to be logged.</param>
    /// <param name="exception">The exception to be logged, if any.</param>
    /// <param name="formatter">The function that formats the log message.</param>
    /// <typeparam name="TState">The type of the state being logged.</typeparam>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        [NotNull] Func<TState, Exception?, string> formatter
    )
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        SdlLoggerConfiguration config = getCurrentConfig();
        if (config.EventId != 0 && config.EventId != eventId.Id)
        {
            return;
        }

        LogCategory category = config.Category;
        ConsoleColor originalColor = Console.ForegroundColor;

        Console.ForegroundColor = config.LogLevelToColorMap[logLevel];

        switch (logLevel)
        {
            case LogLevel.Trace:
                NativeSdl.LogTrace((int)category, formatter(state, exception));
                break;
            case LogLevel.Debug:
                NativeSdl.LogDebug((int)category, formatter(state, exception));
                break;
            case LogLevel.Information:
                NativeSdl.LogInfo((int)category, formatter(state, exception));
                break;
            case LogLevel.Warning:
                NativeSdl.LogWarn((int)category, formatter(state, exception));
                break;
            case LogLevel.Error:
                NativeSdl.LogError((int)category, formatter(state, exception));
                break;
            case LogLevel.Critical:
                NativeSdl.LogCritical((int)category, formatter(state, exception));
                break;
            case LogLevel.None:
            default:
                NativeSdl.LogMessage((int)category, NativeSdl.LogPriority.Invalid, formatter(state, exception));
                break;
        }

        Console.ForegroundColor = originalColor;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="SdlLogger"/> class.</summary>
    ~SdlLogger() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources()
    {
        if (NativeSdl.LogOutputFunctionHandle.IsAllocated)
        {
            NativeSdl.LogOutputFunctionHandle.Free();
        }
    }
}
