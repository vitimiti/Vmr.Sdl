// -----------------------------------------------------------------------
// <copyright file="AudioCategoryHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>A variable setting the app ID string.</summary>
/// <remarks>
/// <para>You can set this hint through the environment variable "SDL_AUDIO_CATEGORY".</para>
/// </remarks>
public class AudioCategoryHint : HintBase
{
    private const string Hint = "SDL_AUDIO_CATEGORY";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>
    /// <para>
    /// The variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see cref="AudioCategory.Ambient"/> (Default): Use the AVAudioSessionCategoryAmbient audio category, will be muted by the phone mute switch.</item>
    /// <item><see cref="AudioCategory.Playback"/>: Use the AVAudioSessionCategoryPlayback category.</item>
    /// </list>
    /// </para>
    /// <para>For more information, see Apple's documentation: <see href="https://developer.apple.com/library/content/documentation/Audio/Conceptual/AudioSessionProgrammingGuide/AudioSessionCategoriesandModes/AudioSessionCategoriesandModes.html"/>.</para>
    /// <para>This hint should be set before SDL is initialized (<see cref="Application"/>).</para>
    /// </remarks>
    public static AudioCategory Value
    {
        get =>
            (NativeSdl.GetHint(Hint) ?? "ambient") switch
            {
                "playback" => AudioCategory.Playback,
                _ => AudioCategory.Ambient,
            };
        [SuppressMessage(
            "Style",
            "IDE0072:Add missing cases",
            Justification = "Missing cases are already taken into account."
        )]
        set
        {
            var category = value switch
            {
                AudioCategory.Playback => "playback",
                _ => "ambient",
            };

            if (!NativeSdl.SetHint(Hint, category))
            {
                throw new InvalidOperationException(
                    $"Unable to set the hint \"{Hint}\" to {value} ({NativeSdl.GetError()}.)"
                );
            }
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
