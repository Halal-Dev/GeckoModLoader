using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class StringCipher
{
  private const int Keysize = 256;
  private const int DerivationIterations = 1000;

  public static string Decrypt(string cipherText, string passPhrase)
  {
    byte[] numArray1 = Convert.FromBase64String(cipherText);
    byte[] array1 = ((IEnumerable<byte>) numArray1).Take<byte>(32).ToArray<byte>();
    byte[] array2 = ((IEnumerable<byte>) numArray1).Skip<byte>(32).Take<byte>(32).ToArray<byte>();
    byte[] array3 = ((IEnumerable<byte>) numArray1).Skip<byte>(64).Take<byte>(numArray1.Length - 64).ToArray<byte>();
    using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, array1, 1000))
    {
      byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
      using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
      {
        rijndaelManaged.BlockSize = 256;
        rijndaelManaged.Mode = CipherMode.CBC;
        rijndaelManaged.Padding = PaddingMode.PKCS7;
        using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, array2))
        {
          using (MemoryStream memoryStream = new MemoryStream(array3))
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
            {
              byte[] numArray2 = new byte[array3.Length];
              int count = cryptoStream.Read(numArray2, 0, numArray2.Length);
              memoryStream.Close();
              cryptoStream.Close();
              return Encoding.UTF8.GetString(numArray2, 0, count);
            }
          }
        }
      }
    }
  }

  private static byte[] Generate256BitsOfRandomEntropy()
  {
    byte[] data = new byte[32];
    using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
      cryptoServiceProvider.GetBytes(data);
    return data;
  }
}
