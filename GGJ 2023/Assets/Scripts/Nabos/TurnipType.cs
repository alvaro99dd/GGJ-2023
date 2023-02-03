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

    private void Awake() {
        Setup();
    }

    void Setup() {
        int randomValue = Random.Range(0, 4);
        switch (randomValue) {
            case 0:
                bodySize = BodySize.Small;
                break;
            case 1:
                bodySize = BodySize.Normal;
                break;
            case 2:
                bodySize = BodySize.Big;
                break;
            case 3:
                bodySize = BodySize.Giant;
                break;
            default:
                break;
        }
    }

    public IEnumerator GetGrabZone() {
        yield return new WaitForSeconds(0.3f);
        trigger.enabled = true;
    }

    private void OnCollisionEnter(Collision collision) {
        bool isPlayer1;
        if (collision.collider.CompareTag("BasePlayer1")) {
            
        }
    }
}
