// -----------------------------------------------------------------------
// <copyright file="SDL_properties.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.CustomMarshallers;
using Vmr.Sdl.Properties;

namespace Vmr.Sdl.NativeImports;

internal static partial class NativeSdl
{
    public static GCHandle EnumeratePropertiesCallbackHandle { get; private set; }

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetGlobalProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial PropertiesId GetGlobalProperties();

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_CreateProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial PropertiesId CreateProperties();

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_CopyProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool CopyProperties(PropertiesId source, PropertiesId destination);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_LockProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool LockProperties(PropertiesId properties);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_UnlockProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnlockProperties(PropertiesId properties);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetPointerProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetPointerProperty(PropertiesId properties, string name, nint value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetStringProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetStringProperty(PropertiesId properties, string name, string? value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetNumberProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetNumberProperty(PropertiesId properties, string name, long value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetFloatProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetFloatProperty(PropertiesId properties, string name, float value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetBooleanProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetBooleanProperty(
        PropertiesId properties,
        string name,
        [MarshalAs(UnmanagedType.I4)] bool value
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_HasProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool HasProperty(PropertiesId properties, string name);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetPropertyType", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial PropertyType GetPropertyType(PropertiesId properties, string name);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetPointerProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetPointerProperty(PropertiesId properties, string name, nint defaultValue);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetStringProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(SdlOwnedUtf8StringMarshaller))]
    public static partial string? GetStringProperty(PropertiesId properties, string name, string? defaultValue);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetNumberProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long GetNumberProperty(PropertiesId properties, string name, long defaultValue);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetFloatProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetFloatProperty(PropertiesId properties, string name, float defaultValue);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetBooleanProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetBooleanProperty(
        PropertiesId properties,
        string name,
        [MarshalAs(UnmanagedType.I4)] bool defaultValue
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ClearProperty", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ClearProperty(PropertiesId properties, string name);

    public static unsafe bool EnumerateProperties(PropertiesId props, PropertiesGroup.EnumerateProperties callback)
    {
        if (EnumeratePropertiesCallbackHandle.IsAllocated)
        {
            EnumeratePropertiesCallbackHandle.Free();
        }

        EnumeratePropertiesCallbackHandle = GCHandle.Alloc(callback);
        return EnumeratePropertiesUnsafe(
            props,
            EnumeratePropertiesCallbackPtr,
            GCHandle.ToIntPtr(EnumeratePropertiesCallbackHandle)
        );
    }

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_DestroyProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyProperties(PropertiesId properties);

    // void (SDLCALL *SDL_EnumeratePropertiesCallback)(void* userdata, SDL_PropertiesID props, const char* name);
    [SuppressMessage(
        "Spacing Rules",
        "SA1023:DereferenceAndAccessOfMustBeSpacedCorrectly",
        Justification = "This is a pointer to a delegate."
    )]
    private static readonly unsafe delegate* unmanaged[Cdecl]<
        nint,
        PropertiesId,
        byte*,
        void> EnumeratePropertiesCallbackPtr = &EnumeratePropertiesCallbackImpl;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void EnumeratePropertiesCallbackImpl(nint userdata, PropertiesId props, byte* name)
    {
        if (GCHandle.FromIntPtr(userdata).Target is not PropertiesGroup.EnumerateProperties enumerate)
        {
            return;
        }

        using PropertiesGroup group = new(props, isGlobal: false);
        var nameSafe =
            Utf8StringMarshaller.ConvertToManaged(name)
            ?? throw new InvalidOperationException(
                "Attempted to access a non-existent property name during property enumeration."
            );

        enumerate(group, nameSafe);
    }

    [SuppressMessage(
        "Spacing Rules",
        "SA1023:DereferenceAndAccessOfMustBeSpacedCorrectly",
        Justification = "This is a pointer to a delegate."
    )]
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_EnumerateProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    private static unsafe partial bool EnumeratePropertiesUnsafe(
        PropertiesId properties,
        delegate* unmanaged[Cdecl]<nint, PropertiesId, byte*, void> callback,
        nint userdata
    );

    public record struct PropertiesId(uint Value)
    {
        public static PropertiesId Invalid => new(0);
    }
}
