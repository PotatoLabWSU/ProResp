using MccDaq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Threading;

namespace ProResp3.Models
{
    using MccDaq;

    internal class MccBoardConnection
    {
        public MccBoard board;

        public MccBoardConnection()
        {
            this.board = new MccBoard(0);
            this.config();
        }

        public void CheckAllPorts()
        {
            int numPorts = 24;
            for (int i = 0; i < numPorts; i++)
            {

                Console.WriteLine("Testing port " + i);

                SetPort(board, i, DigitalLogicState.High);
                Thread.Sleep(2000);

                SetPort(board, i, DigitalLogicState.Low);
            }
        }
        public void config()
        {

            ConfigurePort(board, DigitalPortType.FirstPortA);
            ConfigurePort(board, DigitalPortType.FirstPortB);
            ConfigurePort(board, DigitalPortType.FirstPortCH);
            ConfigurePort(board, DigitalPortType.FirstPortCL);
        }
        public void ConfigurePort(MccBoard board, DigitalPortType portType)
        {
            DigitalPortDirection direction = DigitalPortDirection.DigitalOut;
            board.DConfigPort(portType, direction);
        }

        public void SetPort(MccBoard board, int portNum, DigitalLogicState state)
        {
            board.DBitOut(DigitalPortType.FirstPortA, portNum, state);
        }

        public void open(int current)
        {
            SetPort(board, current, DigitalLogicState.High);
        }
        public void close(int current)
        {
            SetPort(board, current, DigitalLogicState.Low);
        }
        public void TurnOffAllPorts()
        {
            // Turn off all ports
            for (int i = 0; i < 24; i++)
            {
                SetPort(board, i, DigitalLogicState.Low);
            }
        }
    }
}
