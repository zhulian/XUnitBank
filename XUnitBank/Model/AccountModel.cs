using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitBank.ViewModel;

namespace XUnitBank.Model
{
    public class AccountModel
    {
        
        public DateTime Date { get; set; }
        public string? AccountName { get; set; }
        public Constant.Constant.TypeEnum TypeTrans { get; set; }
        public string? TxnId { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
      
    }
}
