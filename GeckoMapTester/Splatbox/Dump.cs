using System;
using System.IO;

namespace SplatBox
{
  public class Dump
  {
    public byte[] mem;
    private uint startAddress;
    private uint endAddress;
    private uint readCompletedAddress;
    private int fileNumber;

    public uint StartAddress
    {
      get
      {
        return this.startAddress;
      }
    }

    public uint EndAddress
    {
      get
      {
        return this.endAddress;
      }
    }

    public uint ReadCompletedAddress
    {
      get
      {
        return this.readCompletedAddress;
      }
      set
      {
        this.readCompletedAddress = value;
      }
    }

    public Dump(uint theStartAddress, uint theEndAddress)
    {
      this.Construct(theStartAddress, theEndAddress, 0);
    }

    public Dump(uint theStartAddress, uint theEndAddress, int theFileNumber)
    {
      this.Construct(theStartAddress, theEndAddress, theFileNumber);
    }

    private void Construct(uint theStartAddress, uint theEndAddress, int theFileNumber)
    {
      this.startAddress = theStartAddress;
      this.endAddress = theEndAddress;
      this.readCompletedAddress = theStartAddress;
      this.mem = new byte[(int) this.endAddress - (int) this.startAddress];
      this.fileNumber = theFileNumber;
    }

    public uint ReadAddress32(uint addressToRead)
    {
      if (addressToRead < this.startAddress || addressToRead > this.endAddress - 4U)
        return 0;
      byte[] numArray = new byte[4];
      Buffer.BlockCopy((Array) this.mem, this.index(addressToRead), (Array) numArray, 0, 4);
      return ByteSwap.Swap(BitConverter.ToUInt32(numArray, 0));
    }

    private int index(uint addressToRead)
    {
      return (int) addressToRead - (int) this.startAddress;
    }

    public uint ReadAddress(uint addressToRead, int numBytes)
    {
      if (addressToRead < this.startAddress || (long) addressToRead > (long) this.endAddress - (long) numBytes)
        return 0;
      byte[] numArray = new byte[4];
      Buffer.BlockCopy((Array) this.mem, this.index(addressToRead), (Array) numArray, 0, numBytes);
      switch (numBytes)
      {
        case 2:
          return (uint) ByteSwap.Swap(BitConverter.ToUInt16(numArray, 0));
        case 4:
          return ByteSwap.Swap(BitConverter.ToUInt32(numArray, 0));
        default:
          return (uint) numArray[0];
      }
    }

    public void WriteStreamToDisk()
    {
      string path = Environment.CurrentDirectory + "\\searchdumps\\";
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      this.WriteStreamToDisk(path + "dump" + this.fileNumber.ToString() + ".dmp");
    }

    public void WriteStreamToDisk(string filepath)
    {
      FileStream fileStream = new FileStream(filepath, FileMode.Create);
      fileStream.Write(this.mem, 0, (int) this.endAddress - (int) this.startAddress);
      fileStream.Close();
      fileStream.Dispose();
    }
  }
}
