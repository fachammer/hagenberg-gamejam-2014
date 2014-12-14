using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Picking {
    public interface IDropper {
        bool TryDropLightBulb();

        IObservable<IPickable> Drop { get; }
    }
}
