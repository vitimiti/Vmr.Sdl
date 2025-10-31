// -----------------------------------------------------------------------
// <copyright file="AudioDeviceAppIconNameHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

/// <summary>Specify an application icon name for an audio device.</summary>
/// <remarks>
/// <para>Some audio backends (such as Pulseaudio and Pipewire) allow you to set an XDG icon name for your application. Among other things, this icon might show up in a system control panel that lets the user adjust the volume on specific audio streams instead of using one giant master volume slider. Note that this is unrelated to the icon used by the windowing system, which may be set with SDL_SetWindowIcon (or via desktop file on Wayland.)</para>
/// <para>You can set this hint through the environment variable "SDL_AUDIO_DEVICE_APP_ICON_NAME".</para>
/// </remarks>
public class AudioDeviceAppIconNameHint : HintBase
{
    private const string Hint = "SDL_AUDIO_DEVICE_APP_ICON_NAME";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>
    /// <para>Setting this to <see cref="string.Empty"/> or leaving it unset will have SDL use a reasonable default, "applications-games", which is likely to be installed. See <see href="https://specifications.freedesktop.org/icon-theme-spec/icon-theme-spec-latest.html"/> and <see href="https://specifications.freedesktop.org/icon-naming-spec/icon-naming-spec-latest.html"/> for the relevant XDG icon specs.</para>
    /// <para>This hint should be set before an audio device is opened (see <see cref="AudioSubsystem"/>.)</para>
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
