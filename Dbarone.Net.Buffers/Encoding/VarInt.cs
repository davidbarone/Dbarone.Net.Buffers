namespace Dbarone.Net.Buffers;

/// <summary>
/// Represents a variable-length unsigned integer.
/// This encoding is used in Sqlite.
/// This encoding uses big-endian byte order, with
/// a maximum length of 9 bytes for 64-bit integers.
/// This differs from ULEB128 which uses little-endian
/// byte order.
/// https://deepwiki.com/go-sqlite/sqlite3/3.2-varint-encoding-and-decoding
/// </summary>
public struct VarInt
{
    /// <summary>
    /// The integer value of the VarInt. 
    /// </summary>
    public ulong Value { get; set; } = 0;

    /// <summary>
    /// The byte[] array representation of the VarInt value.
    /// </summary>
    public byte[] Bytes { get; set; } = new byte[0];

    /// <summary>
    /// The length in bytes that the VarInt uses to store the integer value.
    /// </summary>
    public int Size { get; set; } = 0;

    #region Ctors

    public VarInt(ulong value)
    {
        Value = value;
        Bytes = this.ULongToByteArray(value);
        Size = Bytes.Length;
    }

    public VarInt(byte[] bytes)
    {
        ulong value = 0;
        int i = 0;

        // SQLite varints are at most 9 bytes
        for (; i < 9; i++)
        {
            byte b = bytes[i];

            if (i == 8)
            {
                // Special case: 9th byte — store full 8 bits
                value = (value << 8) | b;
                i++; // Count the 9th byte
                break;
            }
            else
            {
                // Append lower 7 bits
                value = (value << 7) | (ulong)(b & 0x7F);

                // If high bit is 0, this is the last byte
                if ((b & 0x80) == 0)
                {
                    i++;
                    break;
                }
            }
        }

        Size = i;
        Value = value;
        Bytes = new byte[Size];
        Array.Copy(bytes, 0, Bytes, 0, Size);
    }

    #endregion

    #region Implicit Ctor

    // Int8 / sbyte
    public static implicit operator sbyte(VarInt value)
    {
        return (sbyte)value.Value;
    }

    // Int8 / sbyte
    public static implicit operator VarInt(sbyte value)
    {
        return new VarInt((ulong)value);
    }

    // UInt8 / byte
    public static implicit operator byte(VarInt value)
    {
        return (byte)value.Value;
    }

    // UInt8 / byte
    public static implicit operator VarInt(byte value)
    {
        return new VarInt(value);
    }

    // Int16
    public static implicit operator Int16(VarInt value)
    {
        return (Int16)value.Value;
    }

    // Int16
    public static implicit operator VarInt(Int16 value)
    {
        return new VarInt((ulong)value);
    }

    // UInt16
    public static implicit operator UInt16(VarInt value)
    {
        return (UInt16)value.Value;
    }

    // UInt16
    public static implicit operator VarInt(UInt16 value)
    {
        return new VarInt(value);
    }

    // Int32
    public static implicit operator Int32(VarInt value)
    {
        return (Int32)value.Value;
    }

    // Int32
    public static implicit operator VarInt(Int32 value)
    {
        return new VarInt((ulong)value);
    }

    // Int32
    public static implicit operator UInt32(VarInt value)
    {
        return (UInt32)value.Value;
    }

    // Int32
    public static implicit operator VarInt(UInt32 value)
    {
        return new VarInt(value);
    }

    // Int64
    public static implicit operator Int64(VarInt value)
    {
        return (Int64)value.Value;
    }

    // Int64
    public static implicit operator VarInt(Int64 value)
    {
        return new VarInt((ulong)value);
    }

    // UInt64
    public static implicit operator UInt64(VarInt value)
    {
        return (UInt64)value.Value;
    }

    // UInt64
    public static implicit operator VarInt(UInt64 value)
    {
        return new VarInt(value);
    }

    #endregion

    #region Operator Overloading

    public static VarInt operator +(VarInt left, VarInt right)
    {
        return new VarInt(left + right);
    }

    public static VarInt operator -(VarInt left, VarInt right)
    {
        return new VarInt(left - right);
    }

    public static VarInt operator *(VarInt left, VarInt right)
    {
        return new VarInt(left * right);
    }

    public static VarInt operator /(VarInt left, VarInt right)
    {
        return new VarInt(left / right);
    }

    #endregion

    #region Private members

    private byte[] ULongToByteArray(ulong value)
    {
        // Special case: 9-byte varint for values >= 2^56
        // first byte = 0xFF, then the next 8 bytes are
        // the ulong value in big-endian format.
        if (value > 0x00FFFFFFFFFFFFFFUL)
        {
            byte[] result = new byte[9];
            result[0] = 0xFF; // Marker for 9-byte varint
            for (int i = 8; i >= 1; i--)
            {
                result[i] = (byte)(value & 0xFF);
                value >>= 8;
            }
            return result;
        }

        // General case: encode in 1–8 bytes
        List<byte> bytes = new List<byte>();
        do
        {
            bytes.Insert(0, (byte)(value & 0x7F)); // Take 7 bits
            value >>= 7;
        } while (value > 0);

        // Set continuation bits for all but last byte
        for (int i = 0; i < bytes.Count - 1; i++)
        {
            bytes[i] |= 0x80;
        }

        return bytes.ToArray();
    }

    #endregion
}