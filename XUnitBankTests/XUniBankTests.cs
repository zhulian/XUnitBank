
using System;
using System.Collections.Immutable;
using System.Globalization;
using Microsoft.VisualBasic;
using Xunit;
using XUnitBank;
using XUnitBank.Model;
using XUnitBank.View;
using XUnitBank.ViewModel;

namespace XUnitBankTests
{
    public class BankAccountTests
    {
        [Fact]
        public void Adding_Funds_Updates_Balance()
        {
            // ARRANGE
            var account = new BankAccount(1000);

            // ACT
            account.Add(100);

            // ASSERT
            Assert.Equal(1100, account.Balance);
        }

        [Fact]
        public void Adding_Negative_Funds_Throws()
        {
            // ARRANGE
            var account = new BankAccount(1000);

            // ACT + ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => account.Add(-100));
        }

        [Fact]
        public void Withdrawing_Funds_Updates_Balance()
        {
            // ARRANGE
            var account = new BankAccount(1000);

            // ACT
            account.Withdraw(100);

            // ASSERT
            Assert.Equal(900, account.Balance);
        }

        [Fact]
        public void Withdrawing_Negative_Funds_Throws()
        {
            // ARRANGE
            var account = new BankAccount(1000);

            // ACT + ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => account.Withdraw(-100));
        }

        [Fact]
        public void Withdrawing_More_Than_Funds_Throws()
        {
            // ARRANGE
            var account = new BankAccount(1000);

            // ACT + ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => account.Withdraw(2000));
        }

        [Fact]
        public void Adding_Same_Rules_Replace_Old()
        {
            // ARRANGE
            UserConsole.Rules.Add(new RuleModel() { Date = DateTime.ParseExact("20230101", "yyyyMMdd", CultureInfo.InvariantCulture), RuleId = "RULE01", Rate = decimal.Round(Convert.ToDecimal("1.95"), 2, MidpointRounding.AwayFromZero) });
            // ACT 
            UserConsole.Rules.Add(new RuleModel() { Date = DateTime.ParseExact("20230101", "yyyyMMdd", CultureInfo.InvariantCulture), RuleId = "RULE02", Rate = decimal.Round(Convert.ToDecimal("2.25"), 2, MidpointRounding.AwayFromZero) });
            //ASSERT
            Assert.DoesNotContain(new RuleModel() { Date = DateTime.ParseExact("20230101", "yyyyMMdd", CultureInfo.InvariantCulture), RuleId = "RULE01", Rate = decimal.Round(Convert.ToDecimal("1.95"), 2, MidpointRounding.AwayFromZero)},UserConsole.Rules);
        }

    }
}