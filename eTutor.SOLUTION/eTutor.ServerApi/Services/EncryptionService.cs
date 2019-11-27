using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Contracts;

namespace eTutor.ServerApi.Services
{
    /// <inheritdoc />
    public sealed class EncryptionService : IEncryptionService
    {


        /// <inheritdoc />
        public string EncryptWithAes(string normalStr)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string DecryptWithAes(string encryptedStr)
        {
            throw new NotImplementedException();
        }
    }
}
