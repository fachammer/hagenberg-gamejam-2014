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

        [SerializeField]
        private string insertJoystickAxisName;

        private IAxesManager axesManager;

        private void Awake() {
            inserter = this.TryGetClass<ILightBulbInserter>();
            axesManager = this.TryGetClass<IAxesManager>();
        }

        private void Start() {
            IObservableAxis insertAxis = axesManager.GetAxis(insertAxisName);
            IObservableAxis insertAxisJoystick = axesManager.GetAxis(insertJoystickAxisName);
            if(Input.GetJoystickNames().Length < 2)
                insertAxis.DistinctUntilChanged().Subscribe(HandleInsertAxis);
            else
                insertAxisJoystick.DistinctUntilChanged().Subscribe(HandleInsertAxis);
        }

        private void HandleInsertAxis(float axisValue) {
            if(axisValue > 0)
                inserter.TryInsertLightBulb();
        }
    }
}