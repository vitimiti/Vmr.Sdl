// -----------------------------------------------------------------------
// <copyright file="SDL_hints.cs" company="Vmr.Sdl">
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
using Vmr.Sdl.Hints;

namespace Vmr.Sdl.NativeImports;

internal static partial class NativeSdl
{
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetHintWithPriority", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetHintWithPriority(string name, string? value, HintPriority priority);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetHint", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool SetHint(string name, string? value);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ResetHint", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ResetHint(string name);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_ResetHints")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ResetHints();

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetHint", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(SdlOwnedUtf8StringMarshaller))]
    public static partial string? GetHint(string name);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetHintBoolean", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetHintBoolean(string name, [MarshalAs(UnmanagedType.I4)] bool defaultValue);

    [SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1023:DereferenceAndAccessOfMustBeSpacedCorrectly",
        Justification = "This is a pointer within a delegate argument list."
    )]
    public static readonly unsafe delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void> HintCallbackPtr =
        &HintCallbackImpl;

    [SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1023:DereferenceAndAccessOfMustBeSpacedCorrectly",
        Justification = "This is a pointer within a delegate argument list."
    )]
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_AddHintCallback", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static unsafe partial bool AddHintCallback(
        string name,
        delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void> callback,
        nint userdata
    );

    [SuppressMessage(
        "StyleCop.CSharp.SpacingRules",
        "SA1023:DereferenceAndAccessOfMustBeSpacedCorrectly",
        Justification = "This is a pointer within a delegate argument list."
    )]
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_RemoveHintCallback", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void RemoveHintCallback(
        string name,
        delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void> callback,
        nint userdata
    );

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void HintCallbackImpl(nint userdata, byte* name, byte* oldValue, byte* newValue)
    {
        if (GCHandle.FromIntPtr(userdata).Target is not HintUpdated callback)
        {
            return;
        }

        var nameStr = Utf8StringMarshaller.ConvertToManaged(name) ?? throw new ArgumentNullException(nameof(name));
        var oldValueStr = Utf8StringMarshaller.ConvertToManaged(oldValue);
        var newValueStr = Utf8StringMarshaller.ConvertToManaged(newValue);

        callback(nameStr, oldValueStr, newValueStr);
    }
}
