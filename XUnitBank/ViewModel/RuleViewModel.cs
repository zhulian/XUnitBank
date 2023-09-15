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
    public class RuleViewModel
    {
        private RuleModel model = new RuleModel();
        public RuleModel? ExtractData(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string[] items = data.Split('|');
                if (items.Length == 3)
                {
                    try
                    {
                        model.Date = DateTime.ParseExact(items[0], "yyyyMMdd", CultureInfo.InvariantCulture);
                        model.RuleId = items[1];
                        model.Rate = decimal.Round(Convert.ToDecimal(items[2]), 2, MidpointRounding.AwayFromZero);
                        if (model.Rate<=0 ||model.Rate>=100)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
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
            else

            return null;

            return model;
        }

        public void GenerateResult(RuleModel rm)
        {
            if (rm != null)
            {
                if (UserConsole.Rules.Any(x => x.Date == rm.Date))
                    UserConsole.Rules.RemoveAll(x => x.Date == rm.Date);
                UserConsole.Rules.Add(rm);
 


               
            }
      
        }
    }
}
