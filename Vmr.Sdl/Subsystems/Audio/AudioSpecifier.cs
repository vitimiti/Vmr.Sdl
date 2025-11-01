// -----------------------------------------------------------------------
// <copyright file="AudioSpecifier.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Vmr.Sdl.Subsystems.Audio;

/// <summary>Format specifier for audio data.</summary>
public record AudioSpecifier
{
    /// <summary>Gets or sets the audio data format.</summary>
    public AudioFormat Format { get; set; }

    /// <summary>Gets or sets the number of channels: 1 mono, 2 stereo, ect.</summary>
    public int ChannelsCount { get; set; }

    /// <summary>Gets or sets the sample rate: sample frames per second.</summary>
    public int Frequency { get; set; }

    /// <summary>Gets the size of each audio frame (in bytes).</summary>
    /// <remarks>This reports on the size of an audio sample frame: stereo <see cref="short"/> data (2 channels of 2 bytes each) would be 4 bytes per frame, for example.</remarks>
    public int FrameSize => ((int)Format & 0xFF) / 8 * ChannelsCount;
}
