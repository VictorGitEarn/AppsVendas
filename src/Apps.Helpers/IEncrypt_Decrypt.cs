using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Helpers
{
    public interface IEncrypt_Decrypt
    {
        string EncryptString(string plainText);

        string DecryptString(string cipherText);
    }
}
