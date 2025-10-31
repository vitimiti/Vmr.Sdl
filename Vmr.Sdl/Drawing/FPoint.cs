// -----------------------------------------------------------------------
// <copyright file="FPoint.cs" company="Vmr.Sdl">
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

/// <summary>Represents a point in a two-dimensional floating-point coordinate system.</summary>
/// <param name="X">The X-coordinate of the point.</param>
/// <param name="Y">The Y-coordinate of the point.</param>
[NativeMarshalling(typeof(FPointMarshaller))]
public record FPoint(float X, float Y)
{
    /// <summary>Determines whether the point is inside the given rectangle.</summary>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns><see langword="true"/> if the point is inside the rectangle, <see langword="false"/> otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsInRectangle([NotNull] FRectangle rectangle) =>
        X >= rectangle.X && X < rectangle.X + rectangle.Width && Y >= rectangle.Y && Y < rectangle.Y + rectangle.Height;
}
