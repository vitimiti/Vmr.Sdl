// -----------------------------------------------------------------------
// <copyright file="RectangleMarshaller.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.Drawing;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.CustomMarshallers;

[CustomMarshaller(typeof(Rectangle), MarshalMode.Default, typeof(RectangleMarshaller))]
[CustomMarshaller(typeof(Rectangle), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
internal static class RectangleMarshaller
{
    public static Rectangle ConvertToManaged(NativeSdl.Rect unmanaged) =>
        new(unmanaged.X, unmanaged.Y, unmanaged.W, unmanaged.H);

    public static NativeSdl.Rect ConvertToUnmanaged(Rectangle managed) =>
        new()
        {
            X = managed.X,
            Y = managed.Y,
            W = managed.Width,
            H = managed.Height,
        };

    public unsafe ref struct ManagedToUnmanagedIn
    {
        private NativeSdl.Rect* _unmanaged;

        public void FromManaged(Rectangle? managed)
        {
            if (managed is null)
            {
                _unmanaged = null;
                return;
            }

            _unmanaged = (NativeSdl.Rect*)NativeMemory.Alloc((uint)Unsafe.SizeOf<NativeSdl.Rect>());
            _unmanaged->X = managed.X;
            _unmanaged->Y = managed.Y;
            _unmanaged->W = managed.Width;
            _unmanaged->H = managed.Height;
        }

        public readonly NativeSdl.Rect* ToUnmanaged() => _unmanaged;

        public void Free()
        {
            if (_unmanaged is null)
            {
                return;
            }

            NativeMemory.Free(_unmanaged);
            _unmanaged = null;
        }
    }
}
