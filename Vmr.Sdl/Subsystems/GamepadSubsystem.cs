// -----------------------------------------------------------------------
// <copyright file="GamepadSubsystem.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Subsystems;

/// <summary>Represents the gamepad subsystem of an SDL application, enabling initialization and management of gamepad-related functionality.</summary>
/// <remarks>This subsystem must be initialized before usage. Initialization is performed when the <see cref="GamepadSubsystem"/> is accessed through the <see cref="Application"/> class. The subsystem ensures proper setup of SDL's gamepad functionality. Upon disposal, it releases resources associated with the gamepad subsystem.</remarks>
public sealed class GamepadSubsystem : IDisposable
{
    internal GamepadSubsystem()
    {
        if (!NativeSdl.InitSubSystem(NativeSdl.InitFlags.Gamepad))
        {
            throw new InvalidOperationException(
                $"Unable to initialize the {nameof(GamepadSubsystem)} ({NativeSdl.GetError()}.)"
            );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="GamepadSubsystem"/> class.</summary>
    ~GamepadSubsystem() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources() => NativeSdl.QuitSubSystem(NativeSdl.InitFlags.Gamepad);
}
