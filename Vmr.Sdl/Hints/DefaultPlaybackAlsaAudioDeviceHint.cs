// -----------------------------------------------------------------------
// <copyright file="DefaultPlaybackAlsaAudioDeviceHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

/// <summary>Specify the default ALSA audio playback device name.</summary>
/// <remarks>
/// <para>This variable is a specific audio device to open for playback when the "default" audio device is used.</para>
/// <para>If this hint isn't set, SDL will check <see cref="DefaultAlsaAudioDeviceHint"/> before choosing a reasonable default.</para>
/// <para>You can set this hint through the environment variable "SDL_AUDIO_ALSA_DEFAULT_PLAYBACK_DEVICE".</para>
/// </remarks>
/// <seealso cref="DefaultAlsaAudioDeviceHint"/>
/// <seealso cref="DefaultRecordingAlsaAudioDeviceHint"/>
public class DefaultPlaybackAlsaAudioDeviceHint : HintBase
{
    private const string Hint = "SDL_AUDIO_ALSA_DEFAULT_PLAYBACK_DEVICE";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>This hint should be set before an audio device is opened (see <see cref="AudioSubsystem"/>.)</remarks>
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
