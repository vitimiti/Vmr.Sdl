// -----------------------------------------------------------------------
// <copyright file="LowLatencyAudioAndroidHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

/// <summary>A variable to control whether low-latency audio should be enabled.</summary>
/// <remarks>
/// <para>Some devices have poor quality output when this is enabled, but this is usually an improvement in audio latency.</para>
/// <para>You can set this hint through the environment variable "SDL_ANDROID_LOW_LATENCY_AUDIO" with the values "1" to enable or "0" to disable.</para>
/// </remarks>
public class LowLatencyAudioAndroidHint : HintBase
{
    private const string Hint = "SDL_ANDROID_LOW_LATENCY_AUDIO";

    /// <summary>Gets or sets a value indicating whether the hint is enabled or not.</summary>
    /// <remarks>
    /// The variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see langword="true"/>: Low latency audio is not enabled.</item>
    /// <item><see langword="false"/> (Default): Low latency audio is enabled.</item>
    /// </list>
    /// This hint should be set before SDL audio is initialized (<see cref="AudioSubsystem"/>).
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
