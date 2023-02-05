using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawnMower : MonoBehaviour
{
    public float turnSmoothVelocity, turnSmoothTime;
    public float speed, stunDuration;
    public Collider trigger;
    Transform raizPlayer1, raizPlayer2;
    Rigidbody rB;
    public Vector3 dir;
    Vector3 velocity;

    private void Start() {
        raizPlayer1 = GameObject.Find("RaizPlayer1").transform;
        raizPlayer2 = GameObject.Find("RaizPlayer2").transform;

        rB = GetComponent<Rigidbody>();
        dir = GetRandomDirection();
    }

    private void FixedUpdate() {
        LawnMovement();
    }

    void LawnMovement() {
        rB.velocity = dir.normalized * speed * Time.deltaTime;
        transform.forward = Vector3.SmoothDamp(transform.forward, dir, ref velocity, turnSmoothTime);
        //transform.LookAt(rB.velocity);
    }

    public Vector3 GetRandomDirection() {
        Vector3 randomDir = Vector3.zero;
        int randomRoot = Random.Range(0, 2);
        switch (randomRoot) {
            case 0:
                randomDir = raizPlayer1.GetChild(Random.Range(0, raizPlayer1.childCount)).position - transform.position;
                break;
            case 1:
                randomDir = raizPlayer2.GetChild(Random.Range(0, raizPlayer2.childCount)).position - transform.position;
                break;
        }
        return randomDir;
    }

    public IEnumerator GetGrabZone() {
        yield return new WaitForSeconds(0.3f);
        trigger.enabled = true;
    }

    private void OnCollisionEnter(Collision collision) {
        bool isPlayer1;
        if (collision.collider.CompareTag("VegetablePlayer1")) {
            //particulas romper raices
            isPlayer1 = true;
            GameManager.instance.CheckRoots(isPlayer1);
            collision.gameObject.SetActive(false);
            dir = GetRandomDirection();
        } else if (collision.collider.CompareTag("VegetablePlayer2")) {
            //particulas romper raices
            isPlayer1 = false;
            GameManager.instance.CheckRoots(isPlayer1);
            collision.gameObject.SetActive(false);
            dir = GetRandomDirection();
        }

        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Mower")) {
            dir = GetRandomDirection();
        }

        if (collision.transform.CompareTag("Player1") || collision.transform.CompareTag("Player2")) {
            collision.gameObject.GetComponent<PlayerController>().StartCoroutine(collision.gameObject.GetComponent<PlayerController>().StunPlayer(stunDuration));
            dir = GetRandomDirection();
        }
    }
}