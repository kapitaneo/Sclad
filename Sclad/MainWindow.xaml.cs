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
using System.Data.Entity;
using Sclad.Entities;
using Microsoft.Win32;
using System.ComponentModel;

namespace Sclad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ScladDbset dbSclad;
        string _pathDB;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Scklad
        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            View _viewAdd = new View();
            _viewAdd.ShowDialog();
            if (_viewAdd.DialogResult==false)
                return;
            Good _newGood = new Good();
            _newGood.Name = _viewAdd.tb_Name.Text;
            _newGood.Instock = Convert.ToInt32(_viewAdd.tb_Count.Text);
            dbSclad.Goods.Add(_newGood);
            dbSclad.SaveChanges();

            MessageBox.Show("Добавлен новый товар");
        }

        private void bt_Change_Click(object sender, RoutedEventArgs e)
        {
            if (dataTable.SelectedItem!=null)
            {
                int index = (dataTable.SelectedItem as Good).Id;

                Good _chGood = dbSclad.Goods.Find(index);
                View _viewCh = new View();
                _viewCh.tb_Name.Text = _chGood.Name;
                _viewCh.tb_Count.Text = _chGood.Instock.ToString();

                _viewCh.ShowDialog();
                if (_viewCh.DialogResult == false)
                    return;

                _chGood.Name = _viewCh.tb_Name.Text;
                _chGood.Instock = Convert.ToInt32(_viewCh.tb_Count.Text);

                dbSclad.SaveChanges();
                CollectionViewSource.GetDefaultView(dataTable.ItemsSource).Refresh();
                dataTable.Columns[0].Visibility = Visibility.Hidden;

                MessageBox.Show("Товар обновлен");
            }
        }

        private void bt_Search_Click(object sender, RoutedEventArgs e)
        {
            string _searchphrase = tb_Search.Text;
            using (dbSclad = new ScladDbset(_pathDB))
            {
                dataTable.ItemsSource = null;
                //dataTable.Items.Clear();
                //dataTable.Columns.Clear();
                var _search = dbSclad.Goods.Where(p => p.Name.Contains(_searchphrase)).ToList();
                var _binding = new BindingList<Good>(_search);
                dataTable.ItemsSource = _binding;
                dataTable.Columns[0].Visibility = Visibility.Hidden;
            }
        }

        #endregion
        #region DataBase
        private void Create_Base_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog _saveFile = new SaveFileDialog();
            _saveFile.DefaultExt = ".mdf";
            _saveFile.Filter = "Local Data Base (.mdf)|*.mdf";
            if (_saveFile.ShowDialog() == true)
            {
                string _filename = _saveFile.FileName;
                _pathDB = @" data source=(localdb)\v11.0; AttachDbFilename=" + _filename + ";Integrated Security=True;";
                dbSclad = new ScladDbset(_pathDB);
                dbSclad.Goods.Load();
                dataTable.ItemsSource = dbSclad.Goods.Local.ToBindingList();
                dataTable.Columns[0].Visibility=Visibility.Hidden;
            }
            else return;
        }

        private void Open_Base_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _openFile = new OpenFileDialog();
            _openFile.DefaultExt = ".mdf";
            _openFile.Filter = "Local Data Base (.mdf)|*.mdf";
            //            DialogResult result = _openFile.ShowDialog(this);
            if (_openFile.ShowDialog() == true)
            {
                string _filename = _openFile.FileName;
                _pathDB = @" data source=(localdb)\v11.0; AttachDbFilename=" + _filename + ";Integrated Security=True;";

                dbSclad = new ScladDbset(_pathDB);
                dbSclad.Goods.Load();
                dataTable.ItemsSource = dbSclad.Goods.Local.ToBindingList();
                dataTable.Columns[0].Visibility=Visibility.Hidden;
            }
            else return;
        }
        #endregion

        #region Selling
        private void bt_Sale_Click(object sender, RoutedEventArgs e)
        {
            New_sale _newsalewind = new New_sale();
           
            var _goods = dbSclad.Goods.Select(p => new
            {
                Id=p.Id,
                Name = p.Name
            }).ToList();
            _newsalewind.cb_Good.ItemsSource = _goods;
            _newsalewind.ShowDialog();

            if (_newsalewind.DialogResult == false)
                return;

            Selling _newsale = new Selling();
            _newsale.Customer = _newsalewind.tb_Customer.ToString();
            _newsale.Date = _newsalewind.Date.SelectedDate.Value;
            _newsale.Count = Convert.ToInt32(_newsalewind.tb_Count.Text);
            _newsale.Cost = Convert.ToInt32(_newsalewind.tb_Cost.Text);
            _newsale.Price = Convert.ToInt32(_newsalewind.tb_Price.Text);


            //_newGood.Name = _viewAdd.tb_Name.Text;
            //  _newGood.Instock = Convert.ToInt32(_viewAdd.tb_Count.Text);
            //  dbSclad.Goods.Add(_newGood);
            dbSclad.SaveChanges();

            MessageBox.Show("Добавлена новая продажа");
        }

        #endregion

        
    }
}
