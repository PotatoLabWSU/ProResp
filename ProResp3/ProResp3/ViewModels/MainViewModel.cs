using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.ViewModels
{
    using ProResp3.Commands;
    using System.Windows.Input;
    using ProResp3.Collections;
    using ProResp3.Models;
    using System.Windows;

    public class MainViewModel : BaseViewModel
    {

        private BaseViewModel _selectedViewModel;
        private string _dataFilePath = string.Empty;
        private CheckedValvesCollection _checkedValves = new CheckedValvesCollection(Globals.NumValves);
        private bool _experimentRunning;
        private string _valveSwitchTime;
        public Experiment experiment;

        public ICommand CreateFileCommand { get; set; }
        public ICommand StartButtonClick { get; set; }
        public ICommand CheckAllValves { get; set; }
        public ICommand StopButtonClick { get; set; }
        public ICommand CloseButtonClick { get; set; }
        public ICommand SelectAllValves { get; set; }


        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            {   
                _selectedViewModel = value; 
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        public string DataFilePath
        {
            get { return _dataFilePath; }
            set { _dataFilePath = value; OnPropertyChanged(nameof(DataFilePath)); }
        }

        public bool ExperimentRunning
        {
            get { return _experimentRunning; }
            set 
            { 
                _experimentRunning = value;
                NotExperimentRunning = value;
                OnPropertyChanged(nameof(ExperimentRunning));
            }
        }

        public bool NotExperimentRunning
        {
            get { return !_experimentRunning; }
            private set { _experimentRunning = value; OnPropertyChanged(nameof(NotExperimentRunning)); }
        }

        public CheckedValvesCollection CheckedValves
        {
            get { return _checkedValves; }
        }

        public string ValveSwitchTime
        {
            get { return _valveSwitchTime; }
            set { _valveSwitchTime = value; OnPropertyChanged(nameof(ValveSwitchTime)); }
        }

        public MainViewModel()
        {
            ExperimentRunning = false;
            ValveSwitchTime = "15";
            CreateFileCommand = new CreateFileCommand(this);
            StartButtonClick = new StartExperimentCommand(this);
            CheckAllValves = new CheckAllValvesCommand(this);
            StopButtonClick = new StopExperimentCommand(this);
            CloseButtonClick = new CloseCommand(this);
            SelectAllValves = new SelectAllValvesCommand(this);

            this.SelectedViewModel = new SetupViewModel();
        }

        public void MessageBoxAnswerProcess(MessageBoxResult result)
        {

        }
    }
}
