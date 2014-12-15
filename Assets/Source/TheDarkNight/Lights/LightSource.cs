using ModestTree.Zenject;
using TheDarkNight.Extensions;
using TheDarkNight.Observables.Time;
using TheDarkNight.Rooms;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(Collider))]
    public class LightSource : MonoBehaviour, ILightSource {
        private ISubject<ILightBulb> newBulb = new Subject<ILightBulb>();

        private ISubject<Unit> bulbDestroyed = new Subject<Unit>();

        private ISubject<ILightSource> turnOn = new Subject<ILightSource>();

        private ISubject<ILightSource> turnOff = new Subject<ILightSource>();

        private ISubject<Unit> lightBulbDestroyed = new Subject<Unit>();


        [SerializeField]
        private Transform lightBulbTransform;

        public LightBulb lightBulb;
        public GameObject sparks;

        [Inject]
        public IObservableTime Time { get; set; }

        public IObservable<ILightBulb> NewBulb { get { return newBulb; } }
        public IObservable<Unit> BulbDestroyed { get { return bulbDestroyed; } }
        public IObservable<ILightSource> TurnedOn { get { return turnOn; } }
        public IObservable<ILightSource> TurnedOff { get { return turnOff; } }


        public IObservable<Unit> LightBulbDestroyed {
            get { return lightBulbDestroyed; }
        }

        public bool CanTurnOn() {
            return lightBulb != null && lightBulb.CanTurnOn();
        }

        public bool CanTurnOff() {
            return lightBulb != null && lightBulb.CanTurnOff();
        }

        public void TurnOn() {
            lightBulb.TurnOn();
            turnOn.OnNext(this);
        }

        public void TurnOff() {
            lightBulb.TurnOff();
            turnOff.OnNext(this);
        }

        public void InsertLightBulb(LightBulb lightBulb) {
            Time.Once(1.5f).Subscribe(_ => {
                lightBulb.GetTransform().parent = transform;
                lightBulb.GetTransform().position = lightBulbTransform.position;
                lightBulb.GetTransform().rotation = lightBulbTransform.rotation;
                this.lightBulb = lightBulb;
                newBulb.OnNext(lightBulb);
            });
        }

        public Transform GetTransform() {
            return transform;
        }

        public bool CanInsert(LightBulb lightBulb) {
            return this.lightBulb == null;
        }

        private void OnTriggerEnter(Collider collider) {
            ILightBulbInserter inserter = collider.GetClass<ILightBulbInserter>();
            if(inserter != null)
                inserter.CanInsertLightBulb(this);
        }

        private void OnTriggerExit(Collider collider) {
            ILightBulbInserter inserter = collider.GetClass<ILightBulbInserter>();
            if(inserter != null)
                inserter.CannotInsertLightBulb(this);
        }

        private void Start() {
            collider.isTrigger = true;
            lightBulb = null;
        }


        public void SetBulbNull() {
            Destroy(Instantiate(sparks, lightBulbTransform.position, Quaternion.Euler(90, 0, 0)), 2);

            bulbDestroyed.OnNext(Unit.Default);
            lightBulbDestroyed.OnNext(Unit.Default);
            GetComponent<AudioSource>().Play();
            this.lightBulb = null;
        }
    }
}