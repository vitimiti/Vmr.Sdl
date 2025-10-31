// -----------------------------------------------------------------------
// <copyright file="AudioChannelsHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

/// <summary>A variable controlling the default audio channel count.</summary>
/// <remarks>
/// <para>If the application doesn't specify the audio channel count when opening the device, this hint can be used to specify a default channel count that will be used. This defaults to "1" for recording and "2" for playback devices.</para>
/// <para>You can set this hint through the environment variable "SDL_AUDIO_CHANNELS".</para>
/// </remarks>
public class AudioChannelsHint : HintBase
{
    private const string Hint = "SDL_AUDIO_CHANNELS";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>This hint should be set before SDL is initialized (see <see cref="AudioSubsystem"/>.)</remarks>
    public static int Value
    {
        get => int.Parse(NativeSdl.GetHint(Hint) ?? "0", NumberStyles.Integer, CultureInfo.InvariantCulture);
        set => SetHintValue(Hint, value.ToString(CultureInfo.InvariantCulture));
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
