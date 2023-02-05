using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour {
    public Transform initialPos1, initialPos2;

    private void Start() {
        if (GameManager.instance.currentMiniGame == MiniGames.Lobby && GameManager.instance.comeBack) {
            GameObject.FindGameObjectWithTag("Player1").transform.position = initialPos1.position;
            GameObject.FindGameObjectWithTag("Player2").transform.position = initialPos2.position;
            GameManager.instance.comeBack = false;
        }
    }
}
