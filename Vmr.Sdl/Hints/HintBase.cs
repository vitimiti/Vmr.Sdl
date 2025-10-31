// -----------------------------------------------------------------------
// <copyright file="HintBase.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>Delegate for hint updates.</summary>
/// <param name="name">The name of the hint that was updated.</param>
/// <param name="oldValue">The old value of the hint.</param>
/// <param name="newValue">The new value of the hint.</param>
public delegate void HintUpdated(string name, string? oldValue, string? newValue);

/// <summary>Base class for all hint types.</summary>
public abstract class HintBase
{
    /// <summary>Initializes a new instance of the <see cref="HintBase"/> class.</summary>
    protected HintBase() { }

    private static GCHandle CallbackHandle { get; set; }

    /// <summary>Sets a hint with a priority.</summary>
    /// <param name="name">The name of the hint.</param>
    /// <param name="value">The value of the hint.</param>
    /// <param name="priority">The priority of the hint.</param>
    /// <exception cref="InvalidOperationException">Thrown when the hint failed to be set.</exception>
    protected static void SetWithPriority(string name, string value, HintPriority priority)
    {
        if (!NativeSdl.SetHintWithPriority(name, value, priority))
        {
            throw new InvalidOperationException(
                $"Unable to set the hint \"{name}\" to the value \"{value}\" with priority {priority} ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Adds a callback to the hint.</summary>
    /// <param name="name">The name of the hint.</param>
    /// <param name="callback">The callback to add.</param>
    /// <exception cref="InvalidOperationException">Thrown when the callback failed to be added.</exception>
    protected static void AddCallback(string name, HintUpdated callback)
    {
        if (CallbackHandle.IsAllocated)
        {
            RemoveCallback(name);
        }

        CallbackHandle = GCHandle.Alloc(callback);

        unsafe
        {
            if (!NativeSdl.AddHintCallback(name, NativeSdl.HintCallbackPtr, CallbackHandle.AddrOfPinnedObject()))
            {
                throw new InvalidOperationException(
                    $"Unable to add callback to hint \"{name}\" ({NativeSdl.GetError()})."
                );
            }
        }
    }

    /// <summary>Removes a callback from the hint.</summary>
    /// <param name="name">The name of the hint.</param>
    protected static void RemoveCallback(string name)
    {
        if (!CallbackHandle.IsAllocated)
        {
            return;
        }

        unsafe
        {
            NativeSdl.RemoveHintCallback(name, NativeSdl.HintCallbackPtr, CallbackHandle.AddrOfPinnedObject());
        }

        CallbackHandle.Free();
    }
}
