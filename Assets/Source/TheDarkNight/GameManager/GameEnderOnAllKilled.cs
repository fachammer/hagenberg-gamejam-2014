using System.Linq;
using TheDarkNight.Darkness;
using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.GameManager {

    [RequireComponent(typeof(IGameManager))]
    public class GameEnderOnAllKilled : MonoBehaviour {

        [SerializeField]
        private Killable[] killables;

        private void Start() {
            IGameManager gameManager = this.TryGetClass<IGameManager>();
            killables
                .Select(k => k.Killed as IObservable<bool>)
                .Merge()
                .SkipWhile((_, i) => i < killables.Count() - 1)
                .Subscribe(_ => gameManager.EndGame());
        }
    }
}