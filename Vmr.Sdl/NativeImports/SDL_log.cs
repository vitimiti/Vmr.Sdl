// -----------------------------------------------------------------------
// <copyright file="SDL_log.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Extensions.Logging;
using Vmr.Sdl.CustomMarshallers;
using Vmr.Sdl.Logging;

namespace Vmr.Sdl.NativeImports;

internal static partial class NativeSdl
{
    public static GCHandle LogOutputFunctionHandle { get; private set; }

    public static LogLevel LogPriorityToLogLevel(LogPriority priority) =>
        priority switch
        {
            LogPriority.Invalid => LogLevel.None,
            LogPriority.Trace => LogLevel.Trace,
            LogPriority.Verbose => LogLevel.Trace,
            LogPriority.Debug => LogLevel.Debug,
            LogPriority.Info => LogLevel.Information,
            LogPriority.Warn => LogLevel.Warning,
            LogPriority.Error => LogLevel.Error,
            LogPriority.Critical => LogLevel.Critical,
            _ => throw new ArgumentOutOfRangeException(nameof(priority), priority, null),
        };

    public static LogPriority LogLevelToLogPriority(LogLevel level) =>
        level switch
        {
            LogLevel.Trace => LogPriority.Trace,
            LogLevel.Debug => LogPriority.Debug,
            LogLevel.Information => LogPriority.Info,
            LogLevel.Warning => LogPriority.Warn,
            LogLevel.Error => LogPriority.Error,
            LogLevel.Critical => LogPriority.Critical,
            LogLevel.None => LogPriority.Invalid,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
        };

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetLogPriority")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetLogPriority(int category, LogPriority priority);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_LogTrace",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(StdFmtUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogTrace(int category, string? message);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_LogDebug",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(StdFmtUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogDebug(int category, string? message);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_LogInfo",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(StdFmtUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogInfo(int category, string? message);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_LogWarn",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(StdFmtUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogWarn(int category, string? message);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_LogError",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(StdFmtUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogError(int category, string? message);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_LogCritical",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(StdFmtUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogCritical(int category, string? message);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        LibraryName,
        EntryPoint = "SDL_LogMessage",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(StdFmtUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogMessage(int category, LogPriority priority, string? message);

    public static unsafe void SetLogOutputFunction(SdlLogOutputFunction callback)
    {
        if (LogOutputFunctionHandle.IsAllocated)
        {
            LogOutputFunctionHandle.Free();
        }

        LogOutputFunctionHandle = GCHandle.Alloc(callback);
        SetLogOutputFunctionUnsafe(LogOutputFunctionPtr, GCHandle.ToIntPtr(LogOutputFunctionHandle));
    }

    // void (SDLCALL *SDL_LogOutputFunction)(void *userdata, int category, SDL_LogPriority priority, const char *message);
    [SuppressMessage(
        "Spacing Rules",
        "SA1023:DereferenceAndAccessOfMustBeSpacedCorrectly",
        Justification = "This is a pointer to a delegate."
    )]
    private static readonly unsafe delegate* unmanaged[Cdecl]<
        nint,
        int,
        LogPriority,
        byte*,
        void> LogOutputFunctionPtr = &LogOutputFunctionImpl;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void LogOutputFunctionImpl(nint userdata, int category, LogPriority priority, byte* message)
    {
        if (GCHandle.FromIntPtr(userdata).Target is not SdlLogOutputFunction callback)
        {
            return;
        }

        callback(
            LogPriorityToLogLevel(priority),
            (LogCategory)category,
            Utf8StringMarshaller.ConvertToManaged(message) ?? string.Empty
        );
    }

    [SuppressMessage(
        "Spacing Rules",
        "SA1023:DereferenceAndAccessOfMustBeSpacedCorrectly",
        Justification = "This is a pointer to a delegate."
    )]
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(LibraryName, EntryPoint = "SDL_SetLogOutputFunction")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial void SetLogOutputFunctionUnsafe(
        delegate* unmanaged[Cdecl]<nint, int, LogPriority, byte*, void> callback,
        nint userdata
    );

    public enum LogPriority
    {
        Invalid,
        Trace,
        Verbose,
        Debug,
        Info,
        Warn,
        Error,
        Critical,
    }
}
