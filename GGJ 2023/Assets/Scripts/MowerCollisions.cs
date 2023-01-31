using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MowerCollisions : MonoBehaviour
{
    LawnMower lM;

    private void Start() {
        lM = GameObject.Find("MiniGameEvents").GetComponent<LawnMower>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player1") || collision.transform.CompareTag("Player2")) {
            Vector3 randomPos = new Vector3(Random.Range(lM.limit1.position.x, lM.limit2.position.x), Random.Range(lM.limit1.position.y, lM.limit2.position.y), Random.Range(lM.limit1.position.z, lM.limit2.position.z));
            lM.randomDir = randomPos - lM.rb1.position;
            lM.randomDir.y = -0.5f;
            lM.pC = collision.gameObject.GetComponent<PlayerController>();
            StartCoroutine(lM.StunBehaviour());
        }
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Vegetable")) {
            Vector3 randomPos = new Vector3(Random.Range(lM.limit1.position.x, lM.limit2.position.x), Random.Range(lM.limit1.position.y, lM.limit2.position.y), Random.Range(lM.limit1.position.z, lM.limit2.position.z));
            lM.randomDir = randomPos - lM.rb1.position;
            lM.randomDir.y = -0.5f;
        }
    }
}
