// -----------------------------------------------------------------------
// <copyright file="JoystickSubsystem.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Subsystems;

/// <summary>Represents the joystick subsystem of an SDL application, enabling initialization and management of joystick-related functionality.</summary>
/// <remarks>This subsystem must be initialized before usage. Initialization is performed when the <see cref="JoystickSubsystem"/> is accessed through the <see cref="SdlApplication"/> class. The subsystem ensures proper setup of SDL's joystick functionality. Upon disposal, it releases resources associated with the joystick subsystem.</remarks>
public sealed class JoystickSubsystem : IDisposable
{
    internal JoystickSubsystem()
    {
        if (!NativeSdl.InitSubSystem(NativeSdl.InitFlags.Joystick))
        {
            throw new InvalidOperationException(
                $"Unable to initialize the {nameof(JoystickSubsystem)} ({NativeSdl.GetError()})."
            );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="JoystickSubsystem"/> class.</summary>
    ~JoystickSubsystem() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources() => NativeSdl.QuitSubSystem(NativeSdl.InitFlags.Joystick);
}
