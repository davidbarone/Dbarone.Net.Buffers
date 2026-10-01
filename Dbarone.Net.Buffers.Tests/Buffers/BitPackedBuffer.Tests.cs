namespace Dbarone.Net.Buffers.Tests;

using Dbarone.Net.Buffers;

public class BitPackedBufferTests
{
  [Fact]
  public void TestReadFromLSB()
  {
    byte[] bytes = [0B10000100, 0B00000001];
    GenericBuffer buf = new GenericBuffer(bytes);
    var bpb = new BitPackedBuffer(buf, BitOrder.LSB);

    // Read 8 lots of 2 bits at a time:
    byte[] results = new byte[8];
    for (int i = 0; i < 8; i++)
    {
      results[i] = (byte)bpb.ReadBits(2);
    }

    // Assert
    Assert.Equal(new byte[] { 0x00, 0x01, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00 }, results);
  }

  [Fact]
  public void TestReadFromMSB()
  {
    byte[] bytes = [0B10000100, 0B00000001];
    GenericBuffer buf = new GenericBuffer(bytes);
    var bpb = new BitPackedBuffer(buf, BitOrder.MSB);

    // Read 8 lots of 2 bits at a time:
    byte[] results = new byte[8];
    for (int i = 0; i < 8; i++)
    {
      results[i] = (byte)bpb.ReadBits(2);
    }

    // Assert
    Assert.Equal(new byte[] { 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x01 }, results);
  }
}