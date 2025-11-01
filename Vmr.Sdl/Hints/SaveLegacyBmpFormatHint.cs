// -----------------------------------------------------------------------
// <copyright file="SaveLegacyBmpFormatHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>Prevent SDL from using version 4 of the bitmap header when saving BMPs.</summary>
/// <remarks>
/// <para>The bitmap header version 4 is required for proper alpha channel support and SDL will use it when required. Should this not be desired, this hint can force the use of the 40 byte header version which is supported everywhere.</para>
/// <para>You can set this hint through the environment variable "SDL_BMP_SAVE_LEGACY_FORMAT" with the values "1" to enable or "0" to disable.</para>
/// </remarks>
public class SaveLegacyBmpFormatHint : HintBase
{
    private const string Hint = "SDL_BMP_SAVE_LEGACY_FORMAT";

    /// <summary>Gets or sets a value indicating whether the hint is enabled or not.</summary>
    /// <remarks>
    /// The variable can be set to the following values:
    /// <list type="bullet">
    /// <item><see langword="true"/>: Surfaces with a colorkey or an alpha channel are saved to a 32-bit BMP file without an alpha mask. The alpha channel data will be in the file, but applications are going to ignore it.</item>
    /// <item><see langword="false"/> (Default): Surfaces with a colorkey or an alpha channel are saved to a 32-bit BMP file with an alpha mask. SDL will use the bitmap header version 4 and set the alpha mask accordingly.</item>
    /// </list>
    /// This hint can be set anytime.
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
