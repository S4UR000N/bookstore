using Associated.Application.Auth.Service;

namespace Associated.Test.Application.Auth.Service
{
    public class PasswordServiceTests
    {
        [Theory]
        [InlineData("Abcd1234!", true)]  // Valid password
        [InlineData("?4321abcZ", true)]  // Valid password
        [InlineData("*4a3B2C1*", true)]  // Valid password
        [InlineData("short", false)]      // Too short
        [InlineData("no1uppercase?", false)]  // Missing uppercase
        [InlineData("NO1LOWERCASE?", false)]  // Missing lowercase
        [InlineData("NoDigitOrSpecial", false)]  // Missing digit and special character
        public void PasswordValid_ShouldReturnCorrectResult(string password, bool expected)
        {
            // Act
            bool result = PasswordService.PasswordValid(password);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashesForSameInput()
        {
            // Arrange
            string password = "Abcd1234!";

            // Act
            string hash1 = PasswordService.HashPassword(password);
            string hash2 = PasswordService.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrueForValidPassword()
        {
            // Arrange
            string password = "Abcd1234!";
            string hashedPassword = PasswordService.HashPassword(password);

            // Act
            bool result = PasswordService.VerifyPassword(hashedPassword, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalseForInvalidPassword()
        {
            // Arrange
            string correctPassword = "Abcd1234!";
            string incorrectPassword = "Abcd1234!?";
            string hashedPassword = PasswordService.HashPassword(correctPassword);

            // Act
            bool result = PasswordService.VerifyPassword(hashedPassword, incorrectPassword);

            // Assert
            Assert.False(result);
        }
    }
}
