// -----------------------------------------------------------------------
// <copyright file="AppNameHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>A variable setting the application name.</summary>
/// <remarks>
/// <para>This hint lets you specify the application name sent to the OS when required. For example, this will often appear in volume control applets for audio streams, and in lists of applications which are inhibiting the screensaver. You should use a string that describes your program ("My Game 2: The Revenge".)</para>
/// <para>This will override <see cref="ApplicationMetadata.Name"/>, if set by the application.</para>
/// <para>This hint should be set before SDL is initialized.</para>
/// <para>You can set this hint through the environment variable "SDL_APP_NAME".</para>
/// </remarks>
public class AppNameHint : HintBase
{
    private const string Hint = "SDL_APP_NAME";

    /// <summary>Gets or sets the value of the hint.</summary>
    public static string? Value
    {
        get => NativeSdl.GetHint(Hint);
        set => NativeSdl.SetHint(Hint, value);
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
