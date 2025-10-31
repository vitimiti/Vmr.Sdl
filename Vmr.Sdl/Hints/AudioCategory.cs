// -----------------------------------------------------------------------
// <copyright file="AudioCategory.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Vmr.Sdl.Hints;

/// <summary>The iOS and macOS audio categories.</summary>
public enum AudioCategory
{
    /// <summary>Use the AVAudioSessionCategoryAmbient.</summary>
    Ambient,

    /// <summary>Use the AVAudioSessionCategoryPlayback.</summary>
    Playback,
}
