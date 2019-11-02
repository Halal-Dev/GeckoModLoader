using System;

namespace SplatBox
{
  public class ETCPGeckoException : Exception
  {
    private ETCPErrorCode PErrorCode;

    public ETCPErrorCode ErrorCode
    {
      get
      {
        return this.PErrorCode;
      }
    }

    public ETCPGeckoException(ETCPErrorCode code)
    {
      this.PErrorCode = code;
    }

    public ETCPGeckoException(ETCPErrorCode code, string message)
      : base(message)
    {
      this.PErrorCode = code;
    }

    public ETCPGeckoException(ETCPErrorCode code, string message, Exception inner)
      : base(message, inner)
    {
      this.PErrorCode = code;
    }
  }
}
