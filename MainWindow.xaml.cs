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
using AutoLotModel;
using System.Data;

namespace MovieShop
{

    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;

        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();

        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;

        CollectionViewSource customerOrdersViewSource;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inventoryViewSource = (System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource"));
            inventoryViewSource.Source = ctx.Inventories.Local;

            customerViewSource = (CollectionViewSource)FindResource("customerViewSource");
            customerViewSource.Source = ctx.Customers.Local;


            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));

            ctx.Customers.Load();
            ctx.Orders.Load();
            ctx.Inventories.Load();

            cmbCustomers.ItemsSource = ctx.Customers.Local;
            
            cmbCustomers.SelectedValuePath = "CustId";

            cmbInventory.ItemsSource = ctx.Inventories.Local;
           
            cmbInventory.SelectedValuePath = "MovieId";

            BindDataGrid();
        }

        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals
                             cust.CustId
                             join inv in ctx.Inventories on ord.MovieId
                 equals inv.MovieId
                             select new
                             {
                                 ord.OrderId,
                                 ord.MovieId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.MovieTitle,
                                 inv.MovieFormat
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }

        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
        }

        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }


        // cod pt. Customers

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se initializeaza comanda");
            Customer customer = null;
            if (action == ActionState.New)
            {
                MessageBox.Show("S-a Adaugat un client nou");
                try
                {
                    
                    customer = new Customer()
                    {
                        CustId = int.Parse(custIdTextBox.Text.Trim()),
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim(),
                        Age = ageTextBox.Text.Trim(),
                        Telephone = telephoneTextBox.Text.Trim()

                    };

                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Edit)
            {
                MessageBox.Show("Datele clientului au fost editate");
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    customer.Age = ageTextBox.Text.Trim();
                    customer.Telephone = telephoneTextBox.Text.Trim();

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                customerViewSource.View.Refresh();

                customerViewSource.View.MoveCurrentTo(customer);
            }
            else if (action == ActionState.Delete)
            {
                MessageBox.Show("Clientul a fost sters");
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
            }

            action = ActionState.Nothing;
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }

        // cod pt. Inventory/Movie

        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se initializeaza comanda");
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                MessageBox.Show("S-a Adaugat un film nou");
                try
                {
                    inventory = new Inventory()
                    {
                        MovieId = int.Parse(movieIdTextBox.Text.Trim()),
                        MovieTitle = movietitleTextBox.Text.Trim(),
                        MovieFormat = movieformatTextBox.Text.Trim(),
                        MoviePrice = moviepriceTextBox.Text.Trim(),
                        MovieRating = movieratingTextBox.Text.Trim()
                    };

                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Edit)
            {
                MessageBox.Show("Datele filmului au fost editate");
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.MovieId = int.Parse(movieIdTextBox.Text.Trim());
                    inventory.MovieTitle = movietitleTextBox.Text.Trim();
                    inventory.MovieFormat = movieformatTextBox.Text.Trim();
                    inventory.MoviePrice = moviepriceTextBox.Text.Trim();
                    inventory.MovieRating = movieratingTextBox.Text.Trim();

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                inventoryViewSource.View.Refresh();

                inventoryViewSource.View.MoveCurrentTo(inventory);
            
            }
            else if (action == ActionState.Delete)
            {
                MessageBox.Show("Filmul a fost sters");
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
            }

            action = ActionState.Nothing;
        }

        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }

        private void btnPrevious1_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }

        // cod pt. Orders

        private void btnSave2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se initializeaza comanda");
            Order order = null;
            if (action == ActionState.New)
            {
                MessageBox.Show("S-a Adaugat o comanda noua");
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                  
                    order = new Order()
                    {

                        CustId = customer.CustId,
                        MovieId = inventory.MovieId
                    };
                  
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                   
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else if (action == ActionState.Edit)
            {
                dynamic selectedOrder = ordersDataGrid.SelectedItem;
                try
                {
                    MessageBox.Show("Datele comenzii au fost editate");
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                        editedOrder.MovieId = Convert.ToInt32(cmbInventory.SelectedValue.ToString());
                       
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
               
                customerViewSource.View.MoveCurrentTo(selectedOrder);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = ordersDataGrid.SelectedItem;
                    int curr_id = selectedOrder.OrderId;
                    var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (deletedOrder != null)
                    {
                        ctx.Orders.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Comanda a fost stearsa");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToNext();
        }

        private void btnPrevious2_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToPrevious();
        }


        private void customerDataGrid_SelectionChanged()
        {

        }

        private void inventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void customerDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }

 }
