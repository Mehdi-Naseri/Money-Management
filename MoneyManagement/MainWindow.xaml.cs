using MoneyManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Initialize Components
            dataGridAccounts.IsReadOnly = true;
            dataGridTransactions.IsReadOnly = true;
            datePickerTransaction.SelectedDate = DateTime.Today;
            textBoxTransactionDescription.Text = null;
            textBoxTransactionMoney.Text = null;
            //Fill DataGrids
            using (var Database1Entities1Ins = new Data.Database1Entities())
            {
                //Fill Accounts DataGrid
                dataGridAccounts.ItemsSource = Database1Entities1Ins.Accounts.ToList();
                //Fill Transactions DataGrid
                var qTransactions = from p in Database1Entities1Ins.Transactions
                                    join pAccounts in Database1Entities1Ins.Accounts on p.AccountID equals pAccounts.ID
                                    select p;
                //select new { p.ID, Bank = string.Format("{0}:{1}",pAccounts.ID , pAccounts.BankName), p.Change, p.Money, p.Description, p.Date };
                dataGridTransactions.ItemsSource = qTransactions.ToList();

                comboBoxAccounts.Items.Clear();
                comboBoxAccountsReminder.Items.Clear();
                foreach (var a in Database1Entities1Ins.Accounts)
                {
                    comboBoxAccounts.Items.Add(a.BankName);
                    comboBoxAccountsReminder.Items.Add(a.BankName);
                }
                //Fill Reminder DataGrid
                calendar1.SelectedDate = DateTime.Now;
                //datePicker1.SelectedDate = calendar1.SelectedDate;
                //datePicker1.Text = calendar1.SelectedDate.ToString();

                dataGridReminder.ItemsSource = Database1Entities1Ins.Reminders.ToList();
            }
        }

        # region ::::::::::::::::::::::::::::::::::::::::: Accounts :::::::::::::::::::::::::::::::::::::::::::::::::
        private void buttonAddAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Insert
                using (var Database1Entities1Ins = new Data.Database1Entities())
                {
                    //Add new Account
                    int intAccountNumber;
                    if (int.TryParse(textBoxAccountNumber.Text, out intAccountNumber) == false)
                    {
                        intAccountNumber = 0;
                    }
                    var AccountNew = new Account() { BankName = textBoxAccountBank.Text, AcountNumber = intAccountNumber, CardNumber = textBoxAccountCardnumber.Text };
                    Database1Entities1Ins.Accounts.Add(AccountNew);
                    Database1Entities1Ins.SaveChanges();
                    //Show aaccounts in DataGrid and comboboxes
                    dataGridAccounts.ItemsSource = Database1Entities1Ins.Accounts.ToList();
                    dataGridAccounts.Items.Refresh();
                    comboBoxAccounts.Items.Clear();
                    comboBoxAccountsReminder.Items.Clear();
                    foreach (var a in Database1Entities1Ins.Accounts)
                    {
                        comboBoxAccounts.Items.Add(a.BankName);
                        comboBoxAccountsReminder.Items.Add(a.BankName);
                    }
                    //Clear TextBoxes
                    textBoxAccountBank.Text = null;
                    textBoxAccountCardnumber.Text = null;
                    textBoxAccountNumber.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGridAccounts.SelectedItem != null)
                {
                    MoneyManagement.Data.Account selectedItem1 = (MoneyManagement.Data.Account)dataGridAccounts.SelectedItem;
                    //Delete
                    using (var Database1Entities1Ins = new Data.Database1Entities())
                    {
                        var q = from a in Database1Entities1Ins.Accounts
                                where a.ID == selectedItem1.ID
                                select a;
                        foreach (var a in q)
                        {
                            Database1Entities1Ins.Accounts.Remove(a);
                        }
                        Database1Entities1Ins.SaveChanges();
                        //Show aaccounts in DataGrid
                        var q1 = from p in Database1Entities1Ins.Accounts
                                 select p;
                        var results = q1.ToList();
                        dataGridAccounts.ItemsSource = results;
                        dataGridAccounts.Items.Refresh();
                        comboBoxAccounts.Items.Clear();
                        comboBoxAccountsReminder.Items.Clear();
                        foreach (var a in Database1Entities1Ins.Accounts)
                        {
                            comboBoxAccounts.Items.Add(a.BankName);
                            comboBoxAccountsReminder.Items.Add(a.BankName);
                        }
                        //Clear TextBoxes
                        textBoxAccountBank.Text = null;
                        textBoxAccountCardnumber.Text = null;
                        textBoxAccountNumber.Text = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonEditAccount_Click(object sender, RoutedEventArgs e)
        {
            //Update
            //Database1Entities1.SaveChanges();
            try
            {
                if (dataGridAccounts.SelectedItem != null)
                {
                    MoneyManagement.Data.Account selectedItem1 = (MoneyManagement.Data.Account)dataGridAccounts.SelectedItem;
                    //Delete
                    using (var Database1Entities1Ins = new Data.Database1Entities())
                    {
                        var q = from a in Database1Entities1Ins.Accounts
                                where a.ID == selectedItem1.ID
                                select a;
                        foreach (var a in q)
                        {
                            a.BankName = textBoxAccountBank.Text;
                            a.AcountNumber = int.Parse(textBoxAccountNumber.Text);
                            a.CardNumber = textBoxAccountCardnumber.Text;
                        }
                        Database1Entities1Ins.SaveChanges();
                        //Show aaccounts in DataGrid
                        var q1 = from p in Database1Entities1Ins.Accounts
                                 select p;
                        var results = q1.ToList();
                        dataGridAccounts.ItemsSource = results;
                        dataGridAccounts.Items.Refresh();
                        comboBoxAccounts.Items.Clear();
                        comboBoxAccountsReminder.Items.Clear();
                        foreach (var a in Database1Entities1Ins.Accounts)
                        {
                            comboBoxAccounts.Items.Add(a.BankName);
                            comboBoxAccountsReminder.Items.Add(a.BankName);
                        }
                        //Clear TextBoxes
                        textBoxAccountBank.Text = null;
                        textBoxAccountCardnumber.Text = null;
                        textBoxAccountNumber.Text = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBoxAccountNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex("[^0-9]");
            e.Handled = regex1.IsMatch(e.Text);
        }
        private void textBoxAccountCardnumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex("[^0-9]");
            e.Handled = regex1.IsMatch(e.Text);
        }
        private void dataGridAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridAccounts.SelectedItem != null)
                {
                    MoneyManagement.Data.Account selectedItem1 = (MoneyManagement.Data.Account)dataGridAccounts.SelectedItem;
                    //Fill TextBoxes
                    textBoxAccountBank.Text = selectedItem1.BankName;
                    textBoxAccountCardnumber.Text = selectedItem1.CardNumber;
                    textBoxAccountNumber.Text = selectedItem1.AcountNumber.ToString();
                }
            }
            catch
            {
                textBoxAccountBank.Text = null;
                textBoxAccountCardnumber.Text = null;
                textBoxAccountNumber.Text = null;
            }
        }
        # endregion ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        # region :::::::::::::::::::::::::::::::::::::::::: Transactions ::::::::::::::::::::::::::::::::::::::::::::
        private void textBoxTransactionMoney_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex("[^0-9]");
                e.Handled = regex1.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridTransactions.SelectedItem != null)
                {
                    MoneyManagement.Data.Transaction selectedItem1 = (MoneyManagement.Data.Transaction)dataGridTransactions.SelectedItem;
                    datePickerTransaction.SelectedDate = selectedItem1.Date;
                    textBoxTransactionMoney.Text = selectedItem1.Money.ToString();
                    textBoxTransactionDescription.Text = selectedItem1.Description;
                    comboBoxTransactionChange.SelectedItem = selectedItem1.Change;

                    using (var Database1Entities1Ins = new Data.Database1Entities())
                    {
                        string stringBankName = (from acc in Database1Entities1Ins.Accounts
                                                 where acc.ID == selectedItem1.AccountID
                                                 select acc.BankName).First();
                        comboBoxAccounts.SelectedItem = stringBankName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonAddTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Insert
                using (var Database1Entities1Ins = new Data.Database1Entities())
                {
                    //Add new Transaction
                    string stringBankName = comboBoxAccounts.SelectedItem.ToString();
                    int intAccountID = (from acc in Database1Entities1Ins.Accounts
                                        where acc.BankName == stringBankName
                                        select acc.ID).First();
                    var TransactionNew = new Transaction()
                    {
                        AccountID = intAccountID,
                        Change = comboBoxTransactionChange.Text,
                        Date = Convert.ToDateTime(datePickerTransaction.SelectedDate),
                        Description = textBoxTransactionDescription.Text,
                        Money = int.Parse(textBoxTransactionMoney.Text)
                    };
                    Database1Entities1Ins.Transactions.Add(TransactionNew);
                    Database1Entities1Ins.SaveChanges();

                    //Show transactions in DataGrid
                    dataGridTransactions.ItemsSource = Database1Entities1Ins.Transactions.ToList();
                    dataGridTransactions.Items.Refresh();

                    //Clear TextBoxes
                    comboBoxAccounts.SelectedIndex = 0;
                    comboBoxTransactionChange.SelectedIndex = 0;
                    datePickerTransaction.SelectedDate = DateTime.Today;
                    textBoxTransactionDescription.Text = null;
                    textBoxTransactionMoney.Text = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void buttonEditTransaction_Click(object sender, RoutedEventArgs e)
        {
            //Update
            //Database1Entities1.SaveChanges();
            try
            {
                if (dataGridTransactions.SelectedItem != null)
                {
                    MoneyManagement.Data.Transaction selectedItem1 = (MoneyManagement.Data.Transaction)dataGridTransactions.SelectedItem;
                    //Delete
                    using (var Database1Entities1Ins = new Data.Database1Entities())
                    {
                        string stringBankName = comboBoxAccounts.SelectedItem.ToString();
                        int intAccountID = (from acc in Database1Entities1Ins.Accounts
                                            where acc.BankName == stringBankName
                                            select acc.ID).First();
                        var q = from a in Database1Entities1Ins.Transactions
                                where a.ID == selectedItem1.ID
                                select a;
                        foreach (var a in q)
                        {
                            a.AccountID = intAccountID;
                            a.Change = comboBoxTransactionChange.Text;
                            a.Date = Convert.ToDateTime(datePickerTransaction.SelectedDate);
                            a.Description = textBoxTransactionDescription.Text;
                            a.Money = int.Parse(textBoxTransactionMoney.Text);
                        }
                        Database1Entities1Ins.SaveChanges();
                        //Show aaccounts in DataGrid
                        dataGridTransactions.ItemsSource = Database1Entities1Ins.Transactions.ToList();
                        dataGridTransactions.Items.Refresh();
                        //Clear TextBoxes
                        comboBoxAccounts.SelectedIndex = 0;
                        comboBoxTransactionChange.SelectedIndex = 0;
                        datePickerTransaction.SelectedDate = DateTime.Today;
                        textBoxTransactionDescription.Text = null;
                        textBoxTransactionMoney.Text = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonDeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGridTransactions.SelectedItem != null)
                {
                    MoneyManagement.Data.Transaction selectedItem1 = (MoneyManagement.Data.Transaction)dataGridTransactions.SelectedItem;
                    //Delete
                    using (var Database1Entities1Ins = new Data.Database1Entities())
                    {
                        Transaction q = (from a in Database1Entities1Ins.Transactions
                                         where a.ID == selectedItem1.ID
                                         select a).First();
                        Database1Entities1Ins.Transactions.Remove(q);
                        Database1Entities1Ins.SaveChanges();
                        //Show aaccounts in DataGrid
                        dataGridTransactions.ItemsSource = Database1Entities1Ins.Transactions.ToList();
                        dataGridTransactions.Items.Refresh();
                        //Clear TextBoxes
                        comboBoxAccounts.SelectedIndex = 0;
                        comboBoxTransactionChange.SelectedIndex = 0;
                        datePickerTransaction.SelectedDate = DateTime.Today;
                        textBoxTransactionDescription.Text = null;
                        textBoxTransactionMoney.Text = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        # endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        #region :::::::::::::::::::::::::::::::::::::::::: Reminder :::::::::::::::::::::::::::::::::::::::::::::::::
        private void buttonAddReminder_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //Insert
            using (var Database1Entities1Ins = new Data.Database1Entities())
            {
                //Add new Reminder
                string stringBankName = comboBoxAccountsReminder.SelectedItem.ToString();
                int intAccountID = (from acc in Database1Entities1Ins.Accounts
                                    where acc.BankName == stringBankName
                                    select acc.ID).First();
                var ReminderNew = new Reminder() { AccountID = intAccountID, DateEn = DateTime.Parse(calendar1.SelectedDate.ToString()), Details = textBoxReminderDetails.Text };
                Database1Entities1Ins.Reminders.Add(ReminderNew);
                Database1Entities1Ins.SaveChanges();
                //Show Reminders in DataGrid
                dataGridReminder.ItemsSource = Database1Entities1Ins.Reminders.ToList();
                dataGridReminder.Items.Refresh();
                //Clear TextBoxes
                comboBoxAccountsReminder.SelectedIndex = 0;
                textBoxReminderDetails.Text = null;
                calendar1.SelectedDate = DateTime.Now;
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void buttonEditReminder_Click(object sender, RoutedEventArgs e)
        {
            //Update
            try
            {
                if (dataGridReminder.SelectedItem != null)
                {
                    using (var Database1Entities1Ins = new Data.Database1Entities())
                    {
                        //Add new Reminder
                        MoneyManagement.Data.Reminder selectedItem1 = (MoneyManagement.Data.Reminder)dataGridReminder.SelectedItem;
                        var q = from a in Database1Entities1Ins.Reminders
                                where a.ID == selectedItem1.ID
                                select a;
                        foreach (var a in q)
                        {
                            string stringBankName = comboBoxAccountsReminder.SelectedItem.ToString();
                            int intAccountID = (from acc in Database1Entities1Ins.Accounts
                                                where acc.BankName == stringBankName
                                                select acc.ID).First();
                            a.DateEn = DateTime.Parse(calendar1.SelectedDate.ToString());
                            a.Details = textBoxReminderDetails.Text;
                        }
                        Database1Entities1Ins.SaveChanges();
                        //Show Reminders in DataGrid
                        dataGridReminder.ItemsSource = Database1Entities1Ins.Reminders.ToList();
                        dataGridReminder.Items.Refresh();
                        //Clear TextBoxes
                        comboBoxAccountsReminder.SelectedIndex = 0;
                        textBoxReminderDetails.Text = null;
                        calendar1.SelectedDate = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonDeleteReminder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGridAccounts.SelectedItem != null)
                {
                    using (var Database1Entities1Ins = new Data.Database1Entities())
                    {
                        //Delete
                        MoneyManagement.Data.Reminder selectedItem1 = (MoneyManagement.Data.Reminder)dataGridReminder.SelectedItem;
                        var q = from a in Database1Entities1Ins.Reminders
                                where a.ID == selectedItem1.ID
                                select a;
                        foreach (var a in q)
                        {
                            Database1Entities1Ins.Reminders.Remove(a);
                        }
                        Database1Entities1Ins.SaveChanges();
                        //Show Reminders in DataGrid
                        dataGridReminder.ItemsSource = Database1Entities1Ins.Reminders.ToList();
                        dataGridReminder.Items.Refresh();
                        //Clear TextBoxes
                        comboBoxAccountsReminder.SelectedIndex = 0;
                        textBoxReminderDetails.Text = null;
                        calendar1.SelectedDate = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dataGridReminder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridReminder.SelectedItem != null)
                {
                    using (var Database1Entities1Ins = new Data.Database1Entities())
                    {
                        MoneyManagement.Data.Reminder selectedItem1 = (MoneyManagement.Data.Reminder)dataGridReminder.SelectedItem;
                        //Fill TextBoxes
                        string stringBankName = (from acc in Database1Entities1Ins.Accounts
                                                 where acc.ID == selectedItem1.AccountID
                                                 select acc.BankName).First();
                        comboBoxAccountsReminder.SelectedItem = stringBankName;
                        textBoxReminderDetails.Text = selectedItem1.Details;
                        calendar1.SelectedDate = selectedItem1.DateEn;
                    }
                }
            }
            catch
            {
                comboBoxAccountsReminder.SelectedIndex = 0;
                textBoxReminderDetails.Text = null;
                calendar1.SelectedDate = DateTime.Now;
            }
        }
        #endregion Reminder

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            string string1 = comboBoxAccountsReminder.SelectedValue.ToString();
        }

    }
}
