namespace Dbarone.Net.Buffers;

/// <summary>
/// Describes operations that can be performed on a buffer.
/// </summary>
public interface IBuffer
{
    /// <summary>
    /// Clears bytes in the buffer
    /// </summary>
    /// <param name="index"></param>
    /// <param name="length"></param>
    public void Clear(int index, int length);

    /// <summary>
    /// Fills the buffer with a byte.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <param name="value"></param>
    public void Fill(int index, int length, byte value);

    /// <summary>
    /// Returns a byte array representation of the buffer.
    /// </summary>
    /// <returns></returns>
    public byte[] ToArray();

    /// <summary>
    /// Returns a slice of the byte array.
    /// </summary>
    /// <param name="index">The start of the byte array to return.</param>
    /// <param name="length">The length of the byte array to return.</param>
    /// <returns></returns>
    public byte[] Slice(long index, long length);

    /// <summary>
    /// Gets the memory stream used by the buffer.
    /// </summary>
    public MemoryStream Stream { get; }

    /// <summary>
    /// Gets the size of the buffer.
    /// </summary>
    public long Length { get; }

    /// <summary>
    /// Gets / sets the current position in the stream for next read or write.
    /// </summary>
    public long Position { get; set; }

    #region Read methods

    public UInt32 ReadBits(int bitWidth);
    public bool ReadBool();
    public Int32 ReadInt32(Endianness endianness = Endianness.DEFAULT);
    public UInt32 ReadUInt32(Endianness endianness = Endianness.DEFAULT);
    public Int64 ReadInt64(Endianness endianness = Endianness.DEFAULT);
    public UInt64 ReadUInt64(Endianness endianness = Endianness.DEFAULT);
    public float ReadFloat(Endianness endianness = Endianness.DEFAULT);
    public Double ReadDouble(Endianness endianness = Endianness.DEFAULT);
    public byte[] ReadBytes(int length);
    public ULEB128 ReadULEB128();
    public ZigZag ReadZigZag();
    public object Read(PhysicalDataType dataType, int length = 0, Endianness endianness = Endianness.DEFAULT);

    #endregion

    #region Write methods

    public int Write(bool value);
    public int Write(Int32 value, Endianness endianness = Endianness.DEFAULT);
    public int Write(UInt32 value, Endianness endianness = Endianness.DEFAULT);
    public int Write(Int64 value, Endianness endianness = Endianness.DEFAULT);
    public int Write(UInt64 value, Endianness endianness = Endianness.DEFAULT);
    public int Write(float value, Endianness endianness = Endianness.DEFAULT);
    public int Write(Double value, Endianness endianness = Endianness.DEFAULT);
    public int Write(byte[] value);
    public int Write(ULEB128 value);
    public int Write(ZigZag value);
    public int Write(object value, Endianness endianness = Endianness.DEFAULT);

    #endregion
}