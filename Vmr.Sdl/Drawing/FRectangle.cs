// -----------------------------------------------------------------------
// <copyright file="FRectangle.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.CustomMarshallers;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Drawing;

/// <summary>Represents a rectangle defined by its position (X, Y) and dimensions (Width, Height) with support for various geometric operations.</summary>
[NativeMarshalling(typeof(FRectangleMarshaller))]
public record FRectangle
{
    /// <summary>Gets or sets the X-coordinate of the rectangle.</summary>
    public float X { get; set; }

    /// <summary>Gets or sets the Y-coordinate of the rectangle.</summary>
    public float Y { get; set; }

    /// <summary>Gets or sets the width of the rectangle.</summary>
    public float Width { get; set; }

    /// <summary>Gets or sets the height of the rectangle.</summary>
    public float Height { get; set; }

    /// <summary>Gets a value indicating whether the rectangle is empty.</summary>
    public bool IsEmpty => Width <= 0 || Height <= 0;

    /// <summary>Determines if two rectangles intersect.</summary>
    /// <param name="other">The rectangle to check for intersection with the current rectangle.</param>
    /// <returns><see langword="true"/> if the rectangles intersect; otherwise, <see langword="false"/>.</returns>
    public bool HasIntersection([NotNull] FRectangle other) => NativeSdl.HasRectIntersectionFloat(this, other);

    /// <summary>Gets the intersection of two rectangles.</summary>
    /// <param name="other">The rectangle to check for intersection with the current rectangle.</param>
    /// <returns>The intersection of the two rectangles.</returns>
    public FRectangle GetIntersection([NotNull] FRectangle other) =>
        !NativeSdl.GetRectIntersectionFloat(this, other, out FRectangle result)
            ? throw new InvalidOperationException(
                $"Unable to get the intersection between {this} and {other} ({NativeSdl.GetError()}.)"
            )
            : result;

    /// <summary>Gets the union of two rectangles.</summary>
    /// <param name="other">The rectangle to check for union with the current rectangle.</param>
    /// <returns>The union of the two rectangles.</returns>
    public FRectangle GetUnion([NotNull] FRectangle other) =>
        !NativeSdl.GetRectUnionFloat(this, other, out FRectangle result)
            ? throw new InvalidOperationException(
                $"Unable to get the union between {this} and {other} ({NativeSdl.GetError()}.)"
            )
            : result;

    /// <summary>Gets the line intersection of a rectangle and a line.</summary>
    /// <param name="line">The line to check for intersection with the current rectangle.</param>
    /// <returns>The intersection of the line and the rectangle.</returns>
    [SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
        Justification = "This is an attribute followed by a tuple."
    )]
    public (FPoint Start, FPoint End) GetLineIntersection([NotNull] (FPoint Start, FPoint End) line)
    {
        var x1 = line.Start.X;
        var y1 = line.Start.Y;
        var x2 = line.End.X;
        var y2 = line.End.Y;

        return !NativeSdl.GetRectAndLineIntersectionFloat(this, ref x1, ref y1, ref x2, ref y2)
            ? throw new InvalidOperationException(
                $"Unable to get the line {line} intersection for the rectangle {this} ({NativeSdl.GetError()}.)"
            )
            : (new FPoint { X = x1, Y = y1 }, new FPoint { X = x2, Y = y2 });
    }
}
