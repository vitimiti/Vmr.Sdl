// -----------------------------------------------------------------------
// <copyright file="SdlOwnedUtf8StringMarshaller.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.CustomMarshallers;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(ManagedToUnmanagedOut))]
internal static class SdlOwnedUtf8StringMarshaller
{
    public unsafe ref struct ManagedToUnmanagedOut
    {
        private byte* _unmanaged;
        private string? _managed;

        public void FromUnmanaged(byte* unmanaged)
        {
            if (unmanaged is null)
            {
                _unmanaged = null;
                _managed = null;
                return;
            }

            _unmanaged = NativeSdl.StrDup(unmanaged);
            _managed = Utf8StringMarshaller.ConvertToManaged(_unmanaged);
        }

        public readonly string? ToManaged() => _managed;

        public void Free()
        {
            if (_unmanaged is null)
            {
                return;
            }

            NativeSdl.Free(_unmanaged);
            _unmanaged = null;
        }
    }
}
