using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.Commands
{
    using ProResp3.ViewModels;
    using System.Windows.Input;
    internal class DeselectAllValvesCommand : ICommand
    {
        MainViewModel viewModel;
        public event EventHandler? CanExecuteChanged;

        public DeselectAllValvesCommand(MainViewModel argViewModel)
        {
            this.viewModel = argViewModel;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            for (int i = 0; i < Globals.NumValves; i++)
            {
                this.viewModel.CheckedValves[i] = false;
            }
        }
    }
}
