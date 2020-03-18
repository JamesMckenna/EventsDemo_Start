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
    public partial class Dashboard : Form
    {
        Customer customer = new Customer();

        public Dashboard()
        {
            InitializeComponent();

            LoadTestingData();

            WireUpForm();
        }

        private void LoadTestingData()
        {
            customer.CustomerName = "Tim Corey";
            customer.CheckingAccount = new Account();
            customer.SavingsAccount = new Account();

            customer.CheckingAccount.AccountName = "Tim's Checking Account";
            customer.SavingsAccount.AccountName = "Tim's Savings Account";

            customer.CheckingAccount.AddDeposit("Initial Balance", 155.43M);
            customer.SavingsAccount.AddDeposit("Initial Balance", 98.45M);
        }

        private void WireUpForm()
        {
            customerText.Text = customer.CustomerName;
            checkingTransactions.DataSource = customer.CheckingAccount.Transactions;
            savingsTransactions.DataSource = customer.SavingsAccount.Transactions;
            checkingBalanceValue.Text = string.Format("{0:C2}", customer.CheckingAccount.Balance);
            savingsBalanceValue.Text = string.Format("{0:C2}", customer.SavingsAccount.Balance);

            /*
             * Listening for event
                The += implies we can add to or subtract (-=) from the event
                this has to do with removing Event before the app close/shuts down. 
                If we don't remove events, we could have memory leak issues.
                So, it is best to use named functions rather than anonymous functions. If name, it is a lot easier to clean them up and remove.
             */
                                                            /*Function Name - allows easy removal and better garbrage collection by removing the events*/
            customer.CheckingAccount.TransactionApprovedEvent += CheckingAccount_TransactionApprovedEvent;
            customer.SavingsAccount.TransactionApprovedEvent += SavingsAccount_TransactionApprovedEvent;

            /*Listening for when overdraft withdraw gets called*/
            customer.CheckingAccount.OverdraftEvent += CheckingAccount_OverdraftEvent;
        }

        private void CheckingAccount_OverdraftEvent(object sender, OverdraftEventArgs e)
        {
            /*Label on Banking Demo Dashboard Form*/
            errorMessage.Text = $"You had an Overdraft protection transfer of {string.Format("{0:C2}", e.AmountOverdrafted)}";

            /*still showing overdraft attempt, but don't allow it to go through*/
            e.CancelTransaction = denyOverdraft.Checked; /* see if conditional in Account.cs line 69*/
            if (e.CancelTransaction)
            {
                errorMessage.Text = "You do not have enough funds in your checking account and you have chosen not to use overdraft.";
            }
            errorMessage.Visible = true;
        }

        /*
            In the above method we are listening for an event. An event can be listened to in multiple methods
            We declared this event in Account.cs
            Also, in Account.cs we told the event to Fire after the Account Balance has been changed
        */
        private void CheckingAccount_TransactionApprovedEvent(object sender, string e)
        {
            checkingTransactions.DataSource = null;
            checkingTransactions.DataSource = customer.CheckingAccount.Transactions;
            checkingBalanceValue.Text = string.Format("{0:C2}", customer.CheckingAccount.Balance);
        }

        private void SavingsAccount_TransactionApprovedEvent(object sender, string e)
        {
            savingsTransactions.DataSource = null;
            savingsTransactions.DataSource = customer.SavingsAccount.Transactions;
            savingsBalanceValue.Text = string.Format("{0:C2}", customer.SavingsAccount.Balance);
        }

        private void recordTransactionsButton_Click(object sender, EventArgs e)
        {
            /*
              Not covered in Tim Corey video
              When Record Tranactions button is clicked, Transactions Form opens.
              This method declares and intializes the Transactions Form (window)
              Upon doing so, we pass the customer as an argument. So this is where I also pass access to Dashboard controls to 
              Transactions form. I should be able to pass the whole Dashboard Form with the keyword "this" (not sure)
              In this case, I only want to pass the denyOverdraft checkbox control to see whether is is checked or not.
              If checked, I want to change the default errorMessage shown on the Transaction Form.
            */
            
            //int denyCheckboxIndex = this.Controls.IndexOf(denyOverdraft);
            //Control denyCheckboxControl = this.denyOverdraft;
            Transactions transactions = new Transactions(customer, this.denyOverdraft.Checked);
            transactions.Show();
        }

        private void errorMessage_Click(object sender, EventArgs e)
        {
            errorMessage.Visible = false;
        }

        private void denyOverdraft_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkingTransactionsLabel_Click(object sender, EventArgs e)
        {

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        /*
            Tim Corey talked about removing events on close but didn't really show how
            This is my attempt at adding the functionality.
        */
        private void Dashboard_Closing(object sender, FormClosingEventArgs e)
        {
            customer.CheckingAccount.TransactionApprovedEvent -= CheckingAccount_TransactionApprovedEvent;
            MessageBox.Show("customer.CheckingAccount.TransactionApprovedEvent should have been removed.");

            customer.SavingsAccount.TransactionApprovedEvent -= SavingsAccount_TransactionApprovedEvent;
            MessageBox.Show("customer.SavingsAccount.TransactionApprovedEvent should have been removed.");

            customer.CheckingAccount.OverdraftEvent -= CheckingAccount_OverdraftEvent;
            MessageBox.Show("customer.CheckingAccount.OverdraftEvent should have been removed.");
            MessageBox.Show("Events should have been removed.");
        }
    }
}
