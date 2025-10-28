// -----------------------------------------------------------------------
// <copyright file="SdlLoggerConfiguration.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Logging;

/// <summary>A delegate to granularly control the output of the SDL logging system.</summary>
/// <param name="level">The <see cref="LogLevel"/> of the log message.</param>
/// <param name="category">The <see cref="LogCategory"/> of the log message.</param>
/// <param name="message">The log message.</param>
public delegate void SdlLogOutputFunction(LogLevel level, LogCategory category, string message);

/// <summary>Represents the configuration settings for the SDL-based logger implementation.</summary>
public class SdlLoggerConfiguration
{
    private Dictionary<LogCategory, LogLevel> _map = new()
    {
        { LogCategory.Application, LogLevel.Information },
        { LogCategory.Error, LogLevel.Error },
        { LogCategory.Assert, LogLevel.Warning },
        { LogCategory.System, LogLevel.Error },
        { LogCategory.Audio, LogLevel.Error },
        { LogCategory.Video, LogLevel.Error },
        { LogCategory.Render, LogLevel.Error },
        { LogCategory.Input, LogLevel.Error },
        { LogCategory.Test, LogLevel.Trace },
        { LogCategory.Gpu, LogLevel.Information },
    };

    private SdlLogOutputFunction? _callback;

    /// <summary>Gets or sets the event ID to use for logging.</summary>
    public int EventId { get; set; }

    /// <summary>Gets or sets the mapping between log categories and their corresponding log levels.</summary>
    /// <remarks>This dictionary determines the log level assigned to each log category. Setting this property updates the native SDL log priorities to reflect the specified log levels per category.</remarks>
    public Dictionary<LogCategory, LogLevel> LogCategoryToLevelMap
    {
        get => _map;
        set
        {
            foreach ((LogCategory category, LogLevel level) in value)
            {
                NativeSdl.SetLogPriority((int)category, NativeSdl.LogLevelToLogPriority(level));
                _map[category] = level;
            }
        }
    }

    /// <summary>Gets or sets the delegate used to control the output of the SDL logging system.</summary>
    public SdlLogOutputFunction? OutputFunction
    {
        get => _callback;
        set
        {
            NativeSdl.SetLogOutputFunction(value);
            _callback = value;
        }
    }
}
