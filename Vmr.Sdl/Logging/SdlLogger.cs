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
    private LogCategory? _category;

    /// <summary>Begins the scope of the <see cref="SdlLogger"/>.</summary>
    /// <param name="state">The state of the logger.</param>
    /// <typeparam name="TState">The type of the state for the logger.</typeparam>
    /// <returns>An <see cref="IDisposable"/> object for the scope, or <see langword="null"/> if the scope couldn't be started.</returns>
    /// <remarks>The <see cref="state"/> CANNOT be <see langword="null"/>.</remarks>
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull => null;

    /// <summary>Determines whether the specified log level is enabled.
    /// </summary><param name="logLevel">The log level to check.</param>
    /// <returns><see langword="true"/> if the specified log level is enabled; otherwise, <see langword="false"/>.</returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        LogCategory? category = GetLogCategory();
        if (!category.HasValue)
        {
            return false;
        }

        SdlLoggerConfiguration config = getCurrentConfig();

        return config.LogCategoryToLevelMap.TryGetValue(category.Value, out LogLevel enabledLevel)
            && logLevel >= enabledLevel;
    }

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
        LogCategory? category = GetLogCategory();
        if (!category.HasValue || !IsEnabled(logLevel))
        {
            return;
        }

        SdlLoggerConfiguration config = getCurrentConfig();
        if (config.EventId != 0 && config.EventId != eventId.Id)
        {
            return;
        }

        var message = $"{name}\n\t{formatter(state, exception)}";
        var categoryInt = (int)category.Value;
        switch (logLevel)
        {
            case LogLevel.Trace:
                NativeSdl.LogTrace(categoryInt, message);
                break;
            case LogLevel.Debug:
                NativeSdl.LogDebug(categoryInt, message);
                break;
            case LogLevel.Information:
                NativeSdl.LogInfo(categoryInt, message);
                break;
            case LogLevel.Warning:
                NativeSdl.LogWarn(categoryInt, message);
                break;
            case LogLevel.Error:
                NativeSdl.LogError(categoryInt, message);
                break;
            case LogLevel.Critical:
                NativeSdl.LogCritical(categoryInt, message);
                break;
            case LogLevel.None:
            default:
                NativeSdl.LogMessage(categoryInt, NativeSdl.LogPriority.Invalid, message);
                break;
        }
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

    private LogCategory? GetLogCategory()
    {
        if (_category.HasValue)
        {
            return _category;
        }

        // Try to parse the logger name as a LogCategory enum
        if (Enum.TryParse<LogCategory>(name, true, out LogCategory parsedCategory))
        {
            _category = parsedCategory;
            return _category;
        }

        _category = name switch
        {
            _ when name.Contains("application", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Application,
            _ when name.Contains("error", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Error,
            _ when name.Contains("assert", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Assert,
            _ when name.Contains("system", StringComparison.InvariantCultureIgnoreCase) => LogCategory.System,
            _ when name.Contains("audio", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Audio,
            _ when name.Contains("video", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Video,
            _ when name.Contains("render", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Render,
            _ when name.Contains("input", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Input,
            _ when name.Contains("test", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Test,
            _ when name.Contains("gpu", StringComparison.InvariantCultureIgnoreCase) => LogCategory.Gpu,
            _ => null, // No matching category found
        };

        return _category;
    }
}
