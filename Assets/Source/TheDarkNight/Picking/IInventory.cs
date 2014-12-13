using UnityEngine;
using System.Collections;

namespace TheDarkNight.Picking {

    public interface IInventory {

        bool AddPickable(IPickable pickable);
        bool RemovePickable(IPickable pickable);
       
    }

}