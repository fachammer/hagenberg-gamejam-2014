namespace TheDarkNight.Lights {

    public interface ILightSource {
        bool TryTurnOn();
        bool TryInsertLightBulb(ILightBulb lightBulb);
    }
}
