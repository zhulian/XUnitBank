using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using XUnitBank.Model;
using XUnitBank.ViewModel;

namespace XUnitBank.View
{
    public static class UserConsole
    {
        
        public static List<BankAccount> BankAccounts = new List<BankAccount>();
        public static List<RuleModel> Rules = new List<RuleModel>();
        public static void Run()
        {

            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            InitSystem();
        start: string? sOpt = MainMenu();
            if (!string.IsNullOrEmpty(sOpt))
            {
                switch (sOpt.ToLower()
                       )
                {
                    case "i":
                        Console.WriteLine(
                            "Please enter transaction details in <Date>|<Account>|<Type>|<Amount> format\r\n(or enter blank to go back to main menu):");
                        string? s1 = Console.ReadLine();
                        if (!string.IsNullOrEmpty(s1))
                        {
                            
                            //make transaction
                            AccountViewModel accountViewModel = new AccountViewModel();
                            AccountModel account = accountViewModel.ExtractData(s1);
                            accountViewModel.GenerateResult(account);
                            //response
                            Console.WriteLine("");
                            Console.WriteLine("Account: " + account.AccountName);
                            Console.WriteLine("Date     | Txn Id      | Type | Amount |");
                            foreach (BankAccount bankAccount in BankAccounts.Where(x => x.AccountRecords.Any(y => y.AccountName.Equals(account.AccountName))))
                            {
                                bankAccount.AccountRecords.ForEach(x => Console.WriteLine(x.Date.ToString("yyyyMMdd") + " | " + string.Format("{0:00}", x.TxnId) + " | " + x.TypeTrans.ToString() + "    | " + x.Amount.ToString("F") + " |"));

                            }

                            Console.WriteLine("");
                            Console.WriteLine("Is there anything else you'd like to do?");
                            goto start;

                        }

                        goto start;

                        break;


                    case "d":
                        Console.WriteLine(
                            "Please enter interest rules details in <Date>|<RuleId>|<Rate in %> format \r\n(or enter blank to go back to main menu):");
                        string? s2 = Console.ReadLine();
                        if (!string.IsNullOrEmpty(s2))
                        {
                            //define interest rules for all accounts

                            RuleViewModel ruleViewModel = new RuleViewModel();
                            RuleModel rm = ruleViewModel.ExtractData(s2);
                            ruleViewModel.GenerateResult(rm);
                            //response
                            Console.WriteLine("");
                            Console.WriteLine("Interest rules: ");
                            Console.WriteLine("Date     | RuleId | Rate (%) |");

                            UserConsole.Rules.ForEach(x => Console.WriteLine(x.Date.ToString("yyyyMMdd") + " | " + x.RuleId + " |     " + string.Format("{0:0.00}", x.Rate) + " |"));

                            Console.WriteLine("");
                            Console.WriteLine("Is there anything else you'd like to do?");
                            goto start;

                        }

                        goto start;


                        break;
                    case "p":
                        Console.WriteLine(
                            "Please enter account and month to generate the statement <Account>|<Month>\r\n(or enter blank to go back to main menu):");
                        string? s3 = Console.ReadLine();
                        if (!string.IsNullOrEmpty(s3))
                        {
                            //Print statement
                           StatementViewModel statementViewModel = new StatementViewModel();
                           StatementModel statement= statementViewModel.CalculateInterest(s3);
                           if (statement!=null)
                           {
                               //response
                               Console.WriteLine("");
                               Console.WriteLine("Account: ");
                               Console.WriteLine("Date     | Txn Id      | Type | Amount | Balance |");
                               string[] items = s3.Split('|');
                               BankAccounts.SelectMany(x =>
                                       x.AccountRecords.Where(y =>
                                           y.AccountName.Equals(items[0]) && y.Date.Month.Equals(DateTime
                                               .ParseExact(s3.Split('|')[1], "MM", CultureInfo.InvariantCulture)
                                               .Month)))
                                   .ToList().ForEach(
                                       x => Console.WriteLine(x.Date.ToString("yyyyMMdd") + " | " +
                                                              string.Format("{0:00}", x.TxnId) + " | " + x.TypeTrans +
                                                              "    | " + x.Amount.ToString("F") + " |" +
                                                              x.Balance.ToString("F")+" |"));
                                //foreach (BankAccount bankAccount in BankAccounts.Where(x => x.AccountRecords.Any(y => y.AccountName.Equals(s3.Split('|')[0]) && y.Date.Month.Equals(DateTime.ParseExact(s3.Split('|')[1], "MM", CultureInfo.InvariantCulture).Month))))
                                //{
                                //    bankAccount.AccountRecords.ForEach(x => Console.WriteLine(x.Date.ToString("yyyyMMdd") + " | " + string.Format("{0:00}", x.TxnId) + " | " + x.TypeTrans + "    | " + x.Amount.ToString("F") + " |" + x.Balance.ToString("F")));

                                //}
                                //print interest
                                Console.WriteLine(statement.Date.ToString("yyyyMMdd") + " |             | " + statement.TypeEnum + "    | " + statement.Amount.Trim().PadLeft(9-statement.Amount.Trim().Length) + " |" + statement.Balance.Trim().PadLeft(9-statement.Balance.Length)+" |");
                         
                                Console.WriteLine("");
                               Console.WriteLine("Is there anything else you'd like to do?");
                               goto start;
                            }

                           Console.WriteLine("Interest calculation error.");

                        }
                        else
                        {
                            goto start;
                        }
                       
                        break;
                    case "q":
                        Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\r\nHave a nice day!");
                        //quit
                        Environment.Exit(0);

                        break;
                    default:
                        goto start;
                        return;
                }

            }
        }

        private static void InitSystem()
        {
           List<AccountModel> accounts = new List<AccountModel>();
           accounts.Add(new AccountModel(){AccountName = "AC001",Amount = 100,Balance = 100,Date = DateTime.ParseExact("20230505", "yyyyMMdd", CultureInfo.InvariantCulture),TxnId = "20230505-01",TypeTrans = Constant.Constant.TypeEnum.D});
           accounts.Add(new AccountModel() { AccountName = "AC001", Amount = 150, Balance = 250, Date = DateTime.ParseExact("20230601", "yyyyMMdd", CultureInfo.InvariantCulture), TxnId = "20230601-01", TypeTrans = Constant.Constant.TypeEnum.D });
           accounts.Add(new AccountModel() { AccountName = "AC001", Amount = 20, Balance = 230, Date = DateTime.ParseExact("20230626", "yyyyMMdd", CultureInfo.InvariantCulture), TxnId = "20230626-01", TypeTrans = Constant.Constant.TypeEnum.W });
           accounts.Add(new AccountModel() { AccountName = "AC001", Amount = 100, Balance = 130, Date = DateTime.ParseExact("20230626", "yyyyMMdd", CultureInfo.InvariantCulture), TxnId = "20230626-02", TypeTrans = Constant.Constant.TypeEnum.W });
BankAccounts.Add(new BankAccount(){AccountRecords = accounts});
Rules.Add(new RuleModel(){Date = DateTime.ParseExact("20230101", "yyyyMMdd", CultureInfo.InvariantCulture), RuleId = "RULE01",Rate = decimal.Round(Convert.ToDecimal("1.95"), 2, MidpointRounding.AwayFromZero)});
Rules.Add(new RuleModel() { Date = DateTime.ParseExact("20230520", "yyyyMMdd", CultureInfo.InvariantCulture), RuleId = "RULE02", Rate = decimal.Round(Convert.ToDecimal("1.90"), 2, MidpointRounding.AwayFromZero) });
Rules.Add(new RuleModel() { Date = DateTime.ParseExact("20230615", "yyyyMMdd", CultureInfo.InvariantCulture), RuleId = "RULE03", Rate = decimal.Round(Convert.ToDecimal("2.20"), 2, MidpointRounding.AwayFromZero) });


        }


        private static string? MainMenu()
        {

            Console.WriteLine("[I]nput transactions");
            Console.WriteLine("[D]efine interest rules");
            Console.WriteLine("[P]rint statement");
            Console.WriteLine("[Q]uit");
            return Console.ReadLine();
        }

    }
}
