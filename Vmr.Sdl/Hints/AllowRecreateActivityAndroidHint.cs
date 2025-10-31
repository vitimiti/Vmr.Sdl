// -----------------------------------------------------------------------
// <copyright file="AllowRecreateActivityAndroidHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>A variable to control whether the SDL activity is allowed to be re-created.</summary>
/// <remarks>
/// <para>If this hint is true, the activity can be recreated on demand by the OS, and Java static data and C++ static data remain with their current values. If this hint is false, then SDL will call exit() when you return from your main function and the application will be terminated and then started fresh each time.</para>
/// <para>You can set this hint through the environment variable "SDL_ANDROID_ALLOW_RECREATE_ACTIVITY" with the values "1" to enable or "0" to disable.</para>
/// </remarks>
public class AllowRecreateActivityAndroidHint : HintBase
{
    private const string Hint = "SDL_ANDROID_ALLOW_RECREATE_ACTIVITY";

    /// <summary>Gets or sets a value indicating whether the hint is enabled or not.</summary>
    /// <remarks>
    /// The variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see langword="true"/> (Default): The application starts fresh at each launch.</item>
    /// <item><see langword="false"/>: The application activity can be recreated by the OS.</item>
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
