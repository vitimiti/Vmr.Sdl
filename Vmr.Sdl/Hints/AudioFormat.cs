// -----------------------------------------------------------------------
// <copyright file="AudioFormat.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Vmr.Sdl.Hints;

/// <summary>The possible audio formats.</summary>
public enum AudioFormat
{
    /// <summary>Unsigned 8-bit audio.</summary>
    U8,

    /// <summary>Signed 8-bit audio.</summary>
    S8,

    /// <summary>Signed 16-bit little-endian audio.</summary>
    S16LittleEndian,

    /// <summary>Signed 16-bit big-endian audio.</summary>
    S16BigEndian,

    /// <summary>Signed 16-bit native-endian audio.</summary>
    S16,

    /// <summary>Signed 32-bit little-endian audio.</summary>
    S32LittleEndian,

    /// <summary>Signed 32-bit big-endian audio.</summary>
    S32BigEndian,

    /// <summary>Signed 32-bit native-endian audio.</summary>
    S32,

    /// <summary>Floating point little-endian audio.</summary>
    F32LittleEndian,

    /// <summary>Floating point big-endian audio.</summary>
    F32BigEndian,

    /// <summary>Floating point native-endian audio.</summary>
    F32,
}
