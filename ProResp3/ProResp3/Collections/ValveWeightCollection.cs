using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.Collections
{
    using System.ComponentModel;
    using System.Windows.Data;

    public class ValveWeightCollection : INotifyPropertyChanged
    {
        string[] _weights;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ValveWeightCollection(int size)
        {
            _weights = new string[size];
            for(int i = 0; i < size; i++)
            {
                _weights[i] = string.Empty;
            }
        }

        public string this[int index]
        {
            get 
            { 
                if (index < _weights.Length)
                {
                    return _weights[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Index {index} is not in range of current ValveWeightCollection");
                }
                
            }

            set
            {
                if (index < _weights.Length)
                {
                    _weights[index] = value;
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
