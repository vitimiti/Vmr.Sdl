// -----------------------------------------------------------------------
// <copyright file="CameraSubsystem.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Subsystems;

/// <summary>Represents the camera subsystem of an SDL application, enabling initialization and management of camera-related functionality.</summary>
/// <remarks>This subsystem must be initialized before usage. Initialization is performed when the <see cref="CameraSubsystem"/> is accessed through the <see cref="Application"/> class. The subsystem ensures proper setup of SDL's camera functionality. Upon disposal, it releases resources associated with the camera subsystem.</remarks>
public sealed class CameraSubsystem : IDisposable
{
    internal CameraSubsystem()
    {
        if (!NativeSdl.InitSubSystem(NativeSdl.InitFlags.Camera))
        {
            throw new InvalidOperationException(
                $"Unable to initialize the {nameof(CameraSubsystem)} ({NativeSdl.GetError()}.)"
            );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="CameraSubsystem"/> class.</summary>
    ~CameraSubsystem() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources() => NativeSdl.QuitSubSystem(NativeSdl.InitFlags.Camera);
}
