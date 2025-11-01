// -----------------------------------------------------------------------
// <copyright file="FileDialogDriver.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Vmr.Sdl.Hints;

/// <summary>The possible file dialog drivers.</summary>
public enum FileDialogDriver
{
    /// <summary>Automatically selected.</summary>
    Default,

    /// <summary>Use XDG Portals through DBUS.</summary>
    /// <remarks>This is UNIX only.</remarks>
    Portal,

    /// <summary>Use the Zenity program.</summary>
    /// <remarks>This is UNIX only.</remarks>
    Zenity,
}
