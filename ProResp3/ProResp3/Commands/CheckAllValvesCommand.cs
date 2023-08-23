using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProResp3.Commands
{
    using ProResp3.Models;
    using ProResp3.ViewModels;

    internal class CheckAllValvesCommand : ICommand
    {
        private MccBoardConnection mccBoard;
        private MainViewModel viewModel;
        public event EventHandler? CanExecuteChanged;

        public CheckAllValvesCommand(MainViewModel argViewModel)
        {
            this.viewModel = argViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            try
            {
                mccBoard = new MccBoardConnection();
                mccBoard.CheckAllPorts();
                mccBoard = null;
            }
            catch
            {
                return;
            }
            
        }
        
    }
}
