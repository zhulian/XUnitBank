using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitBank.Model;
using XUnitBank.View;

namespace XUnitBank.ViewModel
{
    public class AccountViewModel
    {
        public AccountModel? ExtractData(string data)
        {
            AccountModel model = new AccountModel();
            if (!string.IsNullOrEmpty(data))
            {
                //20230626|AC001|W|100.00
                string[] items = data.Split('|');
                if (items.Length==4)
                {

                    try
                    {
                        model.Date = DateTime.ParseExact(items[0], "yyyyMMdd", CultureInfo.InvariantCulture);
                        model.AccountName = items[1];
                        Constant.Constant.TypeEnum type;
                        var boolParse = Enum.TryParse(items[2], out type);
                        model.TypeTrans = boolParse ? type : throw new FormatException("Wrong account type");
                        model.Amount= decimal.Round(Convert.ToDecimal(items[3]), 2, MidpointRounding.AwayFromZero); 
                      
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return null;
                    }
                   
                }
                else
                {
                    return null;
                }

            }

            return model;
        }

        public void GenerateResult(AccountModel? account)
        {
            if (account != null)
            {
                BankAccount ba = null;
                if (UserConsole.BankAccounts.Any(x => x.AccountRecords.Any(y => y.AccountName.Equals(account.AccountName))))
                {
                    ba = UserConsole.BankAccounts.Find(x =>
                       x.AccountRecords.Any(y => y.AccountName == account.AccountName));
                    if (ba.RunningNumberDic.ContainsKey(account.Date))
                    {
                        ba.RunningNumberDic[account.Date]++;

                    }
                    else
                    {
                        ba.RunningNumberDic.Add(account.Date, 1);

                        ;
                    }
                    account.TxnId = account.Date.ToString("yyyyMMdd") + "-" +
                                    string.Format("{0:00}", ba.RunningNumberDic[account.Date]);
                    switch (account.TypeTrans)
                    {
                        case Constant.Constant.TypeEnum.D:
                            ba.Add(account.Amount);
                            break;
                        case Constant.Constant.TypeEnum.W:
                            ba.Withdraw(account.Amount);
                            break;
                    }

                    account.Balance = ba.Balance;

                    ba.AccountRecords.Add(account);
                }
                else
                {
                    ba = new BankAccount();
                    if (ba.RunningNumberDic.ContainsKey(account.Date))
                    {
                        ba.RunningNumberDic[account.Date]++;

                    }
                    else
                    {
                        ba.RunningNumberDic.Add(account.Date, 1);

                        ;
                    }
                    account.TxnId = account.Date.ToString("yyyyMMdd") + "-" +
                                    string.Format("{0:00}", ba.RunningNumberDic[account.Date]);
                    switch (account.TypeTrans)
                    {
                        case Constant.Constant.TypeEnum.D:
                            ba.Add(account.Amount);
                            break;
                        case Constant.Constant.TypeEnum.W:
                            ba.Withdraw(account.Amount);
                            break;
                    }

                    account.Balance = ba.Balance;

                    ba.AccountRecords.Add(account);
                    UserConsole.BankAccounts.Add(ba);
                }

            }

    
        }
    }
}
