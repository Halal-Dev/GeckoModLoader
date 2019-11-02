using System;

namespace SplatBox
{
  public class ByteSwap
  {
    public static ushort Swap(ushort input)
    {
      if (BitConverter.IsLittleEndian)
        return (ushort) ((65280 & (int) input) >> 8 | ((int) byte.MaxValue & (int) input) << 8);
      return input;
    }

    public static uint Swap(uint input)
    {
      if (BitConverter.IsLittleEndian)
        return (uint) ((int) ((4278190080U & input) >> 24) | (int) ((16711680U & input) >> 8) | (65280 & (int) input) << 8 | ((int) byte.MaxValue & (int) input) << 24);
      return input;
    }

    public static ulong Swap(ulong input)
    {
      if (BitConverter.IsLittleEndian)
        return (ulong) ((long) ((18374686479671623680UL & input) >> 56) | (long) ((71776119061217280UL & input) >> 40) | (long) ((280375465082880UL & input) >> 24) | (long) ((1095216660480UL & input) >> 8) | (4278190080L & (long) input) << 8 | (16711680L & (long) input) << 24 | (65280L & (long) input) << 40 | ((long) byte.MaxValue & (long) input) << 56);
      return input;
    }
  }
}
