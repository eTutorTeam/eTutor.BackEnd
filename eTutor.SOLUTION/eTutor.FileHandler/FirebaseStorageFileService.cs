using System;
using eTutor.Core.Contracts;
using eTutor.Core.Models.Configuration;

namespace eTutor.FileHandler
{
    public sealed class FirebaseStorageFileService : IFileService
    {
        private readonly FirebaseConfiguration _firebaseConfiguration;

        public FirebaseStorageFileService(FirebaseConfiguration firebaseConfiguration)
        {
            _firebaseConfiguration = firebaseConfiguration;
        }
    }
}