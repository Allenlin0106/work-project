using NUnit.Framework;
using WorkProject.Infrastructure.Security;

namespace WorkProject.Tests.Infrastructure
{
    [TestFixture]
    public class Pbkdf2PasswordHasherTests
    {
        [Test]
        public void Hash_then_Verify_with_correct_password_returns_true()
        {
            var hasher = new Pbkdf2PasswordHasher();
            var hashed = hasher.Hash("CorrectHorseBatteryStaple");
            Assert.That(hasher.Verify("CorrectHorseBatteryStaple", hashed.Hash, hashed.Salt, hashed.Iterations), Is.True);
        }

        [Test]
        public void Verify_with_wrong_password_returns_false()
        {
            var hasher = new Pbkdf2PasswordHasher();
            var hashed = hasher.Hash("CorrectHorseBatteryStaple");
            Assert.That(hasher.Verify("wrong", hashed.Hash, hashed.Salt, hashed.Iterations), Is.False);
        }

        [Test]
        public void Hash_produces_distinct_salts()
        {
            var hasher = new Pbkdf2PasswordHasher();
            var a = hasher.Hash("password");
            var b = hasher.Hash("password");
            CollectionAssert.AreNotEqual(a.Salt, b.Salt);
            CollectionAssert.AreNotEqual(a.Hash, b.Hash);
        }
    }
}
