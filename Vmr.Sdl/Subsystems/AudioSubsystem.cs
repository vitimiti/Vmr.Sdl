// -----------------------------------------------------------------------
// <copyright file="AudioSubsystem.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Subsystems;

/// <summary>Represents the audio subsystem of an SDL application, enabling initialization and management of audio-related functionality.</summary>
/// <remarks>This subsystem must be initialized before usage. Initialization is performed when the <see cref="AudioSubsystem"/> is accessed through the <see cref="SdlApplication"/> class. The subsystem ensures proper setup of SDL's audio functionality. Upon disposal, it releases resources associated with the audio subsystem.</remarks>
public sealed class AudioSubsystem : IDisposable
{
    internal AudioSubsystem()
    {
        if (!NativeSdl.InitSubSystem(NativeSdl.InitFlags.Audio))
        {
            throw new InvalidOperationException(
                $"Unable to initialize the {nameof(AudioSubsystem)} ({NativeSdl.GetError()})."
            );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="AudioSubsystem"/> class.</summary>
    ~AudioSubsystem() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources() => NativeSdl.QuitSubSystem(NativeSdl.InitFlags.Audio);
}
