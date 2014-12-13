using System.Collections.Generic;

namespace TheDarkNight.Picking {

    public interface IInventory {

        bool AddItem(IPickable pickable);

        bool RemoveItem(IPickable pickable);

        IEnumerable<IPickable> GetItems();
    }
}