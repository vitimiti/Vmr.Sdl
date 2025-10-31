// -----------------------------------------------------------------------
// <copyright file="SensorSubsystem.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Subsystems;

/// <summary>Represents the sensor subsystem of an SDL application, enabling initialization and management of sensor-related functionality.</summary>
/// <remarks>This subsystem must be initialized before usage. Initialization is performed when the <see cref="SensorSubsystem"/> is accessed through the <see cref="Application"/> class. The subsystem ensures proper setup of SDL's sensor functionality. Upon disposal, it releases resources associated with the sensor subsystem.</remarks>
public sealed class SensorSubsystem : IDisposable
{
    internal SensorSubsystem()
    {
        if (!NativeSdl.InitSubSystem(NativeSdl.InitFlags.Sensor))
        {
            throw new InvalidOperationException(
                $"Unable to initialize the {nameof(SensorSubsystem)} ({NativeSdl.GetError()}.)"
            );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="SensorSubsystem"/> class.</summary>
    ~SensorSubsystem() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources() => NativeSdl.QuitSubSystem(NativeSdl.InitFlags.Sensor);
}
