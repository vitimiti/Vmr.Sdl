// -----------------------------------------------------------------------
// <copyright file="AudioDummyTimeScale.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

/// <summary>A variable controlling the audio rate when using the dummy audio driver.</summary>
/// <remarks>
/// <para>The dummy audio driver normally simulates real-time for the audio rate that was specified, but you can use this variable to adjust this rate higher or lower down to 0. The default value is "1.0".</para>
/// <para>You can set this hint through the environment variable "SDL_AUDIO_DUMMY_TIMESCALE".</para>
/// </remarks>
public class AudioDummyTimeScale : HintBase
{
    private const string Hint = "SDL_AUDIO_DUMMY_TIMESCALE";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>This hint should be set before SDL is initialized (see <see cref="AudioSubsystem"/>.)</remarks>
    public static float Value
    {
        get => float.Parse(NativeSdl.GetHint(Hint) ?? "0", NumberStyles.Integer, CultureInfo.InvariantCulture);
        set
        {
            var actualValue = value;
            if (value < 0F)
            {
                actualValue = 0F;
            }

            SetHintValue(Hint, actualValue.ToString(CultureInfo.InvariantCulture));
        }
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
