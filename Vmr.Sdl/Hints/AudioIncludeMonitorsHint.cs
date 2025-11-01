// -----------------------------------------------------------------------
// <copyright file="AudioIncludeMonitorsHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>A variable that causes SDL to not ignore audio "monitors".</summary>
/// <remarks>
/// <para>This is currently only used by the PulseAudio driver.</para>
/// <para>By default, SDL ignores audio devices that aren't associated with physical hardware. Changing this hint to "1" will expose anything SDL sees that appears to be an audio source or sink. This will add "devices" to the list that the user probably doesn't want or need, but it can be useful in scenarios where you want to hook up SDL to some sort of virtual device, etc.</para>
/// <para>You can set this hint through the environment variable "SDL_AUDIO_INCLUDE_MONITORS" with the values "1" to enable or "0" to disable.</para>
/// </remarks>
public class AudioIncludeMonitorsHint : HintBase
{
    private const string Hint = "SDL_AUDIO_INCLUDE_MONITORS";

    /// <summary>Gets or sets a value indicating whether the hint is enabled or not.</summary>
    /// <remarks>
    /// The variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see langword="true"/>: Audio monitor devices will show up in the device list.</item>
    /// <item><see langword="false"/> (Default): Audio monitor devices will be ignored.</item>
    /// </list>
    /// This hint should be set before SDL is initialized (see <see cref="Application"/>.)
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
