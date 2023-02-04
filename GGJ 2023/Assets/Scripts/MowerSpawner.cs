using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MowerSpawner : MonoBehaviour {
    public float minTime, maxTime;
    public GameObject mowerPrefab;
    public Transform limit1, limit2, rootZone1, rootZone2, rootZone3, rootZone4;
    public Transform player1StartingPos, player2StartingPos;

    bool isInPlayer1Root(Vector3 randomPos) => Mathf.Abs(rootZone1.position.x) > Mathf.Abs(randomPos.x) && Mathf.Abs(rootZone2.position.x) < Mathf.Abs(randomPos.x) && rootZone1.position.z > randomPos.z && rootZone2.position.z<randomPos.z;
    bool isInPlayer2Root(Vector3 randomPos) => Mathf.Abs(rootZone3.position.x) > Mathf.Abs(randomPos.x) && Mathf.Abs(rootZone4.position.x) < Mathf.Abs(randomPos.x) && rootZone3.position.z > randomPos.z && rootZone4.position.z < randomPos.z;

    private void Start() {
        if (GameManager.instance.currentMiniGame == MiniGames.Podadoras) {
            StartCoroutine(randomSpawn());
            SetStartingPos();
        }
    }

    void SetStartingPos() {
        GameObject.FindGameObjectWithTag("Player1").transform.position = player1StartingPos.position;
        GameObject.FindGameObjectWithTag("Player2").transform.position = player2StartingPos.position;
    }

    IEnumerator randomSpawn() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            for (int i = 0; i < 2; i++) {
                Vector3 randomPos = new Vector3(Random.Range(limit1.position.x, limit2.position.x), Random.Range(limit1.position.y, limit2.position.y), Random.Range(limit1.position.z, limit2.position.z));
                CheckSpawn(randomPos);
            }
        }
    }

    void CheckSpawn(Vector3 randomPos) {
        if ((Mathf.Abs(rootZone1.position.x) > Mathf.Abs(randomPos.x) && Mathf.Abs(rootZone2.position.x) < Mathf.Abs(randomPos.x)) && (rootZone1.position.z > randomPos.z && rootZone2.position.z < randomPos.z) || (Mathf.Abs(rootZone3.position.x) > Mathf.Abs(randomPos.x) && Mathf.Abs(rootZone4.position.x) < Mathf.Abs(randomPos.x)) && (rootZone3.position.z > randomPos.z && rootZone4.position.z < randomPos.z)) {
            Debug.Log("golpea");
            randomPos = new Vector3(Random.Range(limit1.position.x, limit2.position.x), Random.Range(limit1.position.y, limit2.position.y), Random.Range(limit1.position.z, limit2.position.z));
            CheckSpawn(randomPos);
        } else {
            Instantiate(mowerPrefab, randomPos, Quaternion.identity);
        }

        //if (isInPlayer1Root(randomPos) || isInPlayer2Root(randomPos)) {
        //    Debug.Log("golpea");
        //    randomPos = new Vector3(Random.Range(limit1.position.x, limit2.position.x), Random.Range(limit1.position.y, limit2.position.y), Random.Range(limit1.position.z, limit2.position.z));
        //    CheckSpawn(randomPos);
        //}
    }
}
