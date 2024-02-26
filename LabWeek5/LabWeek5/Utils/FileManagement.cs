using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using LabWeek5.Models;

namespace LabWeek5.Utils
{
    public class FileManagement
    {
        private readonly LogManagement _logManagement = new LogManagement();
        private string _pathFile;
        private IReadOnlyList<IStorageFile> _file;
        private ObservableCollection<User> _userFromFile = new ObservableCollection<User>();

        // Construtor que recebe uma instância de TopLevel
        public FileManagement(TopLevel topLevel)
        {
            TopLevel = topLevel;
        }

        // Propriedade para armazenar a instância de TopLevel
        private TopLevel TopLevel { get; }

        public async Task ReadFile()
        {
            try
            {
                _userFromFile.Clear();
                await ImportFile();

                // Ler o conteúdo do arquivo
                await using var stream = await _file.First().OpenReadAsync();
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Validar e formatar cada linha do arquivo
                        if (ValidatingData(line))
                        {
                            User user = FormatData(line);
                            _userFromFile.Add(user);
                        }
                        // Se a linha não for válida, ela será ignorada
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to read file");
            }
        }

        public async Task WriteFile(ObservableCollection<User> users)
        {
            try
            {
                if (users.Count == 0)
                {
                    throw new Exception("Data is empty");
                }

                await ExportFile();

                // Construir uma string com os dados dos usuários
                StringBuilder stringUsers = new StringBuilder();
                foreach (User user in users)
                {
                    stringUsers.AppendLine(user.ToString());
                }

                // Escrever a string no arquivo
                using (StreamWriter writer = new StreamWriter(_pathFile))
                {
                    writer.Write(stringUsers);
                }

                Console.WriteLine("File created successfully!");
            }
            catch (Exception e)
            {
                var msg = $"Error exporting file: {e.Message}";
                Console.WriteLine(msg);
                _logManagement.AddLog(msg);
            }
        }

        private async Task ImportFile()
        {
            try
            {
                // Selecionar arquivo para importação usando a instância de TopLevel
                var selectedFileOptions = new FilePickerOpenOptions
                {
                    FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
                };
                _file = await TopLevel.StorageProvider.OpenFilePickerAsync(selectedFileOptions);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to import file");
            }
        }

        private async Task ExportFile()
        {
            try
            {
                // Selecionar pasta para exportação usando a instância de TopLevel
                var selectedFolder = await TopLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions());
                var path = selectedFolder.First().Path;
                _pathFile = Path.Combine(path.AbsolutePath, "List-Users.txt");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to export file");
            }
        }

        private bool ValidatingData(string line)
        {
            // Validar os dados da linha
            string[] datas = line.Trim().Split(",");
            if (datas.Length != 3) return false;

            return datas.All(data => data.Trim().Length > 0 && (Validator.ValidatorName(data) || Validator.ValidateEmail(data)));
        }

        private User FormatData(string line)
        {
            // Formatar dados da linha para criar um objeto User
            string[] datas = line.Trim().Split(",");
            return new User(datas[0].Trim(), datas[1].Trim(), datas[2].Trim());
        }

        public ObservableCollection<User> GetUsersFromFile()
        {
            return _userFromFile;
        }
    }
}
