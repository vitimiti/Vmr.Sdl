// -----------------------------------------------------------------------
// <copyright file="Rectangle.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.CustomMarshallers;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Drawing;

/// <summary>Represents a rectangle defined by its X and Y coordinates, width, and height.</summary>
/// <param name="X">The X-coordinate of the rectangle.</param>
/// <param name="Y">The Y-coordinate of the rectangle.</param>
/// <param name="Width">The width of the rectangle.</param>
/// <param name="Height">The height of the rectangle.</param>
[NativeMarshalling(typeof(RectangleMarshaller))]
public record Rectangle(int X, int Y, int Width, int Height)
{
    /// <summary>Gets a value indicating whether the rectangle is empty.</summary>
    public bool IsEmpty => Width <= 0 || Height <= 0;

    /// <summary>Defines an explicit cast operator to convert a <see cref="Rectangle"/> to a <see cref="FRectangle"/>.</summary>
    /// <param name="rectangle">The instance of <see cref="Rectangle"/> to be converted.</param>
    /// <returns>A <see cref="FRectangle"/> instance converted from the given <see cref="Rectangle"/>.</returns>
    public static explicit operator FRectangle([NotNull] Rectangle rectangle) => rectangle.ToFRectangle();

    /// <summary>Converts the current <see cref="Rectangle"/> to a <see cref="FRectangle"/>.</summary>
    /// <returns>A <see cref="FRectangle"/> instance converted from the current <see cref="Rectangle"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FRectangle ToFRectangle() => new(X, Y, Width, Height);

    /// <summary>Determines if two rectangles intersect.</summary>
    /// <param name="other">The rectangle to check for intersection with the current rectangle.</param>
    /// <returns><see langword="true"/> if the rectangles intersect; otherwise, <see langword="false"/>.</returns>
    public bool HasIntersection([NotNull] Rectangle other) => NativeSdl.HasRectIntersection(this, other);

    /// <summary>Gets the intersection of two rectangles.</summary>
    /// <param name="other">The rectangle to check for intersection with the current rectangle.</param>
    /// <returns>The intersection of the two rectangles.</returns>
    public Rectangle GetIntersection([NotNull] Rectangle other) =>
        !NativeSdl.GetRectIntersection(this, other, out Rectangle result)
            ? throw new InvalidOperationException(
                $"Unable to get the intersection between {this} and {other} ({NativeSdl.GetError()})."
            )
            : result;

    /// <summary>Gets the union of two rectangles.</summary>
    /// <param name="other">The rectangle to check for union with the current rectangle.</param>
    /// <returns>The union of the two rectangles.</returns>
    public Rectangle GetUnion([NotNull] Rectangle other) =>
        !NativeSdl.GetRectUnion(this, other, out Rectangle result)
            ? throw new InvalidOperationException(
                $"Unable to get the union between {this} and {other} ({NativeSdl.GetError()})."
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
    public (Point Start, Point End) GetLineIntersection([NotNull] (Point Start, Point End) line)
    {
        var x1 = line.Start.X;
        var y1 = line.Start.Y;
        var x2 = line.End.X;
        var y2 = line.End.Y;

        return !NativeSdl.GetRectAndLineIntersection(this, ref x1, ref y1, ref x2, ref y2)
            ? throw new InvalidOperationException(
                $"Unable to get the line {line} intersection for the rectangle {this} ({NativeSdl.GetError()})."
            )
            : (new Point(x1, y1), new Point(x2, y2));
    }
}
