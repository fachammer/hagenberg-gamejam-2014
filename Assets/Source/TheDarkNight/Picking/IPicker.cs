using UniRx;

namespace TheDarkNight.Picking {

    public interface IPicker {

        IObservable<IPickable> Picking { get; }

        void CanPickupPickable(IPickable pickable);

        void CannotPickupPickable(IPickable pickable);

        void PickUpPickable();
    }
}