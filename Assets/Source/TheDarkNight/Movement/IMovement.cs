using UniRx;

namespace TheDarkNight.Movement {

    public interface IMovement {

        IObservable<float> HorizontalMovement { get; }

        IObservable<float> DepthMovement { get; }

        void MoveHorizontally(float movementScale);

        void MoveDepth(float movementScale);
    }
}