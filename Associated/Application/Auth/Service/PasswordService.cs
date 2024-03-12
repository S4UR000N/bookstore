using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Associated.Application.Auth.Service
{
    public static class PasswordService
    {
        private static readonly int IterationCount = 10000;
        private static readonly int SaltSize = 128 / 8; // 128 bits

        public static bool PasswordValid(string password)
        {
            return
                password.Length >= 8 &&
                password.Any(char.IsLower) &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsDigit) &&
                password.Any(c => !char.IsLetterOrDigit(c));
        }

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: IterationCount,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        public static bool VerifyPassword(string hashedPassword, string passwordToVerify)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                throw new InvalidOperationException("Invalid hashed password format");
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            string storedHashedPassword = parts[1];

            string hashedPasswordToVerify = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToVerify,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: IterationCount,
                numBytesRequested: 256 / 8));

            return string.Equals(storedHashedPassword, hashedPasswordToVerify);
        }
    }
}
