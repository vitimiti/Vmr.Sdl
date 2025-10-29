// -----------------------------------------------------------------------
// <copyright file="SdlApplicationMetadata.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl;

/// <summary>Represents metadata information for an SDL application, including properties such as name, version, identifier, creator, copyright, URL, and app type.</summary>
/// <remarks>This class is used to interact with SDL and manage metadata through native SDL APIs. Updates to this metadata will reflect through SDL's native functionality.</remarks>
public class SdlApplicationMetadata
{
    /// <summary>Initializes a new instance of the <see cref="SdlApplicationMetadata"/> class.</summary>
    /// <remarks>This constructor is used to initialize an instance of the <see cref="SdlApplicationMetadata"/> class with default values.</remarks>
    public SdlApplicationMetadata() { }

    /// <summary>Initializes a new instance of the <see cref="SdlApplicationMetadata"/> class with the specified metadata.</summary>
    /// <param name="name">The name of the application.</param>
    /// <param name="version">The version of the application.</param>
    /// <param name="appIdentifier">The identifier of the application.</param>
    /// <remarks>This constructor is used to initialize an instance of the <see cref="SdlApplicationMetadata"/> class with the specified metadata.</remarks>
    /// <exception cref="InvalidOperationException">When the SDL application metadata failed to be set.</exception>
    public SdlApplicationMetadata(string? name, Version? version, string? appIdentifier)
    {
        if (!NativeSdl.SetAppMetadata(name, version?.ToString(), appIdentifier))
        {
            throw new InvalidOperationException(
                $"Unable to set application metadata {{ {nameof(name)}: {name}, {nameof(version)}: {version}, {nameof(appIdentifier)}: {appIdentifier} }} ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Gets or sets the name of the application.</summary>
    /// <exception cref="InvalidOperationException">When the SDL application metadata failed to be set.</exception>
    public string? Name
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return NativeSdl.GetAppMetadataProperty(NativeSdl.PropAppMetadataNameString);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            if (!NativeSdl.SetAppMetadataProperty(NativeSdl.PropAppMetadataNameString, value))
            {
                throw new InvalidOperationException(FormErrorString(nameof(Name), value));
            }
        }
    }

    /// <summary>Gets or sets the version of the application.</summary>
    /// <exception cref="InvalidOperationException">When the SDL application metadata failed to be set.</exception>
    public Version? Version
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            var versionString = NativeSdl.GetAppMetadataProperty(NativeSdl.PropAppMetadataVersionString);
            return versionString is null ? null : new Version(versionString);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            if (!NativeSdl.SetAppMetadataProperty(NativeSdl.PropAppMetadataVersionString, value?.ToString()))
            {
                throw new InvalidOperationException(FormErrorString(nameof(Version), value?.ToString()));
            }
        }
    }

    /// <summary>Gets or sets the identifier of the application.</summary>
    /// <exception cref="InvalidOperationException">When the SDL application metadata failed to be set.</exception>
    public string? Identifier
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return NativeSdl.GetAppMetadataProperty(NativeSdl.PropAppMetadataIdentifierString);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            if (!NativeSdl.SetAppMetadataProperty(NativeSdl.PropAppMetadataIdentifierString, value))
            {
                throw new InvalidOperationException(FormErrorString(nameof(Identifier), value));
            }
        }
    }

    /// <summary>Gets or sets the creator of the application.</summary>
    /// <exception cref="InvalidOperationException">When the SDL application metadata failed to be set.</exception>
    public string? Creator
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return NativeSdl.GetAppMetadataProperty(NativeSdl.PropAppMetadataCreatorString);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            if (!NativeSdl.SetAppMetadataProperty(NativeSdl.PropAppMetadataCreatorString, value))
            {
                throw new InvalidOperationException(FormErrorString(nameof(Creator), value));
            }
        }
    }

    /// <summary>Gets or sets the copyright information for the SDL application.</summary>
    /// <exception cref="InvalidOperationException">When setting the copyright value fails due to an error in the native SDL API.</exception>
    public string? Copyright
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return NativeSdl.GetAppMetadataProperty(NativeSdl.PropAppMetadataCopyrightString);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            if (!NativeSdl.SetAppMetadataProperty(NativeSdl.PropAppMetadataCopyrightString, value))
            {
                throw new InvalidOperationException(FormErrorString(nameof(Copyright), value));
            }
        }
    }

    /// <summary>Gets or sets the URL associated with the SDL application.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the SDL application metadata update for the URL fails.</exception>
    public Uri? Url
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            var urlString = NativeSdl.GetAppMetadataProperty(NativeSdl.PropAppMetadataUrlString);
            return urlString is null ? null : new Uri(urlString);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            if (!NativeSdl.SetAppMetadataProperty(NativeSdl.PropAppMetadataUrlString, value?.ToString()))
            {
                throw new InvalidOperationException(FormErrorString(nameof(Url), value?.ToString()));
            }
        }
    }

    /// <summary>Gets or sets the application type metadata.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the application type metadata fails to be set through the SDL API.</exception>
    public string? AppType
    {
        get
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            return NativeSdl.GetAppMetadataProperty(NativeSdl.PropAppMetadataTypeString);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(this); // Will never happen, but it prevents static usage of this property.
            if (!NativeSdl.SetAppMetadataProperty(NativeSdl.PropAppMetadataTypeString, value))
            {
                throw new InvalidOperationException(FormErrorString(nameof(AppType), value));
            }
        }
    }

    private static string FormErrorString(string name, string? value) =>
        $"Unable to set the application property {name} to \"{value}\" ({NativeSdl.GetError()}).";
}
