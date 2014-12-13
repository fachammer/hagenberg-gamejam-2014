using UniRx;

namespace TheDarkNight.Darkness {

    public interface IKillable {

        IObservable<Unit> Killed { get; }
    }
}