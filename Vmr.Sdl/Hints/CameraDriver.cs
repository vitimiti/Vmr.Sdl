// -----------------------------------------------------------------------
// <copyright file="CameraDriver.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>A variable that decides what camera backend to use.</summary>
/// <remarks>
/// <para>By default, SDL will try all available camera backends in a reasonable order until it finds one that can work, but this hint allows the app or user to force a specific target, such as "directshow" if, say, you are on Windows Media Foundations but want to try DirectShow instead.</para>
/// <para>You can set this hint through the environment variable "SDL_CAMERA_DRIVER".</para>
/// </remarks>
public class CameraDriver : HintBase
{
    private const string Hint = "SDL_CAMERA_DRIVER";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>The default value is unset, in which case SDL will try to figure out the best camera backend on your behalf. This hint needs to be set before <see cref="Application"/> is initialized to be useful.</remarks>
    public static string? Value
    {
        get => NativeSdl.GetHint(Hint);
        set => SetHintValue(Hint, value);
    }

    /// <summary>Sets the hint with the given priority.</summary>
    /// <param name="priority">The priority of the hint.</param>
    /// <param name="value">The value of the hint.</param>
    public static void Set(HintPriority priority, bool value) => SetWithPriority(Hint, value ? "1" : "0", priority);

    /// <summary>Adds a callback to the hint.</summary>
    /// <param name="callback">The callback to add.</param>
    /// <remarks>For safety reasons, only ONE (1) callback will be active at a time, the last one. Adding new callbacks while not removing the old one, will result in the automatic removal of the previous one.</remarks>
    /// <seealso cref="RemoveCallback"/>
    public static void AddCallback(HintUpdated callback) => AddCallback(Hint, callback);

    /// <summary>Removes the callback from the hint.</summary>
    /// <seealso cref="AddCallback"/>
    public static void RemoveCallback() => RemoveCallback(Hint);
}
