// -----------------------------------------------------------------------
// <copyright file="VideoSubsystem.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Subsystems;

/// <summary>Represents the video subsystem of an SDL application, enabling initialization and management of video-related functionality.</summary>
/// <remarks>This subsystem must be initialized before usage. Initialization is performed when the <see cref="VideoSubsystem"/> is accessed through the <see cref="SdlApplication"/> class. The subsystem ensures proper setup of SDL's video functionality. Upon disposal, it releases resources associated with the video subsystem.</remarks>
public sealed class VideoSubsystem : IDisposable
{
    internal VideoSubsystem()
    {
        if (!NativeSdl.InitSubSystem(NativeSdl.InitFlags.Video))
        {
            throw new InvalidOperationException(
                $"Unable to initialize the {nameof(VideoSubsystem)} ({NativeSdl.GetError()})."
            );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="VideoSubsystem"/> class.</summary>
    ~VideoSubsystem() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources() => NativeSdl.QuitSubSystem(NativeSdl.InitFlags.Video);
}
