// -----------------------------------------------------------------------
// <copyright file="AudioFormatHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

/// <summary>A variable controlling the default audio format.</summary>
/// <remarks>
/// <para>If the application doesn't specify the audio format when opening the device, this hint can be used to specify a default format that will be used.</para>
/// <para>You can set this hint through the environment variable "SDL_AUDIO_FORMAT".</para>
/// </remarks>
public class AudioFormatHint : HintBase
{
    private const string Hint = "SDL_AUDIO_FORMAT";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>
    /// This variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see cref="AudioFormat.U8"/>: Unsigned 8-bit audio.</item>
    /// <item><see cref="AudioFormat.S8"/>: Signed 8-bit audio.</item>
    /// <item><see cref="AudioFormat.S16LittleEndian"/>: Signed 16-bit little-endian audio.</item>
    /// <item><see cref="AudioFormat.S16BigEndian"/>: Signed 16-bit big-endian audio.</item>
    /// <item><see cref="AudioFormat.S16"/> (Default): Signed 16-bit native-endian audio.</item>
    /// <item><see cref="AudioFormat.S32LittleEndian"/>: Signed 32-bit little-endian audio.</item>
    /// <item><see cref="AudioFormat.S32BigEndian"/>: Signed 32-bit big-endian audio.</item>
    /// <item><see cref="AudioFormat.S32"/>: Signed 32-bit native-endian audio.</item>
    /// <item><see cref="AudioFormat.F32LittleEndian"/>: Floating point little-endian audio.</item>
    /// <item><see cref="AudioFormat.F32BigEndian"/>: Floating point big-endian audio.</item>
    /// <item><see cref="AudioFormat.F32"/>: Floating point native-endian audio.</item>
    /// </list>
    /// This hint should be set before SDL is initialized (see <see cref="AudioSubsystem"/>.)
    /// </remarks>
    public static AudioFormat Value
    {
        get =>
            (NativeSdl.GetHint(Hint) ?? "S16") switch
            {
                "U8" => AudioFormat.U8,
                "S8" => AudioFormat.S8,
                "S16LE" => AudioFormat.S16LittleEndian,
                "S16BE" => AudioFormat.S16BigEndian,
                "S32LE" => AudioFormat.S32LittleEndian,
                "S32BE" => AudioFormat.S32BigEndian,
                "S32" => AudioFormat.S32,
                "F32LE" => AudioFormat.F32LittleEndian,
                "F32BE" => AudioFormat.F32BigEndian,
                "F32" => AudioFormat.F32,
                _ => AudioFormat.S16,
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
                AudioFormat.U8 => "U8",
                AudioFormat.S8 => "S8",
                AudioFormat.S16LittleEndian => "S16LE",
                AudioFormat.S16BigEndian => "S16BE",
                AudioFormat.S32LittleEndian => "S32LE",
                AudioFormat.S32BigEndian => "S32BE",
                AudioFormat.S32 => "S32",
                AudioFormat.F32LittleEndian => "F32LE",
                AudioFormat.F32BigEndian => "F32BE",
                AudioFormat.F32 => "F32",
                _ => "S16",
            };

            SetHintValue(Hint, category);
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
