using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using LabWeek5.Models;
using LabWeek5.Utils;

namespace LabWeek5.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly FileManagement _fileManager;
        public ObservableCollection<User> Users { get; } = new();
        public ObservableCollection<User> RestoreUsers { get; } = new();

        public MainWindowViewModel(TopLevel? topLevel)
        {
            _fileManager = new FileManagement(topLevel);
        }

        public void AddNewUser(User user)
        {
            Users.Add(user);
            NotifyObserver($"User {user.Email} has been added.");
            RestoreUsers.Add(user);
        }

        public void FilterUser(string email)
        {
            string trimmedEmail = email.Trim();

            if (string.IsNullOrEmpty(trimmedEmail))
            {
                RestoreUsersFilter();
                return;
            }

            List<User> filteredUsers = Users.Where(u => u.Email.Contains(trimmedEmail)).ToList();
            FilteredUsers(filteredUsers);
        }

        public void RestoreUsersFilter()
        {
            Users.Clear();
            foreach (User user in RestoreUsers)
            {
                Users.Add(user);
            }
        }

        public void FilteredUsers(List<User> filteredUsers)
        {
            Users.Clear();
            foreach (User user in filteredUsers)
            {
                Users.Add(user);
            }
            NotifyObserver("Filtering email");
        }

        public void ExportUsers()
        {
            _fileManager.WriteFile(Users);
        }

        public async Task ImportUsers()
        {
            await _fileManager.ReadFile();
            AddExportUsers(_fileManager.GetUsersFromFile());
        }

        private void AddExportUsers(ObservableCollection<User> usersExport)
        {
            foreach (User user in usersExport)
            {
                if (UsersEquals(user))
                {
                    Users.Add(user);
                    RestoreUsers.Add(user);
                }
            }
        }

        private bool UsersEquals(User user)
        {
            return Users.All(u => u.ToString() != user.ToString());
        }

        private void NotifyObserver(string message)
        {
            ObserverMessage messageObserver = new ObserverMessage();
            Subject subject = new Subject();
            subject.Add(messageObserver);
            subject.Notify(message);
        }
    }
}
