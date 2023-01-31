using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour {
    public string objecTag;
    public bool objectGrabbed;
    public bool water;

    public float waterCount, waterToWin;
    
    PlayerController pC;

    private void Start() {
        pC = GetComponentInParent<PlayerController>();
    }

    void CheckObject(Transform grabbingObject) {
        switch (grabbingObject.tag) {
            case "Water":
                water = true;
                objecTag = grabbingObject.tag;
                objectGrabbed = true;
                break;
            default:
                break;
        }
    }

    public void DropObject() {
        switch (objecTag) {
            case "Water":
                water = false;
                objectGrabbed = false;
                WaterBehaviour();
                break;
            default:
                break;
        }
    }

    void WaterBehaviour() {
        if (++waterCount >= waterToWin) {
            Debug.Log(transform.parent.tag + " Wins!");
        }
    }

    public void ObjectFalledOff() {
        objectGrabbed = false;
        switch (objecTag) {
            case "Water":
                water = false;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!objectGrabbed && pC.grabZone) {
            CheckObject(other.transform);
        }
    }
}