namespace TheDarkNight.FlashLight {

    public interface IFlashLight {

        bool TryTurnOn();

        bool TryTurnOff();

        void Toggle();

        float GetBatteryLoad();
    }
}