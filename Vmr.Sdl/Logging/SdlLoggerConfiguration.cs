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

public class SdlLoggerConfiguration
{
    private SdlLogOutputFunction? _callback;

    /// <summary>Initializes a new instance of the <see cref="SdlLoggerConfiguration"/> class.</summary>
    /// <param name="sdlLogLevel">The <see cref="LogLevel"/> to set the SDL categories to.</param>
    public SdlLoggerConfiguration(LogLevel sdlLogLevel = LogLevel.Information) =>
        NativeSdl.SetLogPriorities(NativeSdl.LogLevelToLogPriority(sdlLogLevel));

    /// <summary>Gets or sets the event ID to use for logging.</summary>
    public int EventId { get; set; }

    /// <summary>Gets or sets the category to use for logging.</summary>
    public LogCategory Category { get; set; }

    /// <summary>Gets the log level to color map.</summary>
    public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } =
        new()
        {
            [LogLevel.Information] = ConsoleColor.Green,
            [LogLevel.Warning] = ConsoleColor.Yellow,
            [LogLevel.Error] = ConsoleColor.Red,
            [LogLevel.Critical] = ConsoleColor.Red,
            [LogLevel.Trace] = ConsoleColor.Gray,
            [LogLevel.Debug] = ConsoleColor.Gray,
            [LogLevel.None] = ConsoleColor.Gray,
        };

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
