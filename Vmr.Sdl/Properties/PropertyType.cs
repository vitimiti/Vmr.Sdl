// -----------------------------------------------------------------------
// <copyright file="PropertyType.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Vmr.Sdl.Properties;

/// <summary>SDL property types.</summary>
[SuppressMessage(
    "Naming",
    "CA1720: Identifiers should not contain type names",
    Justification = "The types of the properties are actual types."
)]
public enum PropertyType
{
    /// <summary>An invalid property type.</summary>
    Invalid,

    /// <summary>A pointer property.</summary>
    Pointer,

    /// <summary>A string property.</summary>
    String,

    /// <summary>A number property.</summary>
    Number,

    /// <summary>A float property.</summary>
    Float,

    /// <summary>A boolean property.</summary>
    Boolean,
}
