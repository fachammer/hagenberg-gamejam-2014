using TheDarkNight.Picking;
namespace TheDarkNight.Lights {
    public interface ILightBulb : IPickable {
        bool CanTurnOn();
        bool CanTurnOff();
        void TurnOn();
        void TurnOff();
        void Destroy();
    }
}