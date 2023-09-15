using System;
using XUnitBank.Model;

namespace XUnitBank.ViewModel
{
    /// <summary>
    /// Bank account demo class.
    /// </summary>
    public class BankAccount
    {
        public Dictionary<DateTime, int> RunningNumberDic = new Dictionary<DateTime, int>();
        private decimal _balance;
        public List<AccountModel> AccountRecords = new List<AccountModel>();


        public BankAccount()
        {

        }


        public BankAccount(decimal balance)
        {
            _balance = balance;
        }

        public decimal Balance
        {
            get { return _balance; }
        }

        public void Add(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > _balance)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            _balance -= amount;
        }

        public void TransferFundsTo(BankAccount otherAccount, decimal amount)
        {
            if (otherAccount is null)
            {
                throw new ArgumentNullException(nameof(otherAccount));
            }

            Withdraw(amount);
            otherAccount.Add(amount);
        }
    }
}