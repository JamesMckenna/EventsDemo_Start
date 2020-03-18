using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary
{
    /*
     * Inherting from EventArgs class
     * An empty class with a constructor
     * has a public property that's static readonly
     * allows us to pass an empty argument into the Invoke(this, EventArgs.Empty) 2nd param
     */
    public class OverdraftEventArgs : EventArgs 
    {
        public decimal AmountOverdrafted { get; private set; }
        public string MoreInfo { get; private set; }

        /*
         When not to use ReadOnly/private set
         see implementation in Dashboard CheckingAccount_OverdraftEvent method
       */
        public bool CancelTransaction { get; set; } = false;

        public OverdraftEventArgs(decimal amountOverdrafted, string moreInfo)
        {
            AmountOverdrafted = amountOverdrafted;
            MoreInfo = moreInfo;
        }
    }
}
