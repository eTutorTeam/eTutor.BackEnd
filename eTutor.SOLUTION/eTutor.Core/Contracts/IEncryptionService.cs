using System;
using System.Collections.Generic;
using System.Text;

namespace eTutor.Core.Contracts
{
    public interface IEncryptionService
    {
        string EncryptWithAes(string normalStr);

        string DecryptWithAes(string encryptedStr);
    }
}
