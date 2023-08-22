using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.Commands
{
    using System.Windows.Input;
    using ProResp3.ViewModels;

    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;
        public event EventHandler? CanExecuteChanged;
        public UpdateViewCommand(MainViewModel argViewModel)
        {
            this.viewModel = argViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if(parameter?.ToString() == "Setup")
            {
                viewModel.SelectedViewModel = new SetupViewModel();
            }
            else if(parameter?.ToString() == "Experiment")
            {
                viewModel.SelectedViewModel = new ExperimentViewModel();
            }
        }
    }
}
