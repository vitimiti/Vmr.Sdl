// -----------------------------------------------------------------------
// <copyright file="IoStatus.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Vmr.Sdl.Io;

/// <summary>The status of an IO operation.</summary>
public enum IoStatus
{
    /// <summary>The operation was successful.</summary>
    Ready,

    /// <summary>The operation failed.</summary>
    Error,

    /// <summary>The operation reached the end of the file.</summary>
    Eof,

    /// <summary>The operation is not ready yet.</summary>
    NotReady,

    /// <summary>The stream is read-only and the operation cannot be performed.</summary>
    ReadOnly,

    /// <summary>The stream is write-only and the operation cannot be performed.</summary>
    WriteOnly,
}
