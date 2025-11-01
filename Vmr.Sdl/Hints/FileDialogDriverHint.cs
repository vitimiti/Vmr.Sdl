// -----------------------------------------------------------------------
// <copyright file="FileDialogDriverHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Subsystems;

namespace Vmr.Sdl.Hints;

/// <summary>A variable that specifies a dialog backend to use.</summary>
/// <remarks>
/// <para>By default, SDL will try all available dialog backends in a reasonable order until it finds one that can work, but this hint allows the app or user to force a specific target.</para>
/// <para>If the specified target does not exist or is not available, the dialog-related function calls will fail.</para>
/// <para>This hint currently only applies to platforms using the generic "Unix" dialog implementation, but may be extended to more platforms in the future. Note that some Unix and Unix-like platforms have their own implementation, such as macOS and Haiku.</para>
/// <para>You can set this hint through the environment variable "SDL_FILE_DIALOG_DRIVER".</para>
/// </remarks>
public class FileDialogDriverHint : HintBase
{
    private const string Hint = "SDL_FILE_DIALOG_DRIVER";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>
    /// This variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see langword="null"/>(Default, all platforms): Select automatically.</item>
    /// <item><see cref="FileDialogDriver.Portal"/>: Use XDG Portals through DBus (Unix only.)</item>
    /// <item><see cref="FileDialogDriver.Zenity"/>: Use the Zenity program (Unix only.)</item>
    /// </list>
    /// More options may be added in the future. This hint can be set anytime.
    /// </remarks>
    public static FileDialogDriver Value
    {
        get =>
            (NativeSdl.GetHint(Hint) ?? "S16") switch
            {
                "portal" => FileDialogDriver.Portal,
                "zenity" => FileDialogDriver.Zenity,
                _ => FileDialogDriver.Default,
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
                FileDialogDriver.Portal => "portal",
                FileDialogDriver.Zenity => "zenity",
                _ => null,
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
