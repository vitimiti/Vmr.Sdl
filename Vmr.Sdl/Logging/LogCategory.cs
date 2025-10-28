// -----------------------------------------------------------------------
// <copyright file="LogCategory.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Vmr.Sdl.Logging;

/// <summary>The predefined log categories.</summary>
/// <remarks>By default, the application and GPU categories are enabled at the <see cref="LogPriority.Info"/> level, the assert category is enabled at the <see cref="LogPriority.Warn"/> level, test is enabled at the <see cref="LogPriority.Verbose"/> level and all other categories are enabled at the <see cref="LogPriority.Error"/> level.</remarks>
public enum LogCategory
{
    /// <summary>The application category.</summary>
    Application,

    /// <summary>The error category.</summary>
    Error,

    /// <summary>The assert category.</summary>
    Assert,

    /// <summary>The system category.</summary>
    System,

    /// <summary>The audio category.</summary>
    Audio,

    /// <summary>The video category.</summary>
    Video,

    /// <summary>The render category.</summary>
    Render,

    /// <summary>The input category.</summary>
    Input,

    /// <summary>The test category.</summary>
    Test,

    /// <summary>The gpu category.</summary>
    Gpu,

    /// <summary>The custom category.</summary>
    /// <remarks>Beyond this point, the application is responsible for defining its own categories.</remarks>
    /// <example>
    /// <code>
    /// public enum MyLogCategory
    /// {
    ///     Category1 = LogCategory.Custom,
    ///     Category2,
    ///     Category3,
    ///     ...
    /// }
    /// </code>
    /// </example>
    Custom = 19,
}
