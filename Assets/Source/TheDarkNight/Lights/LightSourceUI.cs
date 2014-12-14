using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.TheDarkNight.Lights {

    [RequireComponent(typeof(Image))]
    public class LightSourceUI : MonoBehaviour {
        private Image image;

        [SerializeField]
        private LightBulbInserter inserter;

        private ILightSource lightSource;

        private bool bulbInserted = false;

        private void Awake() {
            image = this.TryGetComponent<Image>();
            lightSource = this.TryGetClassInParent<ILightSource>();
        }

        private void Start() {
            lightSource.LightBulbDestroyed.Subscribe(_ => bulbInserted = false);
            inserter.Insertable.Subscribe(HandleInsertable);
            inserter.InsertedLightBulb.Where(ls => ls == lightSource).Subscribe(_ => {
                bulbInserted = true;
                image.enabled = false;
            });
        }

        private void HandleInsertable(ILightSource lightSource) {
            image.enabled = !bulbInserted && lightSource != null && lightSource == this.lightSource;
        }
    }
}