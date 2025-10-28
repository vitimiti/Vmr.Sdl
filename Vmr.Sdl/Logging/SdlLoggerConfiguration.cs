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

/// <summary>Represents the user-defined delegate for handling SDL-based logging operations.</summary>
/// <param name="level">The severity level of the log entry, represented as a <see cref="LogLevel"/>.</param>
/// <param name="category">The category of the log entry, represented as a <see cref="LogCategory"/>.</param>
/// <param name="message">The log message to be processed.</param>
public delegate void SdlLogOutputFunction(LogLevel level, LogCategory category, string message);

/// <summary>Represents the configuration settings for the SDL-based logger implementation.</summary>
public class SdlLoggerConfiguration
{
    private readonly Dictionary<int, LogLevel> _categoryLevels = [];
    private SdlLogOutputFunction? _outputFunction;

    /// <summary>Initializes a new instance of the <see cref="SdlLoggerConfiguration"/> class with default settings.</summary>
    public SdlLoggerConfiguration() => InitializeDefaultLevels();

    /// <summary>Gets or sets the event ID filter. If set to non-zero, only messages with this event ID will be logged.</summary>
    public int EventId { get; set; }

    /// <summary>Gets or sets the category mapper used to resolve logger names to SDL categories.</summary>
    public ISdlCategoryMapper CategoryMapper { get; set; } = new DefaultSdlCategoryMapper();

    /// <summary>Gets or sets the output function that controls message processing and formatting.</summary>
    /// <remarks>
    /// This function will be called by SDL's native logging system for all messages that pass SDL's internal priority filtering.
    /// The function receives the original SDL callback signature: LogLevel, LogCategory, and the formatted message.
    /// </remarks>
    public SdlLogOutputFunction? OutputFunction
    {
        get => _outputFunction;
        set
        {
            _outputFunction = value;
            NativeSdl.SetLogOutputFunction(value ?? throw new ArgumentNullException(nameof(value)));
        }
    }

    /// <summary>Sets the minimum log level for a specific SDL category.</summary>
    /// <param name="categoryId">The SDL category ID.</param>
    /// <param name="level">The minimum log level.</param>
    /// <remarks>
    /// This configures both our internal filtering and SDL's native priority system.
    /// Messages below this level will be filtered out before reaching the output function.
    /// </remarks>
    public void SetCategoryLevel(int categoryId, LogLevel level)
    {
        _categoryLevels[categoryId] = level;
        NativeSdl.SetLogPriority(categoryId, NativeSdl.LogLevelToLogPriority(level));
    }

    /// <summary>Sets the minimum log level for a specific log category.</summary>
    /// <param name="category">The log category.</param>
    /// <param name="level">The minimum log level.</param>
    public void SetCategoryLevel(LogCategory category, LogLevel level) => SetCategoryLevel((int)category, level);

    /// <summary>Gets the minimum log level for a specific SDL category.</summary>
    /// <param name="categoryId">The SDL category ID.</param>
    /// <returns>The minimum log level, or null if not configured.</returns>
    public LogLevel? GetCategoryLevel(int categoryId) =>
        _categoryLevels.TryGetValue(categoryId, out LogLevel level) ? level : null;

    /// <summary>Registers a custom category with its minimum log level.</summary>
    /// <param name="loggerName">The logger name or pattern.</param>
    /// <param name="categoryId">The SDL category ID.</param>
    /// <param name="level">The minimum log level.</param>
    public void RegisterCustomCategory(string loggerName, int categoryId, LogLevel level)
    {
        CategoryMapper.RegisterCategory(loggerName, categoryId);
        SetCategoryLevel(categoryId, level);
    }

    private void InitializeDefaultLevels()
    {
        // Set up default log levels for predefined categories
        // These will configure both our internal filtering and SDL's native priorities
        SetCategoryLevel(LogCategory.Application, LogLevel.Information);
        SetCategoryLevel(LogCategory.Error, LogLevel.Error);
        SetCategoryLevel(LogCategory.Assert, LogLevel.Warning);
        SetCategoryLevel(LogCategory.System, LogLevel.Error);
        SetCategoryLevel(LogCategory.Audio, LogLevel.Error);
        SetCategoryLevel(LogCategory.Video, LogLevel.Error);
        SetCategoryLevel(LogCategory.Render, LogLevel.Error);
        SetCategoryLevel(LogCategory.Input, LogLevel.Error);
        SetCategoryLevel(LogCategory.Test, LogLevel.Trace);
        SetCategoryLevel(LogCategory.Gpu, LogLevel.Information);
    }
}
