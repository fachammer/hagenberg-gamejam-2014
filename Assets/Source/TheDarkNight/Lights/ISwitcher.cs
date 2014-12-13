using TheDarkNight.Lights;

namespace TheDarkNight.Lights {

    public interface ISwitcher {
        void CanToggleSwitch(ISwitch lightSwitch);
        void CannotToggleSwitch(ISwitch lightSwitch);
        bool ToggleSwitch();
    }
}
