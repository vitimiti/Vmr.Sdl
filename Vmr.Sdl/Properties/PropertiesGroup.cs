// -----------------------------------------------------------------------
// <copyright file="PropertiesGroup.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Properties;

/// <summary>The SDL properties group.</summary>
[DebuggerDisplay("ID = {_id.Value,nq}")]
public class PropertiesGroup : IDisposable
{
    private readonly NativeSdl.PropertiesId _id;
    private readonly bool _isGlobal;

    private bool _disposed;

    /// <summary>A delegate to list all the properties of a given group.</summary>
    /// <param name="group">The <see cref="PropertiesGroup"/> to list the properties of.</param>
    /// <param name="propertyName">The name of the property being listed.</param>
    public delegate void EnumerateProperties(PropertiesGroup group, string propertyName);

    /// <summary>Initializes a new instance of the <see cref="PropertiesGroup"/> class.</summary>
    /// <exception cref="InvalidOperationException">When the SDL properties ID failed to be created.</exception>
    public PropertiesGroup()
    {
        _id = NativeSdl.CreateProperties();
        if (_id == NativeSdl.PropertiesId.Invalid)
        {
            throw new InvalidOperationException($"Failed to create properties group ({NativeSdl.GetError()}).");
        }
    }

    internal PropertiesGroup(NativeSdl.PropertiesId id, bool isGlobal)
    {
        if (id == NativeSdl.PropertiesId.Invalid)
        {
            throw new ArgumentException($"Invalid properties id ({NativeSdl.GetError()}).", nameof(id));
        }

        _isGlobal = isGlobal;
    }

    /// <summary>Gets whether the given property exists within this group.</summary>
    /// <param name="name">The property name to query.</param>
    /// <returns><see langword="true"/> if the queried property <paramref name="name"/> is in this <see cref="PropertiesGroup"/>, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public bool HasProperty(string name)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        return NativeSdl.HasProperty(_id, name);
    }

    /// <summary>Gets the property type of the given property.</summary>
    /// <param name="name">The property name to query.</param>
    /// <returns>A <see cref="PropertyType"/> value with the type of the queried property <paramref name="name"/>.</returns>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public PropertyType GetPropertyType(string name)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        return NativeSdl.GetPropertyType(_id, name);
    }

    /// <summary>Sets a pointer value to a named property.</summary>
    /// <param name="name">The property to set the pointer value to.</param>
    /// <param name="value">The pointer value for the property.</param>
    /// <exception cref="InvalidOperationException">When setting the given <paramref name="value"/> to the given property <paramref name="name"/> fails.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public void SetProperty(string name, nint value)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.SetPointerProperty(_id, name, value))
        {
            throw new InvalidOperationException(
                $"Failed to set property \"{name}\" to value {value:X16} ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Sets a string value to a named property.</summary>
    /// <param name="name">The property to set the string value to.</param>
    /// <param name="value">The string value for the property.</param>
    /// <exception cref="InvalidOperationException">When setting the given <paramref name="value"/> to the given property <paramref name="name"/> fails.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public void SetProperty(string name, string? value)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.SetStringProperty(_id, name, value))
        {
            throw new InvalidOperationException(
                $"Failed to set property \"{name}\" to value \"{value}\" ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Sets a number value to a named property.</summary>
    /// <param name="name">The property to set the number value to.</param>
    /// <param name="value">The number value for the property.</param>
    /// <exception cref="InvalidOperationException">When setting the given <paramref name="value"/> to the given property <paramref name="name"/> fails.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public void SetProperty(string name, long value)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.SetNumberProperty(_id, name, value))
        {
            throw new InvalidOperationException(
                $"Failed to set property \"{name}\" to value {value} ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Sets a floating-point number value to a named property.</summary>
    /// <param name="name">The property to set the floating-point number value to.</param>
    /// <param name="value">The floating-point number value for the property.</param>
    /// <exception cref="InvalidOperationException">When setting the given <paramref name="value"/> to the given property <paramref name="name"/> fails.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public void SetProperty(string name, float value)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.SetFloatProperty(_id, name, value))
        {
            throw new InvalidOperationException(
                $"Failed to set property \"{name}\" to value {value:F4} ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Sets a boolean value to a named property.</summary>
    /// <param name="name">The property to set the boolean value to.</param>
    /// <param name="value">The boolean value for the property.</param>
    /// <exception cref="InvalidOperationException">When setting the given <paramref name="value"/> to the given <paramref name="name"/> fails.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public void SetBooleanProperty(string name, bool value)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.SetBooleanProperty(_id, name, value))
        {
            throw new InvalidOperationException(
                $"Failed to set property \"{name}\" to value \"{value}\" ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Gets the value of the given pointer property.</summary>
    /// <param name="name">The property name to get the value from.</param>
    /// <param name="defaultValue">The default value of the property, if it couldn't be retrieved.</param>
    /// <returns>The pointer value of the queried property <paramref name="name"/>, or <paramref name="defaultValue"/> if it couldn't be found.</returns>
    /// <exception cref="InvalidOperationException">When the property couldn't be locked to retrieve the pointer value.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    /// <remarks>This type of property requires locking to prevent data corruption.</remarks>
    public nint GetPointerProperty(string name, nint defaultValue = 0)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.LockProperties(_id))
        {
            throw new InvalidOperationException(
                $"Failed to lock properties while getting pointer property \"{name}\" ({NativeSdl.GetError()})."
            );
        }

        try
        {
            return NativeSdl.GetPointerProperty(_id, name, defaultValue);
        }
        finally
        {
            NativeSdl.UnlockProperties(_id);
        }
    }

    /// <summary>Gets the value of the given string property.</summary>
    /// <param name="name">The property name to get the value from.</param>
    /// <param name="defaultValue">The default value of the property, if it couldn't be retrieved.</param>
    /// <returns>The string value of the queried property <paramref name="name"/>, or <paramref name="defaultValue"/> if it couldn't be found.</returns>
    /// <exception cref="InvalidOperationException">When the property couldn't be locked to retrieve the string value.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    /// <remarks>This type of property requires locking to prevent data corruption.</remarks>
    public string? GetStringProperty(string name, string? defaultValue = null)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.LockProperties(_id))
        {
            throw new InvalidOperationException(
                $"Failed to lock properties while getting string property \"{name}\" ({NativeSdl.GetError()})."
            );
        }

        try
        {
            return NativeSdl.GetStringProperty(_id, name, defaultValue);
        }
        finally
        {
            NativeSdl.UnlockProperties(_id);
        }
    }

    /// <summary>Gets the value of the given number property.</summary>
    /// <param name="name">The property name to get the value from.</param>
    /// <param name="defaultValue">The default value of the property, if it couldn't be retrieved.</param>
    /// <returns>The number value of the queried property <paramref name="name"/>, or <paramref name="defaultValue"/> if it couldn't be found.</returns>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public long GetNumberProperty(string name, long defaultValue = 0)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        return NativeSdl.GetNumberProperty(_id, name, defaultValue);
    }

    /// <summary>Gets the value of the given floating-point number property.</summary>
    /// <param name="name">The property name to get the value from.</param>
    /// <param name="defaultValue">The default value of the property, if it couldn't be retrieved.</param>
    /// <returns>The floating-point number value of the queried property <paramref name="name"/>, or <paramref name="defaultValue"/> if it couldn't be found.</returns>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public float GetFloatProperty(string name, float defaultValue = 0F)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        return NativeSdl.GetFloatProperty(_id, name, defaultValue);
    }

    /// <summary>Gets the value of the given boolean property.</summary>
    /// <param name="name">The property name to get the value from.</param>
    /// <param name="defaultValue">The default value of the property, if it couldn't be retrieved.</param>
    /// <returns>The boolean value of the queried property <paramref name="name"/>, or <paramref name="defaultValue"/> if it couldn't be found.</returns>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public bool GetBooleanProperty(string name, bool defaultValue = false)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        return NativeSdl.GetBooleanProperty(_id, name, defaultValue);
    }

    /// <summary>Clears the given property from the <see cref="PropertiesGroup"/>.</summary>
    /// <param name="name">The property to clear.</param>
    /// <exception cref="InvalidOperationException">When clearing the given property <paramref name="name"/> fails.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public void ClearProperty(string name)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.ClearProperty(_id, name))
        {
            throw new InvalidOperationException($"Unable to clear the property \"{name}\" ({NativeSdl.GetError()}).");
        }
    }

    /// <summary>Lists all the properties in the <see cref="PropertiesGroup"/>.</summary>
    /// <param name="callback">The <see cref="EnumerateProperties"/> callback to call for each property.</param>
    /// <exception cref="InvalidOperationException">When the properties couldn't be listed.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public void Enumerate(EnumerateProperties callback)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.EnumerateProperties(_id, callback))
        {
            throw new InvalidOperationException($"Failed to enumerate properties ({NativeSdl.GetError()}).");
        }
    }

    /// <summary>Copies the current <see cref="PropertiesGroup"/> to another one.</summary>
    /// <param name="other">The other <see cref="PropertiesGroup"/> to copy to.</param>
    /// <exception cref="InvalidOperationException">When copying to the given <paramref name="other"/> <see cref="PropertiesGroup"/> fails.</exception>
    /// <exception cref="ObjectDisposedException">When the <see cref="PropertiesGroup"/> object had been disposed.</exception>
    public void CopyTo([NotNull] PropertiesGroup other)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(PropertiesGroup));

        if (!NativeSdl.CopyProperties(_id, other._id))
        {
            throw new InvalidOperationException($"Failed to copy properties group ({NativeSdl.GetError()}).");
        }
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Finalizes an instance of the <see cref="PropertiesGroup"/> class.</summary>
    ~PropertiesGroup() => Dispose(disposing: false);

    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// <param name="disposing">Whether to dispose of the managed objects or not.</param>
    /// <remarks>Override this method to do your own disposing.</remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (NativeSdl.EnumeratePropertiesCallbackHandle.IsAllocated)
        {
            NativeSdl.EnumeratePropertiesCallbackHandle.Free();
        }

        if (!_isGlobal)
        {
            NativeSdl.DestroyProperties(_id);
        }

        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Nothing to dispose of.
        }

        _disposed = true;
    }
}
