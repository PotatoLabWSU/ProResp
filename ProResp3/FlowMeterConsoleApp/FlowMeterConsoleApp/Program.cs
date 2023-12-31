﻿using FlowMeterConsoleApp;
using System.Timers;


class Program
{
    static string filePath = "C:\\Users\\wsupo\\Desktop\\FlowData2023";
    static System.Timers.Timer recordDataTimer = new System.Timers.Timer();
    static FlowMeter flowMeter = new FlowMeter();

    static void Main(string[] args)
    {
        
        bool validSetup = false;
        double collectionInterval = 0;

        string fileName = string.Empty;
        

        recordDataTimer.Elapsed += RecordDataTimer_Elapsed;

        //Setup
        while(!validSetup)
        {
            Console.WriteLine("Enter data collection interval (min): ");
            if (!double.TryParse(Console.ReadLine(), out collectionInterval))
            {
                validSetup = false;
                Console.WriteLine("Error: Invalid time entered.");
                continue;
            }

            Console.WriteLine("Enter file path for flow: ");
            fileName = Console.ReadLine() + ".txt";
            filePath += "\\" + fileName;

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine("Date (mm/dd/yyyy)\tTime(hh:mm)\tFlow (ml/min)");
                    sw.Close();
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Error: " +  ex.Message);
                validSetup = false;
                continue;
            }
            validSetup = true;
        }

        recordDataTimer.Interval = collectionInterval * 60000;

        Console.WriteLine("Press Enter to start data collection.");
        Console.ReadLine();
        recordDataTimer.Start();
        Console.WriteLine("Experiment Running...");

        while(true)
        {

        }
    }

    private static void RecordDataTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        string response = string.Empty;
        string[] flowMeterData;
        string data = string.Empty;
        DateTime dateTime;

        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            dateTime = DateTime.Now;
            response = flowMeter.Poll();
            flowMeterData = response.Split(" ");

            data += dateTime.ToString("MM/dd/yyyy\tHH:mm") + "\t";
            data += flowMeterData[3];
            
            sw.WriteLine(data);
            Console.WriteLine(data);
            sw.Close();
        }
    }
}