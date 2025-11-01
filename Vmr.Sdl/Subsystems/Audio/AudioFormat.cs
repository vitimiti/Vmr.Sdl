// -----------------------------------------------------------------------
// <copyright file="AudioFormat.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Vmr.Sdl.Subsystems.Audio;

/// <summary>Audio format.</summary>
[SuppressMessage(
    "Design",
    "CA1028: Enum Storage should be Int32",
    Justification = "The underlying type is uint for interop."
)]
public enum AudioFormat : uint
{
    /// <summary>Unspecified audio format.</summary>
    Unknown = 0x0000U,

    /// <summary>Unsigned 8-bit samples.</summary>
    U8 = 0x0008U,

    /// <summary>Signed 8-bit samples.</summary>
    S8 = 0x8008U,

    /// <summary>Signed 16-bit little-endian samples.</summary>
    S16LittleEndian = 0x8010U,

    /// <summary>Signed 32-bit little-endian samples.</summary>
    S32LittleEndian = 0x8020U,

    /// <summary>32-bit floating point little-endian samples.</summary>
    F32LittleEndian = 0x8120U,

    /// <summary>Signed 16-bit big-endian samples.</summary>
    S16BigEndian = 0x9010U,

    /// <summary>Signed 32-bit big-endian samples.</summary>
    S32BigEndian = 0x9020U,

    /// <summary>32-bit floating point big-endian samples.</summary>
    F32BigEndian = 0x9120U,
}

/// <summary>Native-endian audio formats.</summary>
public static class NativeEndiannessAudioFormat
{
    /// <summary>Gets the <see cref="AudioFormat"/> for the signed 16-bit native-endian samples.</summary>
    public static AudioFormat S16 =>
        BitConverter.IsLittleEndian ? AudioFormat.S16LittleEndian : AudioFormat.S16BigEndian;

    /// <summary>Gets the <see cref="AudioFormat"/> for the signed 32-bit native-endian samples.</summary>
    public static AudioFormat S32 =>
        BitConverter.IsLittleEndian ? AudioFormat.S32LittleEndian : AudioFormat.S32BigEndian;

    /// <summary>Gets the <see cref="AudioFormat"/> for the 32-bit floating point native-endian samples.</summary>
    public static AudioFormat F32 =>
        BitConverter.IsLittleEndian ? AudioFormat.F32LittleEndian : AudioFormat.F32BigEndian;
}
