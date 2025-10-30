// -----------------------------------------------------------------------
// <copyright file="IoProperties.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Vmr.Sdl.NativeImports;

namespace Vmr.Sdl.Properties;

/// <summary>Represents a group of Input/Output (I/O) properties associated with the SDL library.</summary>
/// <remarks>This class provides access to various handles and descriptors related to I/O streams, such as native file handles, memory pointers, and specific platform-related assets.</remarks>
public class IoProperties : PropertiesGroup
{
    internal IoProperties(NativeSdl.PropertiesId propertiesId)
        : base(propertiesId, isGlobal: true) { }

    /// <summary>Gets the native Windows handle associated with an I/O stream.</summary>
    /// <remarks>This property interacts with the underlying native layer to retrieve the Windows handle that is used in conjunction with the corresponding I/O stream. It is specifically intended for use in environments where interoperability with Windows-specific APIs or components is required.</remarks>
    public nint WindowsHandle => GetPointerProperty(NativeSdl.PropIoStreamWindowsHandlePointer);

    /// <summary>Gets the native file handle for standard I/O operations.</summary>
    /// <remarks>This property retrieves a pointer to the standard I/O file handle using the underlying native representation. It is intended for scenarios where direct interaction with the standard input or output file handle is required, leveraging platform-specific capabilities.</remarks>
    public nint StdioFileHandle => GetPointerProperty(NativeSdl.PropIoStreamStdioFilePointer);

    /// <summary>Gets the file descriptor associated with the I/O stream.</summary>
    /// <remarks>This property retrieves a numerical identifier for the underlying file or other I/O resource. It is typically used for low-level operations or interoperability with native APIs that require direct access to file descriptors.</remarks>
    public long FileDescriptor => GetNumberProperty(NativeSdl.PropIoStreamFileDescriptorNumber);

    /// <summary>Gets the native Android asset handle associated with an I/O stream.</summary>
    /// <remarks>This property interacts with the underlying native layer to retrieve a handle specific to Android assets. It is designed to enable interoperability with Android's native asset management system when accessing resources through an I/O stream.</remarks>
    public nint AndroidAAssetHandle => GetPointerProperty(NativeSdl.PropIoStreamAndroidAAssetPointer);

    /// <summary>Gets the memory pointer associated with an I/O stream.</summary>
    /// <remarks>This property retrieves a pointer to the memory region backing the corresponding I/O stream. It allows for low-level access or interaction where direct memory manipulation or inspection is necessary.</remarks>
    public nint MemoryHandle => GetPointerProperty(NativeSdl.PropIoStreamMemoryPointer);

    /// <summary>Gets the size of the memory associated with the I/O stream.</summary>
    /// <remarks>This property retrieves the memory consumption of an I/O stream by interacting with native platform-specific implementations. It is useful for monitoring and optimizing resource usage in applications that handle I/O operations.</remarks>
    public long MemorySize => GetNumberProperty(NativeSdl.PropIoStreamMemorySizeNumber);

    /// <summary>Gets a pointer to the dynamic memory location associated with the I/O stream.</summary>
    /// <remarks>This property retrieves a native pointer that refers to the dynamic memory assigned for an I/O stream. It facilitates memory operations or interactions specific to the underlying system's dynamic allocation mechanisms.</remarks>
    public nint DynamicMemoryHandle => GetPointerProperty(NativeSdl.PropIoStreamDynamicMemoryPointer);

    /// <summary>Gets the size of the dynamic memory chunk used for I/O stream operations.</summary>
    /// <remarks>This property retrieves the configured size of dynamically allocated memory chunks for stream operations using the underlying native property. It serves to optimize memory allocation during data stream processing by adjusting the size of memory blocks allocated dynamically based on specific requirements.</remarks>
    public long DynamicMemorySize => GetNumberProperty(NativeSdl.PropIoStreamDynamicChunkSizeNumber);
}
