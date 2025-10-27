// -----------------------------------------------------------------------
// <copyright file="GlobalPropertiesGroup.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Properties;

/// <summary>The SDL global properties group.</summary>
public class GlobalPropertiesGroup : PropertiesGroup
{
    /// <summary>Initializes a new instance of the <see cref="GlobalPropertiesGroup"/> class.</summary>
    /// <remarks>This property is global and won't be destroyed on disposing.</remarks>
    public GlobalPropertiesGroup()
        : base(NativeSdl.GetGlobalProperties(), isGlobal: true) { }
}
