using UnityEngine;
using System.Collections;

namespace TheDarkNight.Lights {

    public interface ISwitch {
        void Toggle();
        bool IsTurnedOn();
    }

}