using System.IO.Ports;
class Program
{
    
    static void Main(string[] args)
    {
        string response = string.Empty;
        SerialPort flowMeter = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

        flowMeter.WriteTimeout = 1000;
        flowMeter.ReadTimeout = 1000;

        flowMeter.Open();

        Console.WriteLine("Connection open: " + flowMeter.IsOpen);
        Console.WriteLine(flowMeter.CDHolding);
        

        Console.WriteLine("Menu");
        Console.WriteLine("------------------------------");
        Console.WriteLine("'q' to quit");
        while (response != "q")
        {
            Console.WriteLine("Enter Command:");
            response = Console.ReadLine();

            flowMeter.WriteLine(response);
            flowMeter.ReadTo(response);
        }

    }
}