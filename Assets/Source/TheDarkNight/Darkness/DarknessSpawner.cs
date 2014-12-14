using UnityEngine;
using System.Collections;
using ModestTree.Zenject;


namespace TheDarkNight.Darkness {
    public class DarknessSpawner : MonoBehaviour {

        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        public float cooldown = 5f;
        public DarknessSpawnpoint[] spawnpos;
        public GameObject darknessPrefab;
        float lastSpawn = 0;

        void Update() {
            if(UnityEngine.Time.time >= lastSpawn + cooldown) {

                DarknessSpawnpoint dsp =spawnpos[UnityEngine.Random.Range(0, spawnpos.Length)];
                Darkness d = GOI.Instantiate(darknessPrefab).GetComponent<Darkness>(); 
                d.transform.position = dsp.transform.position;
                d.transform.rotation = dsp.transform.rotation;
                d.SetValues(dsp.targetroom);
                lastSpawn = Time.time;
            }
        }
    }
}
