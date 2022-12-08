using BusinessLogic;
using DTO;
using DTO.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace L9_DatingApp
{
    public partial class MainWindow : Window
    {
        //public ObservableCollection<User> UserData { get; set; }

        UserBLL bll = new UserBLL();
        User user;

        public MainWindow()
        {
            InitializeComponent();
            user = new User(); // lav et nyt datacontext obj.
            //idTxt.DataContext = user; // sætter datacontext på vores textbox
            //nameTxt.DataContext = user;
            //emailTxt.DataContext = user;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        private void refreshUsers()
        {
            //userList.Items.Clear();
            userList.ItemsSource = null;
            userList.ItemsSource = bll.getAllusers(); // ItemSource = IEnumable type
            //bll.getAllusers().ForEach(user => userList.Items.Add(user));
        }
        private void uiRefreshPropertyList(User user)
        {
            propertyList.Items.Clear();
            List<Property> usersProperties = bll.getAllUsersProperties(user.Id);
            usersProperties.ForEach(property => propertyList.Items.Add(property));
        }

        //------------------------------------------------------------------------------------------------------------------------
        // Show a user (read datasource and show it to gui)
        //------------------------------------------------------------------------------------------------------------------------
        private void showBtn_Click(object sender, RoutedEventArgs e)
        {
            refreshUsers();
        }

        //------------------------------------------------------------------------------------------------------------------------
        // Create and Add a user (read a gui til datasource)
        //------------------------------------------------------------------------------------------------------------------------
        private void userAddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (userList.SelectedItem != null)
            {
                MessageBox.Show("You should not select an item before you add a new user!");
            }
            else
            {
                User user = new User(int.Parse(idTxt.Text), nameTxt.Text, emailTxt.Text);
                userList.ItemsSource = null;
                //userList.Items.Add(user);
                bll.addUser(user);
                refreshUsers();
            }
        }
        //------------------------------------------------------------------------------------------------------------------------
        // Edit a user (read gui and edit our datasource)
        //------------------------------------------------------------------------------------------------------------------------
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            user = (User)userList.SelectedItem;
            //userList.ItemsSource = null;

            user.Name = nameTxt.Text;
            user.Email = emailTxt.Text;
            bll.editUser(user);

            refreshUsers();
        }

        //------------------------------------------------------------------------------------------------------------------------
        // Delete a user
        //------------------------------------------------------------------------------------------------------------------------
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            user = (User)userList.SelectedItem;
            MessageBox.Show("You delete this user. " + user.ToString());
            bll.deleteUser(user);
            //userList.Items.Remove(user);
            refreshUsers();
        }

        //------------------------------------------------------------------------------------------------------------------------
        // Search a user
        //------------------------------------------------------------------------------------------------------------------------
        private void searchBTN_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(idSearchTxt.Text))
            {
                MessageBox.Show("you have to write a users ID you want to search! ");
            } 
            
            int usersId = int.Parse(idSearchTxt.Text);
            user = bll.getUser(usersId);

            if (user != null)            {  
                MessageBox.Show("The user name is: " + user.Name);
            }
            else { 
                MessageBox.Show("The user you are looking for does not exist in our DB. Can you write a different users id?");
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        // User-Selected change
        //------------------------------------------------------------------------------------------------------------------------
        private void userList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            user = (User)userList.SelectedItem;

            if (user == null)
            {
                MessageBox.Show("User is Null");
            }
            else
                //idTxt.Text = user.Id.ToString();
                //nameTxt.Text = user.Name;
                //emailTxt.Text = user.Email;
                uiRefreshPropertyList(user);
        }
        //------------------------------------------------------------------------------------------------------------------------
        // Property Create and Add
        //------------------------------------------------------------------------------------------------------------------------

        private void addPropertyBTN_Click(object sender, RoutedEventArgs e)
        {
            Property property = null;

            if (residenceRadioBTN.IsChecked == true)
            {
                property = new ResidenceProperty(int.Parse(proIDTxt.Text), double.Parse(rentalPriceTxt.Text), int.Parse(numberRoomsTxt.Text));
            }

            else if (businessRadioBTN.IsChecked == true)
            {
                property = new BusinessProperty(int.Parse(proIDTxt.Text), double.Parse(rentalPriceTxt.Text), int.Parse(numberRoomsTxt.Text));
            }

            if (property != null)
            {
                user = (User)userList.SelectedItem;
                bll.addProperty(user.Id, property);
                user.PropertyList.Add(property);

                uiRefreshPropertyList(user);
            }
        }

        private void proShowButton_Click(object sender, RoutedEventArgs e)
        {
            propertyList.Items.Clear();
            bll.getAllProperties().ForEach(pro => propertyList.Items.Add(pro));
        }

        //------------------------------------------------------------------------------------------------------------------------
        // CheckBox - User Sort by name
        //------------------------------------------------------------------------------------------------------------------------
        private void sortByNameCBX_Checked(object sender, RoutedEventArgs e)
        {
            if(sortByNameCBX.IsChecked == true)
            
                userList.ItemsSource = null;
                bll.getAllusersSortByName().ForEach(user => userList.Items.Add(user));
        }
    }
}
