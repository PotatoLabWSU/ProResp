
namespace ProResp3.Models
{
    public class Valve
    {
        private double cO2;
        private double h2O;
        private double temperature;
        private double flow;


        public int ValveNum { get; set; }
        public double? Weight { get; set; }

        public double CO2
        {
            get { return cO2; }
            internal set { cO2 = value; }
        }
        public string CO2Units { get; internal set; }

        public double H2O
        {
            get { return h2O; }
            internal set { h2O = value; }
        }
        public string H2OUnits { get; internal set; }

        public double Temperature
        {
            get { return temperature; }
            internal set { temperature = value; }
        }
        public string TemperatureUnits { get; internal set; }

        public double Flow { get; internal set; }
        public string FlowUnits { get; internal set; }

        public Valve(int argValveNum)
        {
            this.ValveNum = argValveNum;
        }

        public Valve(int argValveNum, double? argWeight)
        {
            this.ValveNum = argValveNum;
            this.Weight = argWeight;
        }

        public string GetDataString()
        {
            string data = string.Empty;

            data += (this.ValveNum+1) + "\t" + this.CO2 + "\t" + this.H2O + "\t" + this.Temperature + "\t" + this.Flow;

            return data;
        }
    }
}
