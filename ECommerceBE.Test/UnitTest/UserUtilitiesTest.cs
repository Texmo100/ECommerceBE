using ECommerceBE.Controllers.Utilities;
using NUnit.Framework;

namespace ECommerceBE.Test
{
    public class UserUtilitiesTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestHashPassword()
        {
            //Arrange
            string testPassword = "TestPass";
            //Act
            var hashPassword1 = UserUtilities.hashPassword(testPassword);
            var hashPassword2 = UserUtilities.hashPassword(testPassword);
            var doubleHashPassword = UserUtilities.hashPassword(hashPassword1);
            //Assert
            Assert.IsInstanceOf<string>(hashPassword1,"The result of a hashPassword must be a string");
            Assert.AreNotEqual(testPassword, hashPassword1, "The input password can't be the same as the hashPassword");
            Assert.AreNotEqual(doubleHashPassword, hashPassword1, "The hashPassword can't be a valid password");
            Assert.AreEqual(hashPassword1, hashPassword2, "The hashPasswords must be the same if the input is the same");
        }
    }
}