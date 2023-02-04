using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnipRandomizer : MonoBehaviour {
    public Transform campoNabos;
    public Transform player1StartingPos, player2StartingPos;

    private void Start() {
        if (GameManager.instance.currentMiniGame == MiniGames.Nabos) {
            GetRandomTurnips();
            SetStartingPos();
        }
    }

    void SetStartingPos() {
        GameObject.FindGameObjectWithTag("Player1").transform.position = player1StartingPos.position;
        GameObject.FindGameObjectWithTag("Player2").transform.position = player2StartingPos.position;
    }

    void GetRandomTurnips() {
        BodySize turnipBody;
        Transform turnip;
        for (int i = 0; i < campoNabos.childCount; i++) {
            turnip = campoNabos.GetChild(i);
            turnipBody = turnip.GetComponent<TurnipType>().bodySize;
            turnip.GetChild(0).GetChild(Random.Range(0, 4)).gameObject.SetActive(true);
            switch (turnipBody) {
                case BodySize.Small:
                    turnip.GetChild(1).GetChild(3).gameObject.SetActive(true);
                    break;
                case BodySize.Normal:
                    turnip.GetChild(1).GetChild(1).gameObject.SetActive(true);
                    break;
                case BodySize.Big:
                    turnip.GetChild(1).GetChild(0).gameObject.SetActive(true);
                    break;
                case BodySize.Giant:
                    turnip.GetChild(1).GetChild(2).gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
