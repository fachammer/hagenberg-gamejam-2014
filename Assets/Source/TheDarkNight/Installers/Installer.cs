using ModestTree.Zenject;
using PRO3.Observable.Time;
using UnityEngine;

namespace TheDarkNight.Installers {

    public class Installer : MonoInstaller {

        [SerializeField]
        private ObservableTime observableTimePrefab;

        public override void InstallBindings() {
            Container.Bind<IObservableTime>().ToSingleFromPrefab<ObservableTime>(observableTimePrefab.gameObject);
        }
    }
}