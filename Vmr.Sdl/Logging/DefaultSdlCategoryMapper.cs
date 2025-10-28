// -----------------------------------------------------------------------
// <copyright file="DefaultSdlCategoryMapper.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Vmr.Sdl.Logging;

/// <summary>Default implementation of SDL category mapping with support for custom categories and patterns.</summary>
public class DefaultSdlCategoryMapper : ISdlCategoryMapper
{
    private readonly ConcurrentDictionary<string, int> _exactMappings = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, int> _patternMappings = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Initializes a new instance of the <see cref="DefaultSdlCategoryMapper"/> class with default mappings.</summary>
    public DefaultSdlCategoryMapper() => InitializeDefaultMappings();

    /// <inheritdoc/>
    public int? GetCategoryId([NotNull] string loggerName)
    {
        // First, try exact matches
        if (_exactMappings.TryGetValue(loggerName, out var exactMatch))
        {
            return exactMatch;
        }

        // Try to parse as LogCategory enum
        if (Enum.TryParse<LogCategory>(loggerName, true, out LogCategory category))
        {
            return (int)category;
        }

        // Try to parse as numeric category ID
        if (int.TryParse(loggerName, out var categoryId) && categoryId >= 0)
        {
            return categoryId;
        }

        // Try pattern matching
        foreach ((var pattern, var catId) in _patternMappings)
        {
            if (loggerName.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            {
                return catId;
            }
        }

        return null;
    }

    /// <inheritdoc/>
    public void RegisterCategory([NotNull] string loggerName, int categoryId) =>
        _exactMappings[loggerName] = categoryId;

    /// <summary>Registers a pattern-based category mapping.</summary>
    /// <param name="pattern">The pattern to match in logger names.</param>
    /// <param name="categoryId">The SDL category ID.</param>
    public void RegisterPattern(string pattern, int categoryId) => _patternMappings[pattern] = categoryId;

    private void InitializeDefaultMappings()
    {
        // Register default pattern mappings
        _patternMappings["application"] = (int)LogCategory.Application;
        _patternMappings["app"] = (int)LogCategory.Application;
        _patternMappings["error"] = (int)LogCategory.Error;
        _patternMappings["assert"] = (int)LogCategory.Assert;
        _patternMappings["system"] = (int)LogCategory.System;
        _patternMappings["sys"] = (int)LogCategory.System;
        _patternMappings["audio"] = (int)LogCategory.Audio;
        _patternMappings["video"] = (int)LogCategory.Video;
        _patternMappings["render"] = (int)LogCategory.Render;
        _patternMappings["input"] = (int)LogCategory.Input;
        _patternMappings["test"] = (int)LogCategory.Test;
        _patternMappings["gpu"] = (int)LogCategory.Gpu;
        _patternMappings["custom"] = (int)LogCategory.Custom;
    }
}
