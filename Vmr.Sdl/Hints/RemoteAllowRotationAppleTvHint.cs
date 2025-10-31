// -----------------------------------------------------------------------
// <copyright file="RemoteAllowRotationAppleTvHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>A variable controlling whether the Apple TV remote's joystick axes will automatically match the rotation of the remote.</summary>
/// <remarks>You can set this hint through the environment variable "SDL_APPLE_TV_REMOTE_ALLOW_ROTATION" with the values "1" to enable or "0" to disable.</remarks>
public sealed class RemoteAllowRotationAppleTvHint : HintBase
{
    private const string Hint = "SDL_APPLE_TV_REMOTE_ALLOW_ROTATION";

    /// <summary>Gets or sets a value indicating whether the hint is enabled or not.</summary>
    /// <remarks>
    /// The variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see langword="true"/> (Default): Remote orientation does not affect joystick axes.</item>
    /// <item><see langword="false"/>: Joystick axes are based on the orientation of the remote.</item>
    /// </list>
    /// This hint can be set any time.
    /// </remarks>
    public static bool Value
    {
        get => NativeSdl.GetHintBoolean(Hint, defaultValue: true);
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
