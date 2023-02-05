using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawnMowerWater : MonoBehaviour {
    public float turnSmoothVelocity, turnSmoothTime;
    public float minTime, maxTime, speed, stunDuration;
    public Transform limit1, limit2;
    public Rigidbody rb1, rb2;
    public Vector3 randomDir;
    public PlayerController pC;

    Vector3 velocity;

    private void Start() {
        //rb1 = GetComponent<Rigidbody>();
        if (GameManager.instance.currentMiniGame != MiniGames.Regar) {
            return;
        }
        StartCoroutine(randomMovement());
    }

    private void FixedUpdate() {
        if (GameManager.instance.currentMiniGame != MiniGames.Regar) {
            return;
        }
        rb1.velocity = randomDir.normalized * speed * Time.fixedDeltaTime;
        rb2.velocity = randomDir.normalized * speed * Time.fixedDeltaTime;

        rb1.transform.forward = Vector3.SmoothDamp(rb1.transform.forward, randomDir, ref velocity, turnSmoothTime);
        rb2.transform.forward = rb1.transform.forward;
        //rb1.position = new Vector3(rb1.position.x, -0.5f, rb1.position.z);
    }

    IEnumerator randomMovement() {
        Vector3 randomPos = new Vector3(Random.Range(limit1.position.x, limit2.position.x), Random.Range(limit1.position.y, limit2.position.y), Random.Range(limit1.position.z, limit2.position.z));
        randomDir = randomPos - rb1.position;
        randomDir.y = -0.5f;
        //rb1.transform.forward = randomDir;
        //rb2.transform.forward = rb1.transform.forward;
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            randomPos = new Vector3(Random.Range(limit1.position.x, limit2.position.x), Random.Range(limit1.position.y, limit2.position.y), Random.Range(limit1.position.z, limit2.position.z));
            randomDir = randomPos - rb1.position;
            randomDir.y = -0f;
            //rb1.transform.forward = randomDir;
            //rb2.transform.forward = rb1.transform.forward;
        }
    }

    public IEnumerator StunBehaviour() {
        pC.stun = true;
        //particulas stun aparecen
        pC.stunPSystem.Play();
        pC.gB.ObjectFalledOff();
        SoundManager.instance.aS.PlayOneShot(SoundManager.instance.stun);
        yield return new WaitForSeconds(stunDuration);
        //particulas stun fuera
        pC.stun = false;
    }



}
