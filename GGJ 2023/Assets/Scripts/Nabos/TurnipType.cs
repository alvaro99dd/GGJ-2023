using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodySize {
    Small, Normal, Big, Giant
}

public class TurnipType : MonoBehaviour
{
    public BodySize bodySize;
    public Collider trigger;
    int score;
    bool hasCollided;

    private void Awake() {
        Setup();
    }

    void Setup() {
        trigger = GetComponent<SphereCollider>();
        int randomValue = Random.Range(0, 4);
        switch (randomValue) {
            case 0:
                bodySize = BodySize.Small;
                score = 1;
                break;
            case 1:
                bodySize = BodySize.Normal;
                score = 2;
                break;
            case 2:
                bodySize = BodySize.Big;
                score = 3;
                break;
            case 3:
                bodySize = BodySize.Giant;
                score = 5;
                break;
            default:
                break;
        }
    }

    public IEnumerator GetGrabZone() {
        trigger.enabled = false;
        yield return new WaitForSeconds(0.3f);
        trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider collision) {
        bool isPlayer1 = true;
        if (collision.name == "BasePlayer1" && !hasCollided) {
            hasCollided = true;
            GameManager.instance.CheckTurnips(isPlayer1, score);
        } else if (collision.name == "BasePlayer2" && !hasCollided) {
            hasCollided = true;
            GameManager.instance.CheckTurnips(!isPlayer1, score);
        }
    }

    private void OnTriggerExit(Collider other) {
        bool isPlayer1 = true;
        if (other.name == "BasePlayer1" && hasCollided) {
            hasCollided = false;
            GameManager.instance.DeleteTurnips(isPlayer1, score);
        } else if (other.name == "BasePlayer2" && hasCollided) {
            hasCollided = false;
            GameManager.instance.DeleteTurnips(!isPlayer1, score);
        }
    }
}
