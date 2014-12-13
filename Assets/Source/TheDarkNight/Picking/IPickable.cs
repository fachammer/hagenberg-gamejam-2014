using UnityEngine;
using System.Collections;
using UniRx;

namespace TheDarkNight.Picking{

    public interface IPickable {
        Transform GetTransform();
    }
}