// -----------------------------------------------------------------------
// <copyright file="ISdlCategoryMapper.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Vmr.Sdl.Logging;

/// <summary>Provides mapping between logger names and SDL log categories.</summary>
public interface ISdlCategoryMapper
{
    /// <summary>Gets the SDL log category for a given logger name.</summary>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>The corresponding SDL log category ID, or null if no mapping exists.</returns>
    int? GetCategoryId([NotNull] string loggerName);

    /// <summary>Registers a custom category mapping.</summary>
    /// <param name="loggerName">The logger name or pattern.</param>
    /// <param name="categoryId">The SDL category ID.</param>
    void RegisterCategory([NotNull] string loggerName, int categoryId);
}
