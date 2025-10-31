// -----------------------------------------------------------------------
// <copyright file="PointMarshaller.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.Drawing;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.CustomMarshallers;

[CustomMarshaller(typeof(Point), MarshalMode.ElementIn, typeof(ElementIn))]
internal static class PointMarshaller
{
    public static class ElementIn
    {
        public static Point ConvertToManaged(NativeSdl.Point unmanaged) => new() { X = unmanaged.X, Y = unmanaged.Y };

        public static NativeSdl.Point ConvertToUnmanaged(Point managed) => new() { X = managed.X, Y = managed.Y };
    }
}
