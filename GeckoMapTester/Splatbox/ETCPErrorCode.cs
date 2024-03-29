﻿namespace SplatBox
{
  public enum ETCPErrorCode
  {
    FTDIQueryError,
    noFTDIDevicesFound,
    noTCPGeckoFound,
    FTDIResetError,
    FTDIPurgeRxError,
    FTDIPurgeTxError,
    FTDITimeoutSetError,
    FTDITransferSetError,
    FTDICommandSendError,
    FTDIReadDataError,
    FTDIInvalidReply,
    TooManyRetries,
    REGStreamSizeInvalid,
    CheatStreamSizeInvalid,
  }
}
