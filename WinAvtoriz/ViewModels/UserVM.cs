using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WinAvtoriz.Models;
using WinAvtoriz.View;

namespace WinAvtoriz.ViewModels
{
    class UserVM : INotifyPropertyChanged
    {
        private string connect = "Data Source=localhost;Initial Catalog=RegAvt_BD;Integrated Security=True";
        private string login;
        private string password;
        private int id;
        private User newUser;

        public int Id
        {
            get { return id; }
            set 
            {
                id = value;
                OnPropertyChanged(nameof(Id));
                
            }
        }

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand EditingCommand { get; set; }

        public UserVM()
        {
            LoginCommand = new RelayCommand(Logic);
            NewUser = new User();
            AddUserCommand = new RelayCommand(AddUser);
            AddUserCommand2 = new RelayCommand(AddUser2);
            DelUserCommand = new RelayCommand(DelUser);
            EditingUserCommand = new RelayCommand(EditingUser);
        }

        private void Logic()
        {
            Main main = new Main();
            try
            {
                if (IsUserValid(Login, password))
                {
                    main.Show();
                    Application.Current.Windows[0].Close();
                }
                else
                {
                    MessageBox.Show($"Такого пользователя в базе данных нет!");                    
                }
            }
            catch
            {
                return;
            }
        }


        private bool IsUserValid(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Table_1 WHERE Login = @Login AND Password = @Password", connection);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                int count = (int) command.ExecuteScalar();

                return count > 0;
            }
        }

        public User NewUser
        {
            get { return newUser; }
            set
            {
                newUser = value;
                OnPropertyChanged(nameof(NewUser));
            }
        }

        public ICommand AddUserCommand { get; set; }
        public ICommand DelUserCommand { get; set; }
        public ICommand AddUserCommand2 { get; set; }
        public ICommand EditingUserCommand { get; set; }

        private void AddUser()
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                connection.Open();
                

                SqlCommand command = new SqlCommand("INSERT INTO Table_1 (FirstName, LastName, Login, Password) VALUES (@FirstName, @LastName, @Login, @Password)", connection);
                MainWindow mainWindow = new MainWindow();

                try
                {
                    command.Parameters.AddWithValue("@FirstName", NewUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", NewUser.LastName);
                    command.Parameters.AddWithValue("@Login", NewUser.Login);
                    command.Parameters.AddWithValue("@Password", NewUser.Password);

                    int rowAffected = command.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Пользователь добавлен)");
                        mainWindow.Show();
                        Application.Current.Windows[0].Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                }
                connection.Close();
            }
        }

        private void AddUser2()
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                connection.Open();


                SqlCommand command = new SqlCommand("INSERT INTO Table_1 (FirstName, LastName, Login, Password) VALUES (@FirstName, @LastName, @Login, @Password)", connection);
                try
                {
                    command.Parameters.AddWithValue("@FirstName", NewUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", NewUser.LastName);
                    command.Parameters.AddWithValue("@Login", NewUser.Login);
                    command.Parameters.AddWithValue("@Password", NewUser.Password);

                    int rowAffected = command.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Пользователь добавлен)");
                        Application.Current.Windows[2].Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                }
                connection.Close();
            }
        }


        private void EditingUser()
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"UPDATE Table_1 SET Login = @Login, FirstName = @FirstName, LastName = @LastName, Password = @Password WHERE id = @id", connection);
                
                try
                {
                    command.Parameters.AddWithValue("@id", NewUser.Id);
                    command.Parameters.AddWithValue("@FirstName", NewUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", NewUser.LastName);
                    command.Parameters.AddWithValue("@Login", NewUser.Login);
                    command.Parameters.AddWithValue("@Password", NewUser.Password);

                    int rowAffected = command.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Пользователь отредактирован)");
                        Application.Current.Windows[2].Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Пользователь не отредактирован(");
                }
                connection.Close();
            }
        }

        private void DelUser()
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                connection.Open();

                SqlCommand command = new SqlCommand($"DELETE FROM Table_1 WHERE id = @id", connection);

                try
                {
                    command.Parameters.AddWithValue("@id", NewUser.Id);

                    int rowAffected = command.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Пользователь Удалён)");
                        Application.Current.Windows[2].Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Пользователь не удалён(");
                }
                connection.Close();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }
        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
