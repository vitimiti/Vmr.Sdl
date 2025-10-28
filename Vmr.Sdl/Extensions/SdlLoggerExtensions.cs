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
    /// <summary>Adds the SDL logger to the logging builder, configuring the necessary services and enabling SDL logging functionality within the application's logging framework.</summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to configure SDL logging on. This is typically provided within the application's dependency injection system.</param>
    /// <returns>The <see cref="ILoggingBuilder"/> with SDL logging configured, allowing method chaining for additional logging extensions.</returns>
    public static ILoggingBuilder AddSdlLogger([NotNull] this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, SdlLoggerProvider>());
        LoggerProviderOptions.RegisterProviderOptions<SdlLoggerConfiguration, SdlLoggerProvider>(builder.Services);
        return builder;
    }

    /// <summary>Adds the SDL logger to the logging builder, configuring the necessary services and enabling SDL logging functionality with additional custom configuration options.</summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to configure SDL logging on. This is typically provided within the application's dependency injection system.</param>
    /// <param name="configure">A delegate to configure the <see cref="SdlLoggerConfiguration"/>, allowing customization of SDL logging behavior.</param>
    /// <returns>The <see cref="ILoggingBuilder"/> with SDL logging configured, allowing method chaining for additional logging extensions.</returns>
    public static ILoggingBuilder AddSdlLogger(this ILoggingBuilder builder, Action<SdlLoggerConfiguration> configure)
    {
        _ = builder.AddSdlLogger().Services.Configure(configure);
        return builder;
    }
}
