using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Markup;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    public class Experiment : INotifyPropertyChanged
    {
        // Respiration equation constants
        private const double MOLS_OF_SUBSTANCE = 1;
        private const double GAS_CONSTANT = 0.082; // (L*atm)/(K*mol)
        private const double PRESSURE = 0.91; // Based on Pullman, WA (2,352ft)

        private LI7000Connection _LI7000;
        private MccBoardConnection _board;
        private FlowMeterConnection _flowMeter;
        private Valve _activeValve;
        private DateTime startDate;
        private List<int> _activeValveNums = new List<int>();
        private List<double?> _valveWeights = new List<double?>();
        private int _activeValveIndex;
        private double _valveSwitchTimeMin;
        private int _dataPollTimeSec;
        DispatcherTimer pollDataTimer;
        DispatcherTimer valveSwitchTimer;
        private string _dataFilePath;
        DateTime _timeOfLastValveSwitch;


        public event PropertyChangedEventHandler? PropertyChanged;

        public Valve ActiveValve { get { return _activeValve; } set { _activeValve = value; } }
        public DateTime TimeOfLastValveSwitch { get { return _timeOfLastValveSwitch; } }
        public string DataHeader { get; private set; }

        public Experiment(List<int> argActiveValveNums, List<double?> argValveWeights, double argValveSwitchTimeMin, string argDataFilePath)
        {
            _activeValveNums = argActiveValveNums;
            _valveWeights = argValveWeights;
            _valveSwitchTimeMin = argValveSwitchTimeMin;
            _dataFilePath = argDataFilePath;
            _dataPollTimeSec = 3;

            _board = new MccBoardConnection();
            _LI7000 = new LI7000Connection();
            _flowMeter = new FlowMeterConnection();

            //Activate first valve
            this._board.TurnOffAllPorts();
            this._activeValve = new Valve(this._activeValveNums.First(), this._valveWeights.First());


            //Add units to ActiveValve
            string[] LI7000Units = _LI7000.DataHeader.Split('\t');
            for(int i = 0; i < LI7000Units.Length; i++)
            {
                if (LI7000Units[i].Contains("CO2"))
                {
                    LI7000Units[i] = LI7000Units[i].Substring(LI7000Units[i].IndexOf(' ') + 1);
                    this._activeValve.CO2Units = LI7000Units[i];
                }
                else if (LI7000Units[i].Contains("H2O"))
                {
                    LI7000Units[i] = LI7000Units[i].Substring(LI7000Units[i].IndexOf(' ') + 1);
                    this._activeValve.H2OUnits = LI7000Units[i];
                }
                else if (LI7000Units[i].Contains("T"))
                {
                    LI7000Units[i] = LI7000Units[i].Substring(LI7000Units[i].IndexOf(' ') + 1).Replace("C", "°C");
                    this._activeValve.TemperatureUnits = LI7000Units[i];
                }
            }
            this._activeValve.FlowUnits = "ml/min";

            //Setup Timers
            this.pollDataTimer = new DispatcherTimer();
            this.pollDataTimer.Interval = TimeSpan.FromSeconds(this._dataPollTimeSec);
            this.pollDataTimer.Tick += this.PollData;

            this.valveSwitchTimer = new DispatcherTimer();
            this.valveSwitchTimer.Interval = TimeSpan.FromMinutes(this._valveSwitchTimeMin);
            this.valveSwitchTimer.Tick += this.SwitchValve;

            //Write data header
            this.SetDataHeader();
            this.WriteDataHeader();
        }

        private void SetDataHeader()
        {
            this.DataHeader = "Day of Experiment\tDate (mm/dd/yyyy)\tTime (hh:mm)\tValve\t";
            this.DataHeader += this._LI7000.DataHeader;

            this.DataHeader = this.DataHeader.Replace("ppm", "(ppm)");
            this.DataHeader = this.DataHeader.Replace("mm/m", "(mm/m)");
            this.DataHeader = this.DataHeader.Replace("T C", "Temperature (°C)");
            this.DataHeader += "\tFlow (ml/min)";
            this.DataHeader += "\tmg CO2/Kg/hr";
        }

        void PollData(object sender, EventArgs e)
        {
            string response = _LI7000.Poll();
            string flowMeterResponse = _flowMeter.Poll();
            string[] flowData;

            //Parse LI7000 data and store in active valve
            if (response?.Substring(0, 5) == "DATA\t")
            {
                response = response.Substring(5);
                response = response.Replace("\n", string.Empty);

                string[] headers = this._LI7000.DataHeader.Split('\t');
                string[] data = response.Split('\t');
                for (int i = 0; i < headers.Length; i++)
                {
                    switch (headers[i][0])
                    {
                        case 'C':
                            this.ActiveValve.CO2 = double.Parse(data[i]);
                            break;
                        case 'H':
                            this.ActiveValve.H2O = double.Parse(data[i]);
                            break;
                        case 'T':
                            this.ActiveValve.Temperature = double.Parse(data[i]);
                            break;
                    }
                }
            }
            //Parse flow data and store in active valve
            flowData = flowMeterResponse.Split(" ");
            this.ActiveValve.Flow = double.Parse(flowData[3]);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActiveValveData"));
        }

        public void Start()
        {
            this._board.TurnOffAllPorts();
            this.pollDataTimer?.Start();
            this.valveSwitchTimer?.Start();
            this._board.open(this._activeValveNums[this._activeValveIndex]);
            this.startDate = DateTime.Now;
            this.PollData(this, new EventArgs());
        }

        public void SwitchValve(object sender, EventArgs e)
        {
            this.WriteData();
            this._board.close(this.ActiveValve.ValveNum);

            if (this._activeValveIndex + 1 < this._activeValveNums.Count)
            {
                this._activeValveIndex++;
            }
            else
            {
                this._activeValveIndex = 0;
                this._board = null;                         // If board runs for multiple cycles it starts turning multiple ports on.
                this._board = new MccBoardConnection();     // Creating a new board each cycle is an attempt to fix it.
            }

            this._board.open(this._activeValveNums[this._activeValveIndex]);
            this.ActiveValve.ValveNum = this._activeValveNums[this._activeValveIndex];

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActiveValve"));

            return;
        }

        private void WriteDataHeader()
        {
            using (StreamWriter sw = new StreamWriter(this._dataFilePath, false))
            {
                sw.WriteLine(this.DataHeader);
                sw.Close();
            }
        }

        private void WriteData()
        {
            string data = string.Empty;
            DateTime currentDateTime = DateTime.Now;
            this._timeOfLastValveSwitch = currentDateTime;
            this.PollData(this, new EventArgs());
            TimeSpan dayOfExperiment = currentDateTime.Subtract(this.startDate);

            data = (dayOfExperiment.Days + 1).ToString() + "\t";
            data += currentDateTime.ToString("MM/dd/yyyy\tHH:mm") + "\t";
            data += this.ActiveValve.GetDataString();
            data += "\t" + this.GetRespiration();

            using (StreamWriter sw = new StreamWriter(this._dataFilePath, true))
            {
                sw.WriteLine(data);
                sw.Close();
            }
        }

        private string GetRespiration()
        {
            string result = string.Empty;
            double numResult = 0;

            if (this.ActiveValve.Weight == null)
            {
                return "-";
            }

            
            double flow = this.ActiveValve.Flow / 1000; // Convert ml/min to L/min
            double weight = ((double)this.ActiveValve.Weight) / 1000; // Convert g to Kg
            numResult = ((ActiveValve.CO2 * flow) / weight) * 60; // uL CO2/Kg/hr WHERE DOES uL COME FROM??
            numResult = numResult / 1000; //mL CO2/Kg/hr

            double VolGas = ((MOLS_OF_SUBSTANCE * GAS_CONSTANT) * ActiveValve.Temperature) / PRESSURE; // SHOULD IT BE TEMP OR 4C??

            numResult = (((numResult / 1000) / VolGas) * 44) * 1000; // mg CO2/Kg/hr

            return numResult.ToString();
        }

        public void Stop()
        {
            pollDataTimer.Stop();
            valveSwitchTimer.Stop();
            _board.TurnOffAllPorts();
            _flowMeter.Close();
            //_LI7000.CloseConnection();  Breaks if poll data event is called after. Seems to close by itself fine.
        }
    }
}
