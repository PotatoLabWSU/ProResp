using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProResp3.ViewModels
{
    using ProResp3.Collections;

    internal class SetupViewModel : BaseViewModel
    {
        private ValveWeightCollection _valveWeights = new ValveWeightCollection(Globals.NumValves);

        public ValveWeightCollection ValveWeights
        { 
            get
            {
                return _valveWeights;
            }
        }
    }
}
