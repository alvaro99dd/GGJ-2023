using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : MonoBehaviour {
    public float minTime, maxTime, earthquakeTime, stunDuration;
    public bool earthquake;
    public ParticleSystem earthquakePSystem;
    public GameObject mCamera;

    private void Start() {
    //var main = earthquakePSystem.main;
    //main.duration = earthquakeTime;

        if (GameManager.instance.currentMiniGame == MiniGames.Regar) {
            StartCoroutine(randomQuake());
        }
    }

    IEnumerator randomQuake() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            //particulas terremoto
            mCamera.SetActive(false);
            //particulas terremoto
            earthquakePSystem.Play();
            earthquake = true;
            yield return new WaitForSeconds(earthquakeTime);
            earthquake = false;
            mCamera.SetActive(true);
        }
    }
}
