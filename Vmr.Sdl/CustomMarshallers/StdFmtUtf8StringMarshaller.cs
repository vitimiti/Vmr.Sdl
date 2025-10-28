// -----------------------------------------------------------------------
// <copyright file="StdFmtUtf8StringMarshaller.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.CustomMarshallers;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(ManagedToUnmanagedOut))]
internal static class StdFmtUtf8StringMarshaller
{
    // Based on the implementation of the Utf8StringMarshaller.ManagedToUnmanagedIn marshaller.
    public unsafe ref struct ManagedToUnmanagedIn
    {
        public static int BufferSize => 0x0100;

        private byte* _unmanaged;
        private bool _allocated;

        public void FromManaged(string? managed, Span<byte> buffer)
        {
            _allocated = false;

            if (managed is null)
            {
                _unmanaged = null;
                return;
            }

            managed = managed.Replace("%", "%%", StringComparison.InvariantCultureIgnoreCase);

            const int maxUtf8BytesPerChar = 3;
            if ((long)maxUtf8BytesPerChar * managed.Length >= buffer.Length)
            {
                var exactByteCount = checked(Encoding.UTF8.GetByteCount(managed) + 1);
                if (exactByteCount > buffer.Length)
                {
                    buffer = new Span<byte>((byte*)NativeMemory.Alloc((nuint)exactByteCount), exactByteCount);
                    _allocated = true;
                }
            }

            _unmanaged = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(buffer));

            var byteCount = Encoding.UTF8.GetBytes(managed, buffer);
            buffer[byteCount] = 0;
        }

        public readonly byte* ToUnmanaged() => _unmanaged;

        public void Free()
        {
            if (!_allocated)
            {
                return;
            }

            NativeMemory.Free(_unmanaged);
            _unmanaged = null;
        }
    }

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
            _managed = _managed?.Replace("%", "%%", StringComparison.InvariantCultureIgnoreCase);
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
