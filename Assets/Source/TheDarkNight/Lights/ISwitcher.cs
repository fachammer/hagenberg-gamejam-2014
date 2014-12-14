using UniRx;

namespace TheDarkNight.Lights {

    public interface ISwitcher {

        IObservable<ISwitch> ToggleableSwitch { get; }

        void CanToggleSwitch(ISwitch lightSwitch);

        void CannotToggleSwitch();

        bool ToggleSwitch();
    }
}