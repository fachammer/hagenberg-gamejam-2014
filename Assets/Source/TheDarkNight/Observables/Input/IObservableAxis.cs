using UniRx;

namespace TheDarkNight.Observables.Input {

    public interface IObservableAxis : IObservable<float> {

        string GetName();
    }
}