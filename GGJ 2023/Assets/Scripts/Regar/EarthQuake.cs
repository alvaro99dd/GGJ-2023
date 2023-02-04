using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : MonoBehaviour {
    public float minTime, maxTime, earthquakeTime, stunDuration;
    public bool earthquake;

    private void Start() {
        if (GameManager.instance.currentMiniGame == MiniGames.Regar) {
            StartCoroutine(randomQuake());
        }
    }

    IEnumerator randomQuake() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            //particulas terremoto
            earthquake = true;
            yield return new WaitForSeconds(earthquakeTime);
            earthquake = false;
        }
    }
}
