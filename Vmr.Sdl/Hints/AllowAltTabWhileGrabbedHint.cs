// -----------------------------------------------------------------------
// <copyright file="AllowAltTabWhileGrabbedHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>Specify the behavior of Alt+Tab while the keyboard is grabbed.</summary>
/// <remarks>By default, SDL emulates Alt+Tab functionality while the keyboard is grabbed and your window is full-screen. This prevents the user from getting stuck in your application if you've enabled keyboard grab.</remarks>
public sealed class AllowAltTabWhileGrabbedHint : HintBase
{
    private const string Hint = "SDL_ALLOW_ALT_TAB_WHILE_GRABBED";

    /// <summary>Gets or sets a value indicating whether the hint is enabled or not.</summary>
    /// <remarks>
    /// The variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see langword="true"/>: SDL will not handle Alt+Tab. Your application is responsible for handling Alt+Tab while the keyboard is grabbed.</item>
    /// <item><see langword="false"/> (Default): SDL will minimize your window when Alt+Tab is pressed.</item>
    /// </list>
    /// This hint can be set any time.
    /// </remarks>
    public static bool Value
    {
        get => NativeSdl.GetHintBoolean(Hint, true);
        set => NativeSdl.SetHint(Hint, value ? "1" : "0");
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
