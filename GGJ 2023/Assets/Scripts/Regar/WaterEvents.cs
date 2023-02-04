using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEvents : MonoBehaviour {
    public GameObject waterPrefab;
    [SerializeField] float minTime, maxTime, speed;
    [SerializeField] Transform limit1, limit2;
    GameObject tempWater, tempWater2;
    Vector3 randomDir, randomDir2;
    Coroutine movement;
    bool spawningWater = false;
    public Rigidbody rb1, rb2;
    public Transform player1StartingPos, player2StartingPos;

    private void Start() {
        if (GameManager.instance.currentMiniGame != MiniGames.Regar) {
            return;
        }
        firstRandomSpawn();
        SetStartingPos();
        movement = StartCoroutine(randomMovement());
    }

    private void FixedUpdate() {
        if (GameManager.instance.currentMiniGame != MiniGames.Regar) {
            return;
        }
        if (!spawningWater) {
            Movement(randomDir, randomDir2);
        }
    }

    void SetStartingPos() {
        GameObject.FindGameObjectWithTag("Player1").transform.position = player1StartingPos.position;
        GameObject.FindGameObjectWithTag("Player2").transform.position = player2StartingPos.position;
    }

    void firstRandomSpawn() {
        Vector3 waterPosition = new Vector3(rb1.position.x, -1f, rb1.position.z);
        Vector3 waterPosition2 = new Vector3(rb2.position.x, -1f, rb2.position.z);
        tempWater = Instantiate(waterPrefab, waterPosition, Quaternion.identity);
        tempWater2 = Instantiate(waterPrefab, waterPosition2, Quaternion.identity);
        StartCoroutine(randomSpawn());
    }

    IEnumerator randomSpawn() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            StopCoroutine(movement);
            rb1.velocity = Vector3.zero;
            rb2.velocity = Vector3.zero;
            spawningWater = true;

            Destroy(tempWater);
            Destroy(tempWater2);
            Vector3 waterPosition = new Vector3(rb1.position.x, -1f, rb1.position.z);
            Vector3 waterPosition2 = new Vector3(rb2.position.x, -1f, rb2.position.z);
            tempWater = Instantiate(waterPrefab, waterPosition, Quaternion.identity);
            tempWater2 = Instantiate(waterPrefab, waterPosition2, Quaternion.identity);

            yield return new WaitForSeconds(1f);
            movement = StartCoroutine(randomMovement());
            spawningWater = false;
        }
    }

    void Movement(Vector3 dir, Vector3 dir2) {
        rb1.velocity = dir.normalized * speed * Time.fixedDeltaTime;
        rb2.velocity = dir.normalized * speed * Time.fixedDeltaTime;
    }

    IEnumerator randomMovement() {
        Vector3 randomPos = new Vector3(Random.Range(limit1.position.x, limit2.position.x), Random.Range(limit1.position.y, limit2.position.y), Random.Range(limit1.position.z, limit2.position.z));
        randomDir = randomPos - rb1.position;
        randomDir2 = randomPos - rb2.position;
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            randomPos = new Vector3(Random.Range(limit1.position.x, limit2.position.x), Random.Range(limit1.position.y, limit2.position.y), Random.Range(limit1.position.z, limit2.position.z));
            randomDir = randomPos - rb1.position;
            randomDir2 = randomPos - rb2.position;
        }
    }
}
