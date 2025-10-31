// -----------------------------------------------------------------------
// <copyright file="HapticSubsystem.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Subsystems;

/// <summary>Represents the haptic subsystem of an SDL application, enabling initialization and management of haptic-related functionality.</summary>
/// <remarks>This subsystem must be initialized before usage. Initialization is performed when the <see cref="HapticSubsystem"/> is accessed through the <see cref="Application"/> class. The subsystem ensures proper setup of SDL's haptic functionality. Upon disposal, it releases resources associated with the haptic subsystem.</remarks>
public sealed class HapticSubsystem : IDisposable
{
    internal HapticSubsystem()
    {
        if (!NativeSdl.InitSubSystem(NativeSdl.InitFlags.Haptic))
        {
            throw new InvalidOperationException(
                $"Unable to initialize the {nameof(HapticSubsystem)} ({NativeSdl.GetError()}.)"
            );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="HapticSubsystem"/> class.</summary>
    ~HapticSubsystem() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources() => NativeSdl.QuitSubSystem(NativeSdl.InitFlags.Haptic);
}
