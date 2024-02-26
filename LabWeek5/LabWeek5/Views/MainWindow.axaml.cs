using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using LabWeek5.Models;
using LabWeek5.Utils;
using LabWeek5.ViewModels;

namespace LabWeek5.Views;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _mainWindow;
    public MainWindow()
    {
        InitializeComponent();
        _mainWindow = new MainWindowViewModel(GetTopLevel(this));
        DataContext = _mainWindow;
    }


    public void Submit(object sender, RoutedEventArgs e)
    {
        try
        {
            string? _firstName = firstName.Text;
            string?  _email = email.Text;
            string?  _lastName = lastName.Text;
        
            bool existsAllFields = string.IsNullOrEmpty(_firstName) || string.IsNullOrEmpty(_email) || string.IsNullOrEmpty(_lastName);
            bool validation = Validator.ValidatorName(_firstName) && Validator.ValidatorName(_lastName) && Validator.ValidateEmail(_email);

            if (!existsAllFields && !validation)
            {
                ObserverMessage messageObserver = new();
                Subject subject = new();
                subject.Add(messageObserver);
                subject.Notify($"Invalid data");
                return;
            } 
            
            User user = new User(_firstName, _lastName, _email);
            _mainWindow.AddNewUser(user);
            
            messageLabel.Foreground = Brushes.Green;
            ShowMessage($"Usuário {user.FirstName} criado com sucesso");
            ClearInputs();
            
        }catch (Exception error) {
            StackError.PushError(error.Message);
            HandleError(StackError.PopError(), error);
        }
    }

    
    
    private void ShowMessage(string message)
    {
        messageLabel.Content = message;
        messageLabel.IsVisible = true;
        
        RemoveLabelMessage();
    }

    private async void RemoveLabelMessage()
    {
        await Task.Delay(2000);
        messageLabel.IsVisible = false;
    }


    private void HandleError(string errorMessage, Exception exception)
    {
        messageLabel.Foreground = Brushes.Red;
        ShowMessage(errorMessage);
    }

    private void FilterUser(object sender, KeyEventArgs e) 
    {
        if (!string.IsNullOrEmpty(filterEmail.Text))
        {
            _mainWindow.FilterUser(filterEmail.Text);
            return;
        }
        _mainWindow.RestoreUsersFilter();
    }

    private void ClearInputs()
    {
        firstName.Text = null;
        lastName.Text = null;
        email.Text = null;
    }

    private void DataGrid_OnSorting(object sender, DataGridColumnEventArgs e)
    {
        ObserverMessage messageObserver = new();
        Subject subject = new();
        subject.Add(messageObserver);
        subject.Notify($"Ordering the list");
    }

    private void ImportUsers(object sender, RoutedEventArgs e)
    {
        _mainWindow.ImportUsers();
    }

    private void ExportUsers(object sender, RoutedEventArgs e)
    {
        _mainWindow.ExportUsers();
    }
}