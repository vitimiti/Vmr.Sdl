// -----------------------------------------------------------------------
// <copyright file="SDL_rect.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Vmr.Sdl.Drawing;

namespace Vmr.Sdl.NativeImports;

internal static partial class NativeSdl
{
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_HasRectIntersection")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool HasRectIntersection(Rectangle a, Rectangle b);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetRectIntersection")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetRectIntersection(Rectangle a, Rectangle b, out Rectangle result);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetRectUnion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetRectUnion(Rectangle a, Rectangle b, out Rectangle result);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetRectEnclosingPoints")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetRectEnclosingPoints(
        [In]
        [MarshalUsing(typeof(ArrayMarshaller<Sdl.Drawing.Point, Point>), CountElementName = nameof(count))]
            Sdl.Drawing.Point[] points,
        int count,
        Rectangle? clip,
        out Rectangle result
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetRectAndLineIntersection")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetRectAndLineIntersection(
        Rectangle rect,
        ref int x1,
        ref int y1,
        ref int x2,
        ref int y2
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_HasRectIntersectionFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool HasRectIntersectionFloat(FRectangle a, FRectangle b);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetRectIntersectionFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetRectIntersectionFloat(FRectangle a, FRectangle b, out FRectangle result);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetRectUnion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetRectUnionFloat(FRectangle a, FRectangle b, out FRectangle result);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetRectEnclosingPointsFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetRectEnclosingPointsFloat(
        [In]
        [MarshalUsing(typeof(ArrayMarshaller<Sdl.Drawing.FPoint, FPoint>), CountElementName = nameof(count))]
            Sdl.Drawing.FPoint[] points,
        int count,
        FRectangle? clip,
        out FRectangle result
    );

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_GetRectAndLineIntersectionFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool GetRectAndLineIntersectionFloat(
        FRectangle rect,
        ref float x1,
        ref float y1,
        ref float x2,
        ref float y2
    );

    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FPoint
    {
        public float X;
        public float Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int X;
        public int Y;
        public int W;
        public int H;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FRect
    {
        public float X;
        public float Y;
        public float W;
        public float H;
    }
}
