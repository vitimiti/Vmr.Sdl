// -----------------------------------------------------------------------
// <copyright file="SDL_init.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.CustomMarshallers;

namespace Vmr.Sdl.NativeImports;

internal static partial class NativeSdl
{
    public const string PropAppMetadataNameString = "SDL.app.metadata.name";
    public const string PropAppMetadataVersionString = "SDL.app.metadata.version";
    public const string PropAppMetadataIdentifierString = "SDL.app.metadata.identifier";
    public const string PropAppMetadataCreatorString = "SDL.app.metadata.creator";
    public const string PropAppMetadataCopyrightString = "SDL.app.metadata.copyright";
    public const string PropAppMetadataUrlString = "SDL.app.metadata.url";
    public const string PropAppMetadataTypeString = "SDL.app.metadata.type";

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_InitSubSystem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool InitSubSystem(InitFlags flags);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_QuitSubSystem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void QuitSubSystem(InitFlags flags);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_WasInit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial InitFlags WasInit(InitFlags flags);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_Quit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Quit();

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetAppMetadata", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetAppMetadata(string? appName, string? appVersion, string? appIdentifier);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetAppMetadataProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetAppMetadataProperty(string name, string? value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetAppMetadataProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(SdlOwnedUtf8StringMarshaller))]
    public static partial string? GetAppMetadataProperty(string name);

    public record struct InitFlags(uint Value)
    {
        public static InitFlags Audio => new(0x00000010U);

        public static InitFlags Video => new(0x00000020U);

        public static InitFlags Joystick => new(0x00000200U);

        public static InitFlags Haptic => new(0x00001000U);

        public static InitFlags Gamepad => new(0x00002000U);

        public static InitFlags Events => new(0x00004000U);

        public static InitFlags Sensor => new(0x00008000U);

        public static InitFlags Camera => new(0x00010000U);
    }
}
