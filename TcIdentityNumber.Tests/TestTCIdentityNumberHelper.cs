using System.Reflection;
using TCIdentityNumber;

namespace TcIdentityNumber.Tests
{
    [TestFixture]
    public class TestTCIdentityNumberHelper
    {
        [Test]
        public void IsIdentityNumberCorrectWithIdentityNumberDigitsLessThanEleven_ReturnFalse()
        {
            bool result = TCIdentityNumberHelper.IsIdentityNumberCorrect(111);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsIdentityNumberCorrectWithIdentityNumberDigitsGreaterThanEleven_ReturnFalse()
        {
            bool result = TCIdentityNumberHelper.IsIdentityNumberCorrect(111111111111);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsIdentityNumberCorrectWithIdentityNumberStartWithZero_ReturnFalse()
        {
            bool result = TCIdentityNumberHelper.IsIdentityNumberCorrect(0111);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsIdentityNumberCorrectAsyncWithIdentityNumberDigitsLessThanEleven_ReturnFalse()
        {
            bool result = await TCIdentityNumberHelper.IsIdentityNumberCorrectAsync(51529486516, "Ali", "Veli", 2000);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsIdentityNumberCorrectAsyncWithBirthYearDigitsLessThanFour_ReturnFalse()
        {
            bool result = await TCIdentityNumberHelper.IsIdentityNumberCorrectAsync(51529486516, "Ali", "Veli", 200);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsIdentityNumberCorrectAsyncWithEmptyNameAndSurname_ReturnFalse()
        {
            bool result = await TCIdentityNumberHelper.IsIdentityNumberCorrectAsync(51529486516, "", "", 2000);
            Assert.IsFalse(result);
        }
    }
}
