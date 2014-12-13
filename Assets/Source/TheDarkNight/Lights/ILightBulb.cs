namespace TheDarkNight.Lights {
    public interface ILightBulb {
        bool TryTurnOn();
        bool TryTurnOff();
        void Destroy();
    }
}