using TheDarkNight.Lights;

namespace TheDarkNight.Lights {

    public interface ISwitcher {
        void CanToggleSwitch(ISwitch lightSwitch);
        void CannotToggleSwitch();
        bool ToggleSwitch();
    }
}
