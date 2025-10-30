// -----------------------------------------------------------------------
// <copyright file="SdlApplication.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl;

/// <summary>Represents the main application class for initializing and managing various SDL subsystems. This class provides access to multiple subsystems and handles their lifecycle.</summary>
/// <remarks>SdlApplication serves as an entry point for accessing and working with SDL subsystems, such as audio, video, input devices, and sensors. The class ensures proper initialization and disposal of subsystems, and it disposes of unmanaged resources associated with SDL upon completion.</remarks>
public class SdlApplication : IDisposable
{
    /// <summary>Gets access to the SDL event processing functionality through an instance of <see cref="EventPump"/>.</summary>
    /// <remarks>The EventPump property creates and returns an instance of the <see cref="EventPump"/> class, which serves as a mechanism for polling and processing events from the SDL event queue. Event handling is essential for interacting with user inputs and managing application-level events within an SDL-based environment.</remarks>
    public EventPump EventPump
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return new EventPump();
        }
    }

    /// <summary>Gets access to the SDL audio subsystem through an instance of <see cref="AudioSubsystem"/>.</summary>
    /// <remarks>The AudioSubsystem property initializes and provides access to SDL's audio functionality, enabling sound playback and management. This subsystem is intended for handling all audio-related operations in the SDL environment and ensures proper resource initialization and disposal.</remarks>
    public AudioSubsystem AudioSubsystem
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return new AudioSubsystem();
        }
    }

    /// <summary>Gets access to the SDL camera subsystem functionality through an instance of <see cref="CameraSubsystem"/>.</summary>
    /// <remarks>The CameraSubsystem property creates and returns an instance of the <see cref="CameraSubsystem"/> class, which allows for initializing and managing SDL's camera-related functionality. Proper initialization of the camera subsystem is required before its use, and it is managed through the lifecycle of the <see cref="SdlApplication"/> instance.</remarks>
    public CameraSubsystem CameraSubsystem
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return new CameraSubsystem();
        }
    }

    /// <summary>Gets access to the gamepad subsystem through an instance of <see cref="GamepadSubsystem"/>.</summary>
    /// <remarks>The GamepadSubsystem property creates and returns an instance of the <see cref="GamepadSubsystem"/> class, which enables interaction with gamepad devices and SDL's gamepad-related functionality. Proper lifecycle management, including initialization and disposal of resources, is handled within the subsystem.</remarks>
    public GamepadSubsystem GamepadSubsystem
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return new GamepadSubsystem();
        }
    }

    /// <summary>Gets access to the SDL haptic subsystem functionality through an instance of <see cref="HapticSubsystem"/>.</summary>
    /// <remarks>The HapticSubsystem property provides the ability to initialize and manage haptic feedback functionality within an SDL application. It ensures proper setup and disposal of resources related to haptic devices and operations. This subsystem must be accessed via the <see cref="SdlApplication"/> class, and it facilitates interaction with the haptic features provided by SDL.</remarks>
    public HapticSubsystem HapticSubsystem
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return new HapticSubsystem();
        }
    }

    /// <summary>Gets access to the SDL joystick subsystem through an instance of <see cref="JoystickSubsystem"/>.</summary>
    /// <remarks>The JoystickSubsystem property initializes and provides an instance of the <see cref="JoystickSubsystem"/> class, enabling functionality related to joystick devices. It ensures proper setup and management of joystick input within an SDL-based application. The subsystem is initialized upon property access and is disposed automatically when no longer needed.</remarks>
    public JoystickSubsystem JoystickSubsystem
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return new JoystickSubsystem();
        }
    }

    /// <summary>Gets access to the SDL sensor-related functionality through an instance of <see cref="SensorSubsystem"/>.</summary>
    /// <remarks>The SensorSubsystem property initializes and returns an instance of the <see cref="SensorSubsystem"/> class, which facilitates the management and interaction with sensor components within the SDL environment. Proper initialization and disposal of the subsystem are automatically handled during its lifecycle.</remarks>
    public SensorSubsystem SensorSubsystem
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return new SensorSubsystem();
        }
    }

    /// <summary>Gets access to the SDL video subsystem through an instance of <see cref="VideoSubsystem"/>.</summary>
    /// <remarks>The VideoSubsystem property initializes and returns an instance of the <see cref="VideoSubsystem"/> class, providing functionality for managing video-related operations in SDL. This includes handling rendering, window management, and other video output capabilities. Proper initialization of the video subsystem is required to ensure its availability for use within an SDL-based application.</remarks>
    public VideoSubsystem VideoSubsystem
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return new VideoSubsystem();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="SdlApplication"/> class.</summary>
    ~SdlApplication() => Dispose(false);

    /// <summary>Releases all resources used by the <see cref="SdlApplication"/> instance and its subsystems.</summary>
    /// <param name="disposing">A boolean value indicating whether to release both managed and unmanaged resources (<see langword="true"/>) or only managed resources (<see langword="false"/>).</param>
    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            // Nothing to dispose of.
        }
    }

    private static void ReleaseUnmanagedResources() => NativeSdl.Quit();
}
