using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.Commands
{
    using ProResp3.ViewModels;
    using System.Windows.Input;
    public class CloseCommand : ICommand
    {
        MainViewModel viewModel;
        public event EventHandler? CanExecuteChanged;

        public CloseCommand(MainViewModel argViewModel)
        {
            viewModel = argViewModel;
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
            }
        }
    }
}
