using System;
using System.Security.Cryptography;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Services;

namespace WorkProject.Infrastructure.Security
{
    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        private const int SaltBytes = 16;
        private const int HashBytes = 32;
        private const int DefaultIterations = 100_000;

        public HashedPassword Hash(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            var salt = new byte[SaltBytes];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, DefaultIterations, HashAlgorithmName.SHA256))
            {
                return new HashedPassword
                {
                    Hash = pbkdf2.GetBytes(HashBytes),
                    Salt = salt,
                    Iterations = DefaultIterations
                };
            }
        }

        public bool Verify(string password, byte[] hash, byte[] salt, int iterations)
        {
            if (password == null || hash == null || salt == null) return false;

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                var test = pbkdf2.GetBytes(hash.Length);
                return FixedTimeEquals(test, hash);
            }
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
