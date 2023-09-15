using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitBank.Model;
using XUnitBank.View;

namespace XUnitBank.ViewModel
{
    public class StatementViewModel
    {

        public StatementModel? CalculateInterest(string data)
        {

            if (!string.IsNullOrEmpty(data))
            {
                string[] items = data.Split('|');
                if (items.Length == 2)
                {
                    try
                    {
                        StatementModel statement = new StatementModel();
                        statement.Date = DateTime.ParseExact(items[1], "MM", CultureInfo.InvariantCulture);
                        statement.AccountName = items[0];
                        UserConsole.BankAccounts.ForEach(x => x.AccountRecords.OrderBy(y => y.Date));
                        decimal balance = UserConsole.BankAccounts.SelectMany(x =>
                            x.AccountRecords.Where(y => y.AccountName == statement.AccountName && y.Date.Month < statement.Date.Month)).LastOrDefault().Balance;
                        List<AccountModel> tempAccount = UserConsole.BankAccounts.SelectMany(x =>
                            x.AccountRecords.Where(y => y.AccountName == statement.AccountName && y.Date.Month.Equals(statement.Date.Month))).ToList();
                        var firstDayOfMonth = new DateTime(tempAccount.Select(x => x.Date).FirstOrDefault().Year,
                            statement.Date.Month, 1); //todo: how to handle the year?
                        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);

                        List<RuleModel> tempRules = UserConsole.Rules.OrderBy(x => x.Date).ToList();
                        
                        decimal rate = 0;
                        if (tempAccount.Count > 0 && tempRules.Count > 0 && tempRules[0].Date > firstDayOfMonth)
                        {
                            throw new DataException("Need to define interest rule before transactions.");
                            return null;
                        }
                        else
                        {
                            rate = tempRules.Where(x => x.Date <= firstDayOfMonth).Select(y => y.Rate).LastOrDefault();
                        }
                        
                       
                        tempRules = UserConsole.Rules.OrderBy(x => x.Date).Where(y => y.Date.Month.Equals(firstDayOfMonth.Month)).ToList();


                        int days = 0;
                      
                        decimal interest = 0;
                     
                        for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1.0))
                        {
                           
                            for (int i = 0; i < tempAccount.Count; i++)
                            {
                                for (int j = 0; j < tempRules.Count; j++)
                                {
                                    //find latest interest
                                    if (tempRules[j].Date.Day.Equals(date.Day))
                                    {
                                        interest += days * rate / 100 * balance;
                                        rate = tempRules[j].Rate;
                                        days = 0;
                                    }

                                }

                                if (tempAccount[i].Date.Day.Equals(date.Day))
                                {
                                    //calculate previous interest
                                    interest += days * rate / 100 * balance;
                                    balance = tempAccount[i].Balance;
                                    days = 0;
                                

                                }

                                if (date.Day.Equals(lastDayOfMonth.Day))
                                {
                                    interest += days==0?0:++days * rate / 100 * balance;
                                    days = 0;
                                }

                            }

                            days++;
                        }

                        statement.Date = lastDayOfMonth;
                        statement.TypeEnum = Constant.Constant.TypeEnum.I;
                        statement.Balance = (tempAccount.LastOrDefault().Balance + interest/365).ToString("F");
                        statement.Amount = ((interest/365).ToString("F"));

                        return statement;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return null;
                    }


                }
            }

            return null;

        }
    }
}
