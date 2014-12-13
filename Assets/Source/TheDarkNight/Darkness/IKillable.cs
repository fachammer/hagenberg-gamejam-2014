using TheDarkNight.Utility;

namespace TheDarkNight.Darkness {

    public interface IKillable {

        ObservableProperty<bool> Killed { get; }

        void Kill();
    }
}