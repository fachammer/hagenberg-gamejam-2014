using UnityEngine;

namespace TheDarkNight.Picking {

    public interface IPickable {

        Transform GetTransform();

        bool CanBePickedUpBy(IPicker picker);

        void CannotBePickedupByOthersThan(IPicker picker);
    }
}