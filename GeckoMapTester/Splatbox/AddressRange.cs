namespace SplatBox
{
  public class AddressRange
  {
    private AddressType PDesc;
    private byte PId;
    private uint PLow;
    private uint PHigh;

    public AddressType description
    {
      get
      {
        return this.PDesc;
      }
    }

    public byte id
    {
      get
      {
        return this.PId;
      }
    }

    public uint low
    {
      get
      {
        return this.PLow;
      }
    }

    public uint high
    {
      get
      {
        return this.PHigh;
      }
    }

    public AddressRange(AddressType desc, byte id, uint low, uint high)
    {
      this.PId = id;
      this.PDesc = desc;
      this.PLow = low;
      this.PHigh = high;
    }

    public AddressRange(AddressType desc, uint low, uint high)
      : this(desc, (byte) (low >> 24), low, high)
    {
    }
  }
}
