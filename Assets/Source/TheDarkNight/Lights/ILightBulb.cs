namespace TheDarkNight.Lights {
    public interface ILightBulb {
        bool CanTurnOn();
        bool CanTurnOff();
        void TurnOn();
        void TurnOff();
        void Destroy();
    }
}