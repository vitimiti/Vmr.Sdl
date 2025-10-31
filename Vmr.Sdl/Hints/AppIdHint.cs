// -----------------------------------------------------------------------
// <copyright file="AppIdHint.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Hints;

/// <summary>A variable setting the app ID string.</summary>
/// <remarks>
/// <para>This string is used by desktop compositors to identify and group windows together, as well as match applications with associated desktop settings and icons.</para>
/// <para>This will override <see cref="ApplicationMetadata.Identifier"/>, if set by the application.</para>
/// <para>You can set this hint through the environment variable "SDL_APP_ID".</para>
/// </remarks>
public class AppIdHint : HintBase
{
    private const string Hint = "SDL_APP_ID";

    /// <summary>Gets or sets the value of the hint.</summary>
    /// <remarks>This hint should be set before SDL is initialized.</remarks>
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
