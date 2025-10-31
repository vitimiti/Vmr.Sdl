// -----------------------------------------------------------------------
// <copyright file="PointExtensions.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Vmr.Sdl.Drawing;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Extensions;

/// <summary>Extensions for <see cref="Point"/>.</summary>
public static class PointExtensions
{
    /// <summary>Gets the enclosing rectangle for the specified points.</summary>
    /// <param name="points">The points to get the enclosing rectangle for.</param>
    /// <param name="clip">The optional clipping rectangle.</param>
    /// <returns>The enclosing rectangle for the specified points.</returns>
    /// <exception cref="InvalidOperationException">Unable to get the enclosing points rectangle.</exception>
    public static Rectangle GetEnclosingPoints(
        [NotNull] this IReadOnlyCollection<Point> points,
        Rectangle? clip = null
    ) =>
        !NativeSdl.GetRectEnclosingPoints([.. points], points.Count, clip, out Rectangle result)
            ? throw new InvalidOperationException("Unable to get the enclosing points rectangle.")
            : result;
}
