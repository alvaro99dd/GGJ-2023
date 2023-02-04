using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableSpawner : MonoBehaviour {
    public GameObject[ ] VegetablePrefabs;
    public Transform limit1, limit2;
    public Transform player1StartingPos, player2StartingPos;
    public float minTime, maxTime;
    public int minQuantity, maxQuantity;

    private void Start() {
        if (GameManager.instance.currentMiniGame != MiniGames.Huertos) {
            return;
        }
        SetStartingPos();
        StartCoroutine(randomSpawn());
    }

    void SetStartingPos() {
        GameObject.FindGameObjectWithTag("Player1").transform.position = player1StartingPos.position; 
        GameObject.FindGameObjectWithTag("Player2").transform.position = player2StartingPos.position; 
    }

    IEnumerator randomSpawn() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            for (int i = 0; i < Random.Range(minQuantity, maxQuantity); i++) {
                Vector3 randomPos = new Vector3(Random.Range(limit1.position.x, limit2.position.x), Random.Range(limit1.position.y, limit2.position.y), Random.Range(limit1.position.z, limit2.position.z));
                Instantiate(VegetablePrefabs[Random.Range(0, VegetablePrefabs.Length)], randomPos, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            }
        }
    }
}
