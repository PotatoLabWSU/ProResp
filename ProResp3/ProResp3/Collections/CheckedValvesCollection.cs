using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.Collections
{
    using System.ComponentModel;
    using System.Windows.Data;
    public class CheckedValvesCollection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool[] _checkBoxChecked;

        public CheckedValvesCollection(int size)
        {
            _checkBoxChecked = new bool[size];
        }

        public bool this[int index]
        {
            get
            {
                if (index < _checkBoxChecked.Length)
                {
                    return _checkBoxChecked[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Index {index} is not in range of current ValveWeightCollection");
                }

            }

            set
            {
                if (index < _checkBoxChecked.Length)
                {
                    _checkBoxChecked[index] = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Binding.IndexerName));
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Index {index} is not in range of current ValveWeightCollection");
                }
            }
        }

        
    }
}
