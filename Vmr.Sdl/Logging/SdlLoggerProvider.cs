// -----------------------------------------------------------------------
// <copyright file="SdlLoggerProvider.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Vmr.Sdl.Logging;

/// <summary>Provides a logging provider implementation for SDL logs with capabilities to create and manage SDL-based loggers.</summary>
/// <remarks>This class integrates with the Microsoft.Extensions.Logging infrastructure to provide an SDL-specific logger implementation. The provider manages a collection of loggers and supports runtime configuration changes.</remarks>
[UnsupportedOSPlatform("browser")]
[ProviderAlias("Sdl")]
public sealed class SdlLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private readonly ConcurrentDictionary<string, SdlLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

    private SdlLoggerConfiguration _currentConfig;

    /// <summary>Initializes a new instance of the <see cref="SdlLoggerProvider"/> class.</summary>
    /// <param name="config">The configuration for the logger.</param>
    public SdlLoggerProvider([NotNull] IOptionsMonitor<SdlLoggerConfiguration> config)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updateConfig => _currentConfig = updateConfig);
    }

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, static (name, arg) => new SdlLogger(name, arg.GetCurrentConfig), this);

    /// <inheritdoc/>
    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }

    private SdlLoggerConfiguration GetCurrentConfig() => _currentConfig;
}
