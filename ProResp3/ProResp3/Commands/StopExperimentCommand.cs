using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.Commands
{
    using System.Windows.Input;
    using ProResp3.ViewModels;

    internal class StopExperimentCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private MainViewModel viewModel;

        public StopExperimentCommand(MainViewModel argViewModel)
        {
            this.viewModel = argViewModel;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (this.viewModel.SelectedViewModel.GetType() == typeof(ExperimentViewModel))
            {
                ExperimentViewModel experimentViewModel = (ExperimentViewModel)this.viewModel.SelectedViewModel;
                experimentViewModel.experiment?.Stop();
                experimentViewModel.experiment = null;
                this.viewModel.SelectedViewModel = new SetupViewModel();
                this.viewModel.ExperimentRunning = false;
            }
        }
    }
}
