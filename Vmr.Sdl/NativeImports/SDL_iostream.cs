// -----------------------------------------------------------------------
// <copyright file="SDL_iostream.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.CustomMarshallers;
using Vmr.Sdl.Io;

namespace Vmr.Sdl.NativeImports;

internal static partial class NativeSdl
{
    public const string PropIoStreamWindowsHandlePointer = "SDL.iostream.windows.handle";
    public const string PropIoStreamStdioFilePointer = "SDL.iostream.stdio.file";
    public const string PropIoStreamFileDescriptorNumber = "SDL.iostream.file_descriptor";
    public const string PropIoStreamAndroidAAssetPointer = "SDL.iostream.android.aasset";
    public const string PropIoStreamMemoryPointer = "SDL.iostream.memory.base";
    public const string PropIoStreamMemorySizeNumber = "SDL.iostream.memory.size";
    public const string PropIoStreamDynamicMemoryPointer = "SDL.iostream.dynamic.memory";
    public const string PropIoStreamDynamicChunkSizeNumber = "SDL.iostream.dynamic.chunksize";

    public static string ModeAndAccessToSdlMode(FileMode mode, FileAccess access, bool isBinaryFile)
    {
#pragma warning disable IDE0072 // Add missing cases to switch expression
        var result = mode switch
        {
            FileMode.Open when access is FileAccess.ReadWrite => "r+",
            FileMode.Truncate when access is FileAccess.ReadWrite => "w+",
            FileMode.Append when access is FileAccess.ReadWrite => "a+",
            _ => mode switch
            {
                FileMode.Open when access is FileAccess.Read => "r",
                FileMode.Truncate when access is FileAccess.Write => "w",
                FileMode.Append when access is FileAccess.Write => "a",
                _ => throw new InvalidOperationException(
                    $"Invalid mode and access combination ({mode}, {access}) for SDL file I/O."
                ),
            },
        };
#pragma warning restore IDE0072 // Add missing cases to switch expression

        if (isBinaryFile)
        {
            result += "b";
        }

        return result;
    }

    public static IoWhence SeekOriginToIoWhence(SeekOrigin origin) =>
        origin switch
        {
            SeekOrigin.Begin => IoWhence.Set,
            SeekOrigin.Current => IoWhence.Cur,
            SeekOrigin.End => IoWhence.End,
            _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null),
        };

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_IOFromFile", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IoStream IoFromFile(string file, string mode);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_IOFromMem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IoStream IoFromMem(
#pragma warning disable IDE0055 // Fix formatting
        [In] [MarshalUsing(typeof(ArrayMarshaller<byte, byte>), CountElementName = nameof(size))] byte[] mem,
#pragma warning restore IDE0055 // Fix formatting
        CULong size
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_IOFromConstMem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IoStream IoFromConstMem(
#pragma warning disable IDE0055 // Fix formatting
        [In] [MarshalUsing(typeof(ArrayMarshaller<byte, byte>), CountElementName = nameof(size))] byte[] mem,
#pragma warning restore IDE0055 // Fix formatting
        CULong size
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_IOFromDynamicMem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IoStream IoFromDynamicMem();

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_CloseIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool CloseIo(IoStream context);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetIOProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial PropertiesId GetIoProperties(IoStream context);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetIOStatus")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IoStatus GetIoStatus(IoStream context);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetIOSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long GetIoSize(IoStream context);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SeekIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long SeekIo(IoStream context, long offset, IoWhence whence);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_TellIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long TellIo(IoStream context);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CULong ReadIo(
        IoStream context,
#pragma warning disable IDE0055 // Fix formatting
        [Out] [MarshalUsing(typeof(ArrayMarshaller<byte, byte>), CountElementName = nameof(size))] byte[] buffer,
#pragma warning restore IDE0055 // Fix formatting
        CULong size
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "WriteIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CULong WriteIo(
        IoStream context,
#pragma warning disable IDE0055 // Fix formatting
        [In] [MarshalUsing(typeof(ArrayMarshaller<byte, byte>), CountElementName = nameof(size))] byte[] buffer,
#pragma warning restore IDE0055 // Fix formatting
        CULong size
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_IOprintf",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(StdFmtUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CULong IoPrint(IoStream context, string text);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_FlushIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool FlushIo(IoStream context);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_LoadFile_IO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial byte* LoadFileIo(
        IoStream src,
        out CULong dataSize,
        [MarshalAs(UnmanagedType.I4)] bool closeIo
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_LoadFile", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial byte* LoadFile(string file, out CULong dataSize);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SaveFile_IO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SaveFileIo(
        IoStream src,
#pragma warning disable IDE0055 // Fix formatting
        [In] [MarshalUsing(typeof(ArrayMarshaller<byte, byte>))] byte[] data,
#pragma warning restore IDE0055 // Fix formatting
        CULong dataSize,
        [MarshalAs(UnmanagedType.I4)] bool closeIo
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SaveFile", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SaveFile(
        string file,
#pragma warning disable IDE0055 // Fix formatting
        [In] [MarshalUsing(typeof(ArrayMarshaller<byte, byte>))] byte[] data,
#pragma warning restore IDE0055 // Fix formatting
        CULong dataSize
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadU8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadU8(IoStream src, out byte value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadU8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadS8(IoStream src, out sbyte value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadU16LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadU16LittleEndian(IoStream src, out ushort value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadS16LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadS16LittleEndian(IoStream src, out short value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadU16BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadU16BigEndian(IoStream src, out ushort value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadS16BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadS16BigEndian(IoStream src, out short value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadU32LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadU32LittleEndian(IoStream src, out uint value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadS32LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadS32LittleEndian(IoStream src, out int value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadU32BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadU32BigEndian(IoStream src, out uint value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadS32BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadS32BigEndian(IoStream src, out int value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadU64LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadU64LittleEndian(IoStream src, out ulong value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadS64LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadS64LittleEndian(IoStream src, out long value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadU64BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadU64BigEndian(IoStream src, out ulong value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ReadS64BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ReadS64BigEndian(IoStream src, out long value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteU8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteU8(IoStream dst, byte value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteS8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteS8(IoStream dst, sbyte value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteU16LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteU16LittleEndian(IoStream dst, ushort value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteS16LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteS16LittleEndian(IoStream dst, short value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteU16BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteU16BigEndian(IoStream dst, ushort value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteS16BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteS16BigEndian(IoStream dst, short value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteU32LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteU32LittleEndian(IoStream dst, uint value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteS32LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteS32LittleEndian(IoStream dst, int value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteU32BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteU32BigEndian(IoStream dst, uint value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteS32BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteS32BigEndian(IoStream dst, int value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteU64LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteU64LittleEndian(IoStream dst, ulong value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteS64LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteS64LittleEndian(IoStream dst, long value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteU64BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteU64BigEndian(IoStream dst, ulong value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WriteS64BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool WriteS64BigEndian(IoStream dst, long value);

    public enum IoWhence
    {
        Set,
        Cur,
        End,
    }

    [NativeMarshalling(typeof(SafeHandleMarshaller<IoStream>))]
    public sealed class IoStream : SafeHandle
    {
        public override bool IsInvalid => handle == nint.Zero;

        public IoStream()
            : base(invalidHandleValue: nint.Zero, ownsHandle: true) => SetHandle(nint.Zero);

        protected override bool ReleaseHandle() => CloseIo(this);
    }
}
