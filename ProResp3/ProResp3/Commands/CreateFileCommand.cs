using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.Commands
{
    using ProResp3.ViewModels;
    using System.Windows.Input;
    using Microsoft.Win32;

    public class CreateFileCommand : ICommand
    {
        private MainViewModel viewModel;

        public CreateFileCommand(MainViewModel argViewModel)
        {
            this.viewModel = argViewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt";

            Nullable<bool> result = saveFileDialog1.ShowDialog();

            if(result == true)
            {
                if(parameter?.ToString() == "DataFile")
                {
                    viewModel.DataFilePath = saveFileDialog1.FileName;
                }
            }
        }
    }
}
