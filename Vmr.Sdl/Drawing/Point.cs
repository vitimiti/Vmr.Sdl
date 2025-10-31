// -----------------------------------------------------------------------
// <copyright file="Point.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.CustomMarshallers;

namespace Vmr.Sdl.Drawing;

/// <summary>Represents a point in a two-dimensional coordinate system.</summary>
[NativeMarshalling(typeof(PointMarshaller))]
public record Point
{
    /// <summary>Gets or sets the X-coordinate of the point.</summary>
    public int X { get; set; }

    /// <summary>Gets or sets the Y-coordinate of the point.</summary>
    public int Y { get; set; }

    /// <summary>Determines whether the point is inside the given rectangle.</summary>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns><see langword="true"/> if the point is inside the rectangle, <see langword="false"/> otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsInRectangle([NotNull] Rectangle rectangle) =>
        X >= rectangle.X && X < rectangle.X + rectangle.Width && Y >= rectangle.Y && Y < rectangle.Y + rectangle.Height;
}
