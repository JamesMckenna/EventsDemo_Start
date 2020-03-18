using DemoLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class Transactions : Form
    {
        private Customer _customer;
        private bool _denyCheckbox;
        public Transactions(Customer customer, bool denyCheckBoxIsChecked)
        {
            InitializeComponent();
            _customer = customer;

            /*
              Not convered in Tim Corey video. 
              denyOverdraft checkbox on Dashboard form Checked = true or Checked = false
              bool is passed as constructor argument from Dashboard form
             */
            _denyCheckbox = denyCheckBoxIsChecked;

            customerText.Text = _customer.CustomerName;
            _customer.CheckingAccount.OverdraftEvent += CheckingAccount_OverdraftEvent;   
        }
        
        private void CheckingAccount_OverdraftEvent(object sender, OverdraftEventArgs e)
        {   
            /*Not convered in Tim Corey video*/
            if (_denyCheckbox)
                errorMessage.Text = "Overdraft not allowed";

            /*Tim Corey code*/
            errorMessage.Visible = true;
        }

        private void makePurchaseButton_Click(object sender, EventArgs e)
        {
            bool paymentResult = _customer.CheckingAccount.MakePayment("Credit Card Purchase", amountValue.Value, _customer.SavingsAccount);
            amountValue.Value = 0;
        }

        private void errorMessage_Click(object sender, EventArgs e)
        {
            errorMessage.Visible = false;
        }

        private void Transaction_Closing(object sender, FormClosingEventArgs e)
        {
            _customer.CheckingAccount.OverdraftEvent -= CheckingAccount_OverdraftEvent;
            MessageBox.Show("_customer.CheckingAccount.OverdraftEvent removed.");
            MessageBox.Show("Events should have been removed.");
        }
    }
}
