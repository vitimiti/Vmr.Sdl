// -----------------------------------------------------------------------
// <copyright file="SdlStream.cs" company="Vmr.Sdl">
// Copyright (c) Vmr.Sdl. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Vmr.Sdl.NativeImports;
using Vmr.Sdl.Properties;

namespace Vmr.Sdl.Io;

/// <summary>Represents a stream wrapper for SDL I/O operations, providing <see cref="Stream"/> compatibility for SDL-based file and memory operations.</summary>
public class SdlStream : Stream
{
    private readonly NativeSdl.IoStream _baseStream;

    private bool _disposed;

    /// <summary>Initializes a new instance of the <see cref="SdlStream"/> class from a file path with specified mode and access.</summary>
    /// <param name="filePath">The path to the file to open.</param>
    /// <param name="mode">The file mode.</param>
    /// <param name="access">The file access permissions.</param>
    /// <param name="isBinaryFile">Whether to open the file in binary mode.</param>
    /// <exception cref="InvalidOperationException">Thrown when the SDL stream cannot be created from the file.</exception>
    public SdlStream(string filePath, FileMode mode, FileAccess access, bool isBinaryFile = false)
    {
        _baseStream = NativeSdl.IoFromFile(filePath, NativeSdl.ModeAndAccessToSdlMode(mode, access, isBinaryFile));
        if (_baseStream.IsInvalid)
        {
            throw new InvalidOperationException(
                $"Failed to create SDL stream from file \"{filePath}\" in {mode} mode and with {access} access ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Initializes a new instance of the <see cref="SdlStream"/> class from a writable memory buffer.</summary>
    /// <param name="buffer">The memory buffer to wrap.</param>
    /// <exception cref="InvalidOperationException">Thrown when the SDL stream cannot be created from the memory buffer.</exception>
    public SdlStream(Span<byte> buffer)
    {
        _baseStream = NativeSdl.IoFromMem(buffer.ToArray(), new CULong((uint)buffer.Length));
        if (_baseStream.IsInvalid)
        {
            throw new InvalidOperationException(
                $"Failed to create SDL stream from memory buffer ({NativeSdl.GetError()})."
            );
        }
    }

    /// <summary>Initializes a new instance of the <see cref="SdlStream"/> class from a read-only memory buffer.</summary>
    /// <param name="buffer">The read-only memory buffer to wrap.</param>
    /// <exception cref="InvalidOperationException">Thrown when the SDL stream cannot be created from the read-only memory buffer.</exception>
    public SdlStream(ReadOnlySpan<byte> buffer)
    {
        _baseStream = NativeSdl.IoFromConstMem(buffer.ToArray(), new CULong((uint)buffer.Length));
        if (_baseStream.IsInvalid)
        {
            throw new InvalidOperationException(
                $"Failed to create SDL stream from read-only memory buffer ({NativeSdl.GetError()})."
            );
        }
    }

    private SdlStream(NativeSdl.IoStream baseStream) => _baseStream = baseStream;

    /// <summary>Gets the current I/O status of the stream.</summary>
    public IoStatus Status => NativeSdl.GetIoStatus(_baseStream);

    /// <summary>Gets a value indicating whether the stream supports reading.</summary>
    public override bool CanRead => Status is not IoStatus.WriteOnly;

    /// <summary>Gets a value indicating whether the stream supports seeking.</summary>
    public override bool CanSeek => CanRead;

    /// <summary>Gets a value indicating whether the stream supports writing.</summary>
    public override bool CanWrite => Status is not IoStatus.ReadOnly;

    /// <summary>Gets the length of the stream in bytes.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the stream size cannot be determined.</exception>
    public override long Length
    {
        get
        {
            var size = NativeSdl.GetIoSize(_baseStream);
            return size < 0
                ? throw new InvalidOperationException(
                    $"Failed to get SDL stream size ({NativeSdl.GetError()}) - {Status}."
                )
                : size;
        }
    }

    /// <summary>Gets or sets the current position within the stream.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the position cannot be retrieved or set.</exception>
    public override long Position
    {
        get => Tell();
        set => Seek(value, SeekOrigin.Begin);
    }

    /// <summary>Creates a new <see cref="SdlStream"/> instance backed by dynamic memory that can grow as needed.</summary>
    /// <returns>A new <see cref="SdlStream"/> instance backed by dynamic memory.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the SDL stream cannot be created from dynamic memory.</exception>
    public static SdlStream FromDynamicMemory()
    {
        NativeSdl.IoStream stream = NativeSdl.IoFromDynamicMem();
        return stream.IsInvalid
            ? throw new InvalidOperationException(
                $"Failed to create SDL stream from dynamic memory ({NativeSdl.GetError()})."
            )
            : new SdlStream(stream);
    }

    /// <summary>Loads the entire contents of a file into a <see cref="Span{T}"/> of bytes.</summary>
    /// <param name="filePath">The path to the file to load.</param>
    /// <returns>A <see cref="Span{T}"/> containing the file contents.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the file cannot be loaded.</exception>
    public static Span<byte> LoadFile([NotNull] string filePath)
    {
        unsafe
        {
            var buffer = NativeSdl.LoadFile(filePath, out CULong dataSize);
            if (buffer is null)
            {
                throw new InvalidOperationException(
                    $"Unable to load from the file \"{filePath}\" ({NativeSdl.GetError()})."
                );
            }

            try
            {
                return new Span<byte>(buffer, (int)dataSize.Value);
            }
            finally
            {
                NativeSdl.Free(buffer);
            }
        }
    }

    /// <summary>Saves a buffer of bytes to a file.</summary>
    /// <param name="filePath">The path to the file to save to.</param>
    /// <param name="buffer">The buffer containing the data to save.</param>
    /// <exception cref="InvalidOperationException">Thrown when the file cannot be saved.</exception>
    public static void SaveFile([NotNull] string filePath, ReadOnlySpan<byte> buffer)
    {
        if (!NativeSdl.SaveFile(filePath, buffer.ToArray(), new CULong((uint)buffer.Length)))
        {
            throw new InvalidOperationException($"Unable to save to the file \"{filePath}\" ({NativeSdl.GetError()}).");
        }
    }

    /// <summary>Gets the properties associated with this I/O stream.</summary>
    /// <returns>An <see cref="IoProperties"/> instance containing the stream properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the stream properties cannot be retrieved.</exception>
    public IoProperties GetProperties()
    {
        NativeSdl.PropertiesId props = NativeSdl.GetIoProperties(_baseStream);
        return props == NativeSdl.PropertiesId.Invalid
            ? throw new InvalidOperationException(
                $"Failed to get SDL stream properties ({NativeSdl.GetError()}) - {Status}."
            )
            : new IoProperties(props);
    }

    /// <summary>Flushes any buffered data to the underlying storage.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the stream cannot be flushed.</exception>
    public override void Flush()
    {
        if (!NativeSdl.FlushIo(_baseStream))
        {
            throw new InvalidOperationException($"Failed to flush SDL stream ({NativeSdl.GetError()}) - {Status}.");
        }
    }

    /// <summary>Reads a sequence of bytes from the stream and advances the position within the stream by the number of bytes read.</summary>
    /// <param name="buffer">The buffer to read data into.</param>
    /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the stream.</param>
    /// <param name="count">The maximum number of bytes to read from the stream.</param>
    /// <returns>The total number of bytes read into the buffer.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the read operation fails.</exception>
    public override int Read([NotNull] byte[] buffer, int offset, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        var result = (int)NativeSdl.ReadIo(_baseStream, buffer[offset..count], new CULong((uint)count)).Value;
        return result <= 0 && Status is not IoStatus.Eof
            ? throw new InvalidOperationException(
                $"Failed to read {count} bytes from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : result;
    }

    /// <summary>Reads a single byte from the stream.</summary>
    /// <returns>The byte read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the byte cannot be read.</exception>
    public new byte ReadByte() =>
        !NativeSdl.ReadU8(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Failed to read a {nameof(Byte)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a signed byte from the stream.</summary>
    /// <returns>The signed byte read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the signed byte cannot be read.</exception>
    public sbyte ReadSByte() =>
        !NativeSdl.ReadS8(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Failed to read an {nameof(SByte)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 16-bit unsigned integer in little-endian byte order from the stream.</summary>
    /// <returns>The 16-bit unsigned integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public ushort ReadUInt16LittleEndian() =>
        !NativeSdl.ReadU16LittleEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a little-endian {nameof(UInt16)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 16-bit signed integer in little-endian byte order from the stream.</summary>
    /// <returns>The 16-bit signed integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public short ReadInt16LittleEndian() =>
        !NativeSdl.ReadS16LittleEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a little-endian {nameof(Int16)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 16-bit unsigned integer in big-endian byte order from the stream.</summary>
    /// <returns>The 16-bit unsigned integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public ushort ReadUInt16BigEndian() =>
        !NativeSdl.ReadU16BigEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a big-endian {nameof(UInt16)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 16-bit signed integer in big-endian byte order from the stream.</summary>
    /// <returns>The 16-bit signed integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public short ReadInt16BigEndian() =>
        !NativeSdl.ReadS16BigEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a big-endian {nameof(Int16)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 32-bit unsigned integer in little-endian byte order from the stream.</summary>
    /// <returns>The 32-bit unsigned integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public uint ReadUInt32LittleEndian() =>
        !NativeSdl.ReadU32LittleEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a little-endian {nameof(UInt32)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 32-bit signed integer in little-endian byte order from the stream.</summary>
    /// <returns>The 32-bit signed integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public int ReadInt32LittleEndian() =>
        !NativeSdl.ReadS32LittleEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a little-endian {nameof(Int32)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 32-bit unsigned integer in big-endian byte order from the stream.</summary>
    /// <returns>The 32-bit unsigned integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public uint ReadUInt32BigEndian() =>
        !NativeSdl.ReadU32BigEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a big-endian {nameof(UInt32)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 32-bit signed integer in big-endian byte order from the stream.</summary>
    /// <returns>The 32-bit signed integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public int ReadInt32BigEndian() =>
        !NativeSdl.ReadS32BigEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a big-endian {nameof(Int32)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 64-bit unsigned integer in little-endian byte order from the stream.</summary>
    /// <returns>The 64-bit unsigned integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public ulong ReadUInt64LittleEndian() =>
        !NativeSdl.ReadU64LittleEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a little-endian {nameof(UInt64)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 64-bit signed integer in little-endian byte order from the stream.</summary>
    /// <returns>The 64-bit signed integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public long ReadInt64LittleEndian() =>
        !NativeSdl.ReadS64LittleEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a little-endian {nameof(Int64)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 64-bit unsigned integer in big-endian byte order from the stream.</summary>
    /// <returns>The 64-bit unsigned integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public ulong ReadUInt64BigEndian() =>
        !NativeSdl.ReadU64BigEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a big-endian {nameof(UInt64)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Reads a 64-bit signed integer in big-endian byte order from the stream.</summary>
    /// <returns>The 64-bit signed integer read from the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the value cannot be read.</exception>
    public long ReadInt64BigEndian() =>
        !NativeSdl.ReadS64BigEndian(_baseStream, out var value)
            ? throw new InvalidOperationException(
                $"Unable to read a big-endian {nameof(Int64)} from SDL stream ({NativeSdl.GetError()}) - {Status}."
            )
            : value;

    /// <summary>Sets the position within the stream to the specified value.</summary>
    /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
    /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
    /// <returns>The new position within the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the seek operation fails.</exception>
    public override long Seek(long offset, SeekOrigin origin)
    {
        var result = NativeSdl.SeekIo(_baseStream, offset, NativeSdl.SeekOriginToIoWhence(origin));
        return result < 0
            ? throw new InvalidOperationException(
                $"Failed to seek SDL stream with offset {offset} and seek origin {origin} ({NativeSdl.GetError()}) - {Status}."
            )
            : result;
    }

    /// <summary>Gets the current position within the stream.</summary>
    /// <returns>The current position within the stream.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the position cannot be determined.</exception>
    public long Tell()
    {
        var result = NativeSdl.TellIo(_baseStream);
        return result < 0
            ? throw new InvalidOperationException($"Failed to tell SDL stream ({NativeSdl.GetError()}) - {Status}.")
            : result;
    }

    /// <summary>Sets the length of the stream.</summary>
    /// <param name="value">The desired length of the stream in bytes.</param>
    /// <exception cref="NotSupportedException">Always thrown as setting the length of SDL streams is not supported.</exception>
    public override void SetLength(long value) =>
        throw new NotSupportedException("Setting the length of SDL streams is not supported.");

    /// <summary>Writes a sequence of bytes to the stream and advances the current position within the stream by the number of bytes written.</summary>
    /// <param name="buffer">An array of bytes to write to the stream.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the stream.</param>
    /// <param name="count">The number of bytes to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public override void Write([NotNull] byte[] buffer, int offset, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        var result = (int)NativeSdl.WriteIo(_baseStream, buffer[offset..count], new CULong((uint)count)).Value;

        if (result < count)
        {
            throw new InvalidOperationException(
                $"Failed to write {count} bytes to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a byte to the stream and advances the position within the stream by one byte.</summary>
    /// <param name="value">The byte to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public override void WriteByte(byte value)
    {
        if (!NativeSdl.WriteU8(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Failed to write a {nameof(Byte)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a signed byte to the stream.</summary>
    /// <param name="value">The signed byte to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteSByte(sbyte value)
    {
        if (!NativeSdl.WriteS8(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Failed to write an {nameof(SByte)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 16-bit unsigned integer in little-endian byte order to the stream.</summary>
    /// <param name="value">The 16-bit unsigned integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteUInt16LittleEndian(ushort value)
    {
        if (!NativeSdl.WriteU16LittleEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a little-endian {nameof(UInt16)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 16-bit signed integer in little-endian byte order to the stream.</summary>
    /// <param name="value">The 16-bit signed integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteInt16LittleEndian(short value)
    {
        if (!NativeSdl.WriteS16LittleEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a little-endian {nameof(Int16)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 16-bit unsigned integer in big-endian byte order to the stream.</summary>
    /// <param name="value">The 16-bit unsigned integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteUInt16BigEndian(ushort value)
    {
        if (!NativeSdl.WriteU16BigEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a big-endian {nameof(UInt16)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 16-bit signed integer in big-endian byte order to the stream.</summary>
    /// <param name="value">The 16-bit signed integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteInt16BigEndian(short value)
    {
        if (!NativeSdl.WriteS16BigEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a big-endian {nameof(Int16)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 32-bit unsigned integer in little-endian byte order to the stream.</summary>
    /// <param name="value">The 32-bit unsigned integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteUInt32LittleEndian(uint value)
    {
        if (!NativeSdl.WriteU32LittleEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a little-endian {nameof(UInt32)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 32-bit signed integer in little-endian byte order to the stream.</summary>
    /// <param name="value">The 32-bit signed integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteInt32LittleEndian(int value)
    {
        if (!NativeSdl.WriteS32LittleEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a little-endian {nameof(Int32)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 32-bit unsigned integer in big-endian byte order to the stream.</summary>
    /// <param name="value">The 32-bit unsigned integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteUInt32BigEndian(uint value)
    {
        if (!NativeSdl.WriteU32BigEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a big-endian {nameof(UInt32)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 32-bit signed integer in big-endian byte order to the stream.</summary>
    /// <param name="value">The 32-bit signed integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteInt32BigEndian(int value)
    {
        if (!NativeSdl.WriteS32BigEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a big-endian {nameof(Int32)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 64-bit unsigned integer in little-endian byte order to the stream.</summary>
    /// <param name="value">The 64-bit unsigned integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteUInt64LittleEndian(ulong value)
    {
        if (!NativeSdl.WriteU64LittleEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a little-endian {nameof(UInt64)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 64-bit signed integer in little-endian byte order to the stream.</summary>
    /// <param name="value">The 64-bit signed integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteInt64LittleEndian(long value)
    {
        if (!NativeSdl.WriteS64LittleEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a little-endian {nameof(Int64)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 64-bit unsigned integer in big-endian byte order to the stream.</summary>
    /// <param name="value">The 64-bit unsigned integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteUInt64BigEndian(ulong value)
    {
        if (!NativeSdl.WriteU64BigEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a big-endian {nameof(UInt64)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Writes a 64-bit signed integer in big-endian byte order to the stream.</summary>
    /// <param name="value">The 64-bit signed integer to write to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the write operation fails.</exception>
    public void WriteInt64BigEndian(long value)
    {
        if (!NativeSdl.WriteS64BigEndian(_baseStream, value))
        {
            throw new InvalidOperationException(
                $"Unable to write a big-endian {nameof(Int64)} to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Prints a formatted message to the stream.</summary>
    /// <param name="message">The message to print to the stream.</param>
    /// <exception cref="InvalidOperationException">Thrown when the print operation fails.</exception>
    public void Print(string message)
    {
        if (NativeSdl.IoPrint(_baseStream, message).Value <= 0)
        {
            throw new InvalidOperationException(
                $"Failed to print the message \"{message}\" to SDL stream ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <summary>Closes the stream and releases any resources associated with it.</summary>
    /// <exception cref="InvalidOperationException">Thrown when the stream cannot be closed.</exception>
    public override void Close()
    {
        if (!NativeSdl.CloseIo(_baseStream))
        {
            throw new InvalidOperationException($"Failed to close SDL stream ({NativeSdl.GetError()}) - {Status}.");
        }

        base.Close();
    }

    /// <summary>Loads the entire contents of this stream into a <see cref="Span{T}"/> of bytes.</summary>
    /// <param name="closeStream">Whether to close the stream after loading.</param>
    /// <returns>A <see cref="Span{T}"/> containing the stream contents.</returns>
    public Span<byte> LoadFile(bool closeStream = true)
    {
        unsafe
        {
            var buffer = NativeSdl.LoadFileIo(_baseStream, out CULong dataSize, closeStream);
            try
            {
                return new Span<byte>(buffer, (int)dataSize.Value);
            }
            finally
            {
                NativeSdl.Free(buffer);
            }
        }
    }

    /// <summary>Saves the contents of a buffer to this stream as a file.</summary>
    /// <param name="buffer">The buffer containing the data to save.</param>
    /// <param name="closeStream">Whether to close the stream after saving.</param>
    /// <exception cref="InvalidOperationException">Thrown when the save operation fails.</exception>
    public void SaveFile([NotNull] ReadOnlyMemory<byte> buffer, bool closeStream = true)
    {
        if (!NativeSdl.SaveFileIo(_baseStream, buffer.ToArray(), new CULong((uint)buffer.Length), closeStream))
        {
            throw new InvalidOperationException(
                $"Failed to save SDL stream to file ({NativeSdl.GetError()}) - {Status}."
            );
        }
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _baseStream.Dispose();
        }

        _disposed = true;
        base.Dispose(disposing);
    }
}
