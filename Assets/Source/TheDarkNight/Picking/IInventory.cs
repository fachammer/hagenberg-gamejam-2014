using UnityEngine;
using System.Collections;

namespace TheDarkNight.Picking {

    public interface IInventory {
        bool AddItem(GameObject pickable);
        bool RemoveItem(GameObject pickable);       
    }

}