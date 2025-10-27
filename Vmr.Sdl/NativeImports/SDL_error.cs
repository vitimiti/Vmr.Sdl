// -----------------------------------------------------------------------
// <copyright file="SDL_error.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vmr.Sdl.CustomMarshallers;

namespace Vmr.Sdl.NativeImports;

internal static partial class NativeSdl
{
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_GetError",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(SdlOwnedUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial string GetError();
}
