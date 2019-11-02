using GeckoMapTester;

namespace SplatBox
{
  public static class ValidMemory
  {
    public static bool addressDebug = false;
    public static readonly AddressRange[] ValidAreas = new AddressRange[10]
    {
      new AddressRange(AddressType.Ex, 16777216U, 25165824U),
      new AddressRange(AddressType.Ex, 238026752U, 268435456U),
      new AddressRange(AddressType.Rw, 268435456U, 1342177280U),
      new AddressRange(AddressType.Ro, 3758096384U, 3825205248U),
      new AddressRange(AddressType.Ro, 3892314112U, 3925868544U),
      new AddressRange(AddressType.Ro, 4093640704U, 4127195136U),
      new AddressRange(AddressType.Ro, 4127195136U, 4135583744U),
      new AddressRange(AddressType.Ro, 4160749568U, 4211081216U),
      new AddressRange(AddressType.Ro, 4211081216U, 4219469824U),
      new AddressRange(AddressType.Rw, 4294836224U, uint.MaxValue)
    };

    public static AddressType rangeCheck(uint address)
    {
      int index = ValidMemory.rangeCheckId(address);
      if (index == -1)
        return AddressType.Unknown;
      return ValidMemory.ValidAreas[index].description;
    }

    public static int rangeCheckId(uint address)
    {
      for (int index = 0; index < ValidMemory.ValidAreas.Length; ++index)
      {
        AddressRange validArea = ValidMemory.ValidAreas[index];
        if (address >= validArea.low && address < validArea.high)
          return index;
      }
      return -1;
    }

    public static bool validAddress(uint address, bool debug)
    {
      if (debug)
        return true;
      return ValidMemory.rangeCheckId(address) >= 0;
    }

    public static bool validAddress(uint address)
    {
      return ValidMemory.validAddress(address, ValidMemory.addressDebug);
    }

    public static bool validRange(uint low, uint high, bool debug)
    {
      if (debug)
        return true;
      return ValidMemory.rangeCheckId(low) == ValidMemory.rangeCheckId(high - 1U);
    }

    public static bool validRange(uint low, uint high)
    {
      return ValidMemory.validRange(low, high, ValidMemory.addressDebug);
    }

    public static void setDataUpper(TCPGecko upper)
    {
      uint num1 = upper.OsVersionRequest();
      if (num1 <= 410U)
      {
        if ((int) num1 != 400 && (int) num1 != 410)
          return;
        uint num2 = upper.peek_kern(4293419420U);
        uint num3 = upper.peek_kern(num2 + 4U);
        uint num4 = upper.peek_kern(num3 + 20U);
        uint low1 = upper.peek_kern((uint) ((int) num4 + 0 + 0));
        uint num5 = upper.peek_kern((uint) ((int) num4 + 4 + 0));
        uint low2 = upper.peek_kern((uint) ((int) num4 + 0 + 16));
        uint num6 = upper.peek_kern((uint) ((int) num4 + 4 + 16));
        uint low3 = upper.peek_kern((uint) ((int) num4 + 0 + 32));
        uint num7 = upper.peek_kern((uint) ((int) num4 + 4 + 32));
        ValidMemory.ValidAreas[0] = new AddressRange(AddressType.Ex, low1, low1 + num5);
        ValidMemory.ValidAreas[1] = new AddressRange(AddressType.Ex, low2, low2 + num6);
        ValidMemory.ValidAreas[2] = new AddressRange(AddressType.Rw, low3, low3 + num7);
      }
      else if ((int) num1 == 500 || (int) num1 == 510)
        ;
    }
  }
}
