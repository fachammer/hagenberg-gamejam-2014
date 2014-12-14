using UniRx;

namespace TheDarkNight.Picking {

    public interface IPicker {

        IObservable<IPickable> CanPickup { get; }

        IObservable<IPickable> Picking { get; }

        void CanPickupPickable(IPickable pickable);

        void CannotPickupPickable();

        void TryPickUpPickable();
    }
}