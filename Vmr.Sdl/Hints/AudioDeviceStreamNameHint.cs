// -----------------------------------------------------------------------
// <copyright file="AudioDeviceStreamNameHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

/// <summary>Specify an audio stream name for an audio device.</summary>
/// <remarks>
/// <para>Some audio backends (such as PulseAudio) allow you to describe your audio stream. Among other things, this description might show up in a system control panel that lets the user adjust the volume on specific audio streams instead of using one giant master volume slider.</para>
/// <para>This hint lets you transmit that information to the OS. The contents of this hint are used while opening an audio device. You should use a string that describes your what your program is playing ("audio stream" is probably sufficient in many cases, but this could be useful for something like "team chat" if you have a headset playing VoIP audio separately.)</para>
/// <para>You can set this hint through the environment variable "SDL_AUDIO_DEVICE_STREAM_NAME".</para>
/// </remarks>
public class AudioDeviceStreamNameHint : HintBase
{
    private const string Hint = "SDL_AUDIO_DEVICE_STREAM_NAME";

    // TODO: Change SDL_AudioStream to our own object docs.

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>
    /// <para>Setting this to <see cref="string.Empty"/> or leaving it unset will have SDL use a reasonable default: "audio stream" or something similar.</para>
    /// <para>Note that while this talks about audio streams, this is an OS-level concept, so it applies to a physical audio device in this case, and not an SDL_AudioStream, nor an SDL logical audio device.</para>
    /// <para>This hint should be set before an audio device is opened (<see cref="AudioSubsystem"/>.)</para>
    /// </remarks>
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
