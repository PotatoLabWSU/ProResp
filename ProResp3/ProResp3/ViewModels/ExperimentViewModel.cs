using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.ViewModels
{
    using ProResp3.Models;
    using ProResp3.Collections;
    using System.ComponentModel;

    public class ExperimentViewModel : BaseViewModel
    {
        public Experiment? experiment;
        string _activeValveNum;
        string _currentCO2;
        string _currentH2O;
        string _currentTemperature;
        string _currentFlow;
        string _previousValveNum;
        string _previousCO2;
        string _previousH2O;
        string _previousTemperature;
        string _previousFlow;
        string _timeUntillValveSwitch;

        public string? ActiveValveNum
        {
            get { return  _activeValveNum; }
            set 
            { 
                _activeValveNum = value; 
                OnPropertyChanged(nameof(ActiveValveNum));
            }
        }
        public string? CurrentCO2
        {
            get { return _currentCO2; }
            set
            {
                _currentCO2 = value;
                OnPropertyChanged(nameof(CurrentCO2));
            }
        }
        public string? CurrentH2O
        {
            get { return _currentH2O; }
            set
            {
                _currentH2O = value;
                OnPropertyChanged(nameof(CurrentH2O));
            }
        }
        public string? CurrentTemperature
        {
            get { return _currentTemperature; }
            set
            {
                _currentTemperature = value;
                OnPropertyChanged(nameof(CurrentTemperature));
            }
        }
        public string? CurrentFlow
        {
            get { return _currentFlow; }
            set
            {
                _currentFlow = value;
                OnPropertyChanged(nameof(CurrentFlow));
            }
        }
        public string? PreviousValveNum
        {
            get { return _previousValveNum; }
            set
            {
                _previousValveNum = value;
                OnPropertyChanged(nameof(PreviousValveNum));
            }
        }
        public string? PreviousCO2
        {
            get { return _previousCO2; }
            set
            {
                _previousCO2 = value;
                OnPropertyChanged(nameof(PreviousCO2));
            }
        }
        public string? PreviousH2O
        {
            get { return _previousH2O; }
            set
            {
                _previousH2O = value;
                OnPropertyChanged(nameof(PreviousH2O));
            }
        }
        public string? PreviousTemperature
        {
            get { return _previousTemperature; }
            set
            {
                _previousTemperature = value;
                OnPropertyChanged(nameof(PreviousTemperature));
            }
        }
        public string? PreviousFlow
        {
            get { return _previousFlow; }
            set
            {
                _previousFlow = value;
                OnPropertyChanged(nameof(PreviousFlow));
            }
        }
        public string? TimeOfLastValveSwitch
        {
            get { return _timeUntillValveSwitch; }
            set
            {
                _timeUntillValveSwitch = value;
                OnPropertyChanged(nameof(TimeOfLastValveSwitch));
            }
        }

        public ExperimentViewModel()
        {

        }

        public ExperimentViewModel(ValveWeightCollection argValveWeights, MainViewModel mainViewModel)
        {
            List <int> activeValvesNums = new List <int>();
            List <double?> valveWeights = new List <double?>();

            for (int i = 0; i < Globals.NumValves; i++)
            {
                if (mainViewModel.CheckedValves[i] == true)
                {
                    activeValvesNums.Add(i);

                    if (argValveWeights[i] == string.Empty)
                    {
                        valveWeights.Add(null);
                    }
                    else if (double.TryParse(argValveWeights[i], out double valveWeight))
                    {
                        
                        valveWeights.Add(valveWeight);
                    }
                    else
                    {
                        throw new Exception("Error: Invalid weight for Valve " + (i + 1).ToString() + ".");
                    }
                }
            }

            if (activeValvesNums.Count > 0 && double.TryParse(mainViewModel.ValveSwitchTime, out double valveSwitchTime) && mainViewModel.DataFilePath != string.Empty)
            {
                experiment = new Experiment(activeValvesNums, valveWeights, valveSwitchTime, mainViewModel.DataFilePath);
                experiment.PropertyChanged += this.ExperimentUpdated;
                experiment.Start();
            }
            else if (activeValvesNums.Count <= 0)
            {
                throw new Exception("Error: No Valve Selected.");
            }
            else if (!double.TryParse(mainViewModel.ValveSwitchTime, out double valveSwitchTimeInvalid))
            {
                throw new Exception("Error: Invalid Valve Switch Time.");
            }
            else if (mainViewModel.DataFilePath == string.Empty)
            {
                throw new Exception("Error: Empty Data File Path");
            }

            //Initialize previous valve data
            PreviousValveNum = "n/a";
            PreviousCO2 = "n/a";
            PreviousH2O = "n/a";
            PreviousTemperature = "n/a";
            PreviousFlow = "n/a";
        }

        public void ExperimentUpdated(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveValve")
            {
                PreviousValveNum = ActiveValveNum;
                PreviousCO2 = CurrentCO2;
                PreviousH2O = CurrentH2O;
                PreviousTemperature = CurrentTemperature;
                PreviousFlow = CurrentFlow;
            }
            if (e.PropertyName == "ActiveValveData")
            {
                ActiveValveNum = (this.experiment?.ActiveValve.ValveNum + 1).ToString();
                CurrentCO2 = this.experiment?.ActiveValve.CO2.ToString() + " " + this.experiment?.ActiveValve.CO2Units;
                CurrentH2O = this.experiment?.ActiveValve.H2O.ToString() + " " + this.experiment?.ActiveValve.H2OUnits;
                CurrentTemperature = this.experiment?.ActiveValve.Temperature.ToString() + " " + this.experiment?.ActiveValve.TemperatureUnits;
                CurrentFlow = this.experiment?.ActiveValve.Flow.ToString() + " " + this.experiment?.ActiveValve.FlowUnits;
            }
        }
    }
}
