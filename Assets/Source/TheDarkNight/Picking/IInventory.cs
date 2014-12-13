using UnityEngine;
using System.Collections;

namespace TheDarkNight.Picking {

    public interface IInventory {
        bool AddItem(IPickable pickable);
        bool RemoveItem(IPickable pickable);       
    }

}