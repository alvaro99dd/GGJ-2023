using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : MonoBehaviour {
    public float minTime, maxTime, earthquakeTime, stunDuration;
    public bool earthquake;
    public ParticleSystem earthquakePSystem;
    public GameObject mCamera;
    public Animator molinoAnim;

    private void Start() {
    //var main = earthquakePSystem.main;
    //main.duration = earthquakeTime;

        if (GameManager.instance.currentMiniGame == MiniGames.Regar) {
            StartCoroutine(randomQuake());
        }
    }

    IEnumerator randomQuake() {
        float timer = Random.Range(minTime, maxTime);
        while (true) {
            yield return new WaitForSeconds(timer - 1f);
            //particulas terremoto
            mCamera.SetActive(false);
            //particulas terremoto

            earthquakePSystem.Play();
            SoundManager.instance.aS.PlayOneShot(SoundManager.instance.terremoto);
            molinoAnim.SetBool("terremoto", true);
            yield return new WaitForSeconds(1f);
            earthquake = true;
            yield return new WaitForSeconds(earthquakeTime);
            earthquake = false;
            molinoAnim.SetBool("terremoto", false);
            mCamera.SetActive(true);
        }
    }
}
