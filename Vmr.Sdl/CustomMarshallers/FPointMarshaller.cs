// -----------------------------------------------------------------------
// <copyright file="FPointMarshaller.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.Drawing;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.CustomMarshallers;

[CustomMarshaller(typeof(FPoint), MarshalMode.ElementIn, typeof(ElementIn))]
internal static class FPointMarshaller
{
    public static class ElementIn
    {
        public static FPoint ConvertToManaged(NativeSdl.FPoint unmanaged) => new() { X = unmanaged.X, Y = unmanaged.Y };

        public static NativeSdl.FPoint ConvertToUnmanaged(FPoint managed) => new() { X = managed.X, Y = managed.Y };
    }
}
