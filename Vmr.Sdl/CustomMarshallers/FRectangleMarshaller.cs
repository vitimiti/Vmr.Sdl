// -----------------------------------------------------------------------
// <copyright file="FRectangleMarshaller.cs" company="Vmr.Sdl">
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

[CustomMarshaller(typeof(FRectangle), MarshalMode.Default, typeof(FRectangleMarshaller))]
[CustomMarshaller(typeof(FRectangle), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
internal static class FRectangleMarshaller
{
    public static FRectangle ConvertToManaged(NativeSdl.FRect unmanaged) =>
        new()
        {
            X = unmanaged.X,
            Y = unmanaged.Y,
            Width = unmanaged.W,
            Height = unmanaged.H,
        };

    public static NativeSdl.FRect ConvertToUnmanaged(FRectangle managed) =>
        new()
        {
            X = managed.X,
            Y = managed.Y,
            W = managed.Width,
            H = managed.Height,
        };

    public unsafe ref struct ManagedToUnmanagedIn
    {
        private NativeSdl.FRect* _unmanaged;

        public void FromManaged(FRectangle? managed)
        {
            if (managed is null)
            {
                _unmanaged = null;
                return;
            }

            _unmanaged = (NativeSdl.FRect*)NativeMemory.Alloc((uint)Unsafe.SizeOf<NativeSdl.FRect>());
            _unmanaged->X = managed.X;
            _unmanaged->Y = managed.Y;
            _unmanaged->W = managed.Width;
            _unmanaged->H = managed.Height;
        }

        public readonly NativeSdl.FRect* ToUnmanaged() => _unmanaged;

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
