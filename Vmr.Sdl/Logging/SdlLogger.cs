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

/// <summary>A logger that integrates with SDL's logging system while providing full ILogger compatibility.</summary>
/// <param name="name">The name of the logger.</param>
/// <param name="getCurrentConfig">Function to get the current configuration.</param>
public sealed class SdlLogger(string name, Func<SdlLoggerConfiguration> getCurrentConfig) : IDisposable, ILogger
{
    private int? _categoryId;

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull => null;

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        var categoryId = GetCategoryId();
        if (!categoryId.HasValue)
        {
            return false;
        }

        SdlLoggerConfiguration config = getCurrentConfig();
        LogLevel? configuredLevel = config.GetCategoryLevel(categoryId.Value);

        // If no level is configured for this category, use SDL's default behavior
        if (!configuredLevel.HasValue)
        {
            return logLevel != LogLevel.None;
        }

        // Check if the level meets the minimum configured level
        return logLevel >= configuredLevel.Value;
    }

    /// <inheritdoc/>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        [NotNull] Func<TState, Exception?, string> formatter
    )
    {
        var categoryId = GetCategoryId();
        if (!categoryId.HasValue || !IsEnabled(logLevel))
        {
            return;
        }

        SdlLoggerConfiguration config = getCurrentConfig();

        // Check event ID filter
        if (config.EventId != 0 && config.EventId != eventId.Id)
        {
            return;
        }

        // Format the message
        var message = formatter(state, exception);
        if (string.IsNullOrEmpty(message) && exception == null)
        {
            return;
        }

        // Create the full formatted message including logger name
        var fullMessage = $"{name}: {message}";
        if (exception != null)
        {
            fullMessage += Environment.NewLine + exception.ToString();
        }

        // Send the message to SDL's logging system
        // SDL will handle filtering based on the priority we set via SetLogPriority
        // and will call our output function (if configured) through the native callback
        LogToSdl(logLevel, categoryId.Value, fullMessage);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="SdlLogger"/> class.</summary>
    ~SdlLogger() => ReleaseUnmanagedResources();

    private static void LogToSdl(LogLevel level, int categoryId, string message)
    {
        // Send message to SDL's native logging system
        // SDL will check its internal priority settings and potentially call our output function
        switch (level)
        {
            case LogLevel.Trace:
                NativeSdl.LogTrace(categoryId, message);
                break;
            case LogLevel.Debug:
                NativeSdl.LogDebug(categoryId, message);
                break;
            case LogLevel.Information:
                NativeSdl.LogInfo(categoryId, message);
                break;
            case LogLevel.Warning:
                NativeSdl.LogWarn(categoryId, message);
                break;
            case LogLevel.Error:
                NativeSdl.LogError(categoryId, message);
                break;
            case LogLevel.Critical:
                NativeSdl.LogCritical(categoryId, message);
                break;
            case LogLevel.None:
            default:
                NativeSdl.LogMessage(categoryId, NativeSdl.LogPriority.Invalid, message);
                break;
        }
    }

    private static void ReleaseUnmanagedResources()
    {
        if (NativeSdl.LogOutputFunctionHandle.IsAllocated)
        {
            NativeSdl.LogOutputFunctionHandle.Free();
        }
    }

    private int? GetCategoryId()
    {
        if (_categoryId.HasValue)
        {
            return _categoryId;
        }

        SdlLoggerConfiguration config = getCurrentConfig();
        _categoryId = config.CategoryMapper.GetCategoryId(name);
        return _categoryId;
    }
}
