// -----------------------------------------------------------------------
// <copyright file="EventPump.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Subsystems;

/// <summary>Represents the events subsystem of an SDL application, enabling initialization and management of events-related functionality.</summary>
/// <remarks>This subsystem must be initialized before usage. Initialization is performed when the <see cref="EventPump"/> is accessed through the <see cref="Application"/> class. The subsystem ensures proper setup of SDL's events functionality. Upon disposal, it releases resources associated with the events subsystem. Only ONE event pump will be initialized at any time.</remarks>
public sealed class EventPump : IDisposable
{
    internal EventPump()
    {
        if (
            (NativeSdl.WasInit(NativeSdl.InitFlags.Events).Value & NativeSdl.InitFlags.Events.Value)
            == NativeSdl.InitFlags.Events.Value
        )
        {
            return;
        }

        if (!NativeSdl.InitSubSystem(NativeSdl.InitFlags.Events))
        {
            throw new InvalidOperationException(
                $"Unable to initialize the {nameof(EventPump)} ({NativeSdl.GetError()}.)"
            );
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="EventPump"/> class.</summary>
    ~EventPump() => ReleaseUnmanagedResources();

    private static void ReleaseUnmanagedResources() => NativeSdl.QuitSubSystem(NativeSdl.InitFlags.Events);
}
