using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitBank.Model
{
    public class StatementModel
    {
      
        public string? AccountName { get; set; }
        public DateTime Date { get; set; }
        public string? TxnId { get; set; }

        public Constant.Constant.TypeEnum TypeEnum
        {
            get;set;
        }
        public string? Amount { get; set; }
        public string? Balance { get; set; }

    }
}
