using System;
using Xunit;

namespace Solitons.Security.Azure
{
    // ReSharper disable once InconsistentNaming
    public sealed class AzureBatchAccountSettingsGroup_Parse_Should
    {

        [Theory]
        [InlineData("Account=account1;Location=westeurope;Key=e6bCebsUSb5fkASI0K3qLVdntO4AcB08a1j0s9aPoM4H4Ah6L548dOLHQPRwP5uo9FBZZxGWxbOw+ABavBGbtw==", "account1", "westeurope", "e6bCebsUSb5fkASI0K3qLVdntO4AcB08a1j0s9aPoM4H4Ah6L548dOLHQPRwP5uo9FBZZxGWxbOw+ABavBGbtw==", null)]
        [InlineData("Account=account2;Location=centralus;Key=dLL22VQsrw81NNvJfvee+b5SWv7N4E51wHbVhxSeVd1LR71dC+PMzh0q4NXBDreZYjUmSpQBzn0p+ABayWUfKg==", "account2", "centralus", "dLL22VQsrw81NNvJfvee+b5SWv7N4E51wHbVhxSeVd1LR71dC+PMzh0q4NXBDreZYjUmSpQBzn0p+ABayWUfKg==", null)]
        [InlineData("Account=account2;Location=centralus;Key=dLL22VQsrw81NNvJfvee+b5SWv7N4E51wHbVhxSeVd1LR71dC+PMzh0q4NXBDreZYjUmSpQBzn0p+ABayWUfKg==;principal=4002d9a9-bc4d-419a-8b71-95e2a2180427", "account2", "centralus", "dLL22VQsrw81NNvJfvee+b5SWv7N4E51wHbVhxSeVd1LR71dC+PMzh0q4NXBDreZYjUmSpQBzn0p+ABayWUfKg==", "4002d9a9-bc4d-419a-8b71-95e2a2180427")]
        public void HandleValidInputs(string input, string account, string location, string key, string? principalIdText)
        {
            var principalId = principalIdText.IsPrintable() ? Guid.Parse(principalIdText!) : default(Guid?);
            var settings = AzureBatchAccountSettingsGroup.Parse(input)!;
            Assert.Equal(account, settings.Account, StringComparer.Ordinal);
            Assert.Equal(location, settings.Location, StringComparer.Ordinal);
            Assert.Equal(key, settings.Key, StringComparer.Ordinal);
            Assert.Equal(principalId, settings.PrincipalId);
        }

        [Theory]
        [InlineData("Account=AcCoUnT1;Location=westeurope;Key=e6bCebsUSb5fkASI0K3qLVdntO4AcB08a1j0s9aPoM4H4Ah6L548dOLHQPRwP5uo9FBZZxGWxbOw+ABavBGbtw==")]
        [InlineData("Account=account1;Location=loc;Key=e6bCebsUSb5fkASI0K3qLVdntO4AcB08a1j0s9aPoM4H4Ah6L548dOLHQPRwP5uo9FBZZxGWxbOw+ABavBGbtw==")]
        [InlineData("Account=account1;Location=westeurope;Key=:-)")]
        [InlineData(":-)")]
        public void ThrowFormatExceptionOnInvalidInput(string input)
        {
            Assert.Throws<FormatException>(() => AzureBatchAccountSettingsGroup.Parse(input));
        }

    }
}
