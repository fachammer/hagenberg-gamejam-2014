using UnityEngine;
using System.Collections;
using ModestTree.Zenject;
using UniRx;


namespace TheDarkNight.Darkness {
    public class DarknessSpawner : MonoBehaviour {

        public GameManager.GameManager gameManager;

        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        public float cooldown = 5f;
        public DarknessSpawnpoint[] spawnpos;
        public GameObject darknessPrefab;
        private float lastSpawn = 0;
        private bool spawning = false;


        void Start() {
            gameManager.GameStarted.Subscribe(started => this.spawning = started);
        }

        void Update() {
            if(spawning && UnityEngine.Time.time >= lastSpawn + cooldown) {

                DarknessSpawnpoint dsp =spawnpos[UnityEngine.Random.Range(0, spawnpos.Length)];
                Darkness d = GOI.Instantiate(darknessPrefab).GetComponent<Darkness>(); 
                d.transform.position = dsp.transform.position;
                d.SetValues(dsp.targetroom);
                lastSpawn = Time.time;
            }
        }
    }
}
