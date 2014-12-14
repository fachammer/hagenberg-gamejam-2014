using TheDarkNight.Extensions;
using TheDarkNight.Observables.Input;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(IAxesManager))]
    [RequireComponent(typeof(ILightBulbInserter))]
    public class LightBulbInserterController : MonoBehaviour {
        private ILightBulbInserter inserter;

        [SerializeField]
        private string insertAxisName;

        private IAxesManager axesManager;

        private void Awake() {
            inserter = this.TryGetClass<ILightBulbInserter>();
            axesManager = this.TryGetClass<IAxesManager>();
        }

        private void Start() {
            IObservableAxis insertAxis = axesManager.GetAxis(insertAxisName);
            insertAxis.Subscribe(HandleInsertAxis);
        }

        private void HandleInsertAxis(float axisValue) {
            if(axisValue > 0)
                inserter.TryInsertLightBulb();
        }
    }
}