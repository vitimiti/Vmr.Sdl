// -----------------------------------------------------------------------
// <copyright file="SdlLoggerExtensions.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Vmr.Sdl.Logging;

namespace Vmr.Sdl.Extensions;

/// <summary>Provides extension methods for configuring SDL logging within the Microsoft.Extensions.Logging framework.</summary>
public static class SdlLoggerExtensions
{
    /// <summary>Adds the SDL logger to the logging builder.</summary>
    /// <param name="builder">The logging builder.</param>
    /// <returns>The logging builder for method chaining.</returns>
    public static ILoggingBuilder AddSdlLogger([NotNull] this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        builder.Services.TryAddSingleton<ISdlCategoryMapper, DefaultSdlCategoryMapper>();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, SdlLoggerProvider>());
        LoggerProviderOptions.RegisterProviderOptions<SdlLoggerConfiguration, SdlLoggerProvider>(builder.Services);
        return builder;
    }

    /// <summary>Adds the SDL logger to the logging builder with custom configuration.</summary>
    /// <param name="builder">The logging builder.</param>
    /// <param name="configure">Configuration action.</param>
    /// <returns>The logging builder for method chaining.</returns>
    public static ILoggingBuilder AddSdlLogger(
        this ILoggingBuilder builder,
        Action<SdlLoggerConfiguration> configure
    ) => builder.AddSdlLogger().Services.Configure(configure).GetLoggingBuilder();

    /// <summary>Adds the SDL logger to the logging builder with a custom category mapper.</summary>
    /// <param name="builder">The logging builder.</param>
    /// <param name="categoryMapper">Custom category mapper.</param>
    /// <returns>The logging builder for method chaining.</returns>
    public static ILoggingBuilder AddSdlLogger(
        [NotNull] this ILoggingBuilder builder,
        ISdlCategoryMapper categoryMapper
    )
    {
        _ = builder.Services.AddSingleton(categoryMapper);
        return builder.AddSdlLogger();
    }

    private static LoggingBuilder GetLoggingBuilder(this IServiceCollection services) => new(services);

    private sealed class LoggingBuilder(IServiceCollection services) : ILoggingBuilder
    {
        public IServiceCollection Services => services;
    }
}
