// -----------------------------------------------------------------------
// <copyright file="TrapBackButtonAndroidHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

// TODO: Change SDL_EVENT_KEY_DOWN to our object docs
// TODO: Change SDL_EVENT_KEY_UP to our object docs
// TODO: Change SDL_SCANCODE_AC_BACK to our object docs

/// <summary>A variable to control whether we trap the Android back button to handle it manually.</summary>
/// <remarks>
/// <para>This is necessary for the right mouse button to work on some Android devices, or to be able to trap the back button for use in your code reliably. If this hint is true, the back button will show up as an SDL_EVENT_KEY_DOWN / SDL_EVENT_KEY_UP pair with a keycode of SDL_SCANCODE_AC_BACK.</para>
/// <para>You can set this hint through the environment variable "SDL_ANDROID_TRAP_BACK_BUTTON" with the values "1" to enable or "0" to disable.</para>
/// </remarks>
public class TrapBackButtonAndroidHint : HintBase
{
    private const string Hint = "SDL_ANDROID_TRAP_BACK_BUTTON";

    /// <summary>Gets or sets a value indicating whether the hint is enabled or not.</summary>
    /// <remarks>
    /// The variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see langword="true"/>: Back button will be trapped, allowing you to handle the key press manually. (This will also let right mouse click work on systems where the right mouse button functions as back.)</item>
    /// <item><see langword="false"/> (Default): Back button will be handled as usual for the system.</item>
    /// </list>
    /// This hint can be set anytime.
    /// </remarks>
    public static bool Value
    {
        get => NativeSdl.GetHintBoolean(Hint, defaultValue: false);
        set => SetHintValue(Hint, value ? "1" : "0");
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
