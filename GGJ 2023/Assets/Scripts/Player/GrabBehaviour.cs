using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour {
    bool filledHuerto;
    public string objecTag;
    public bool objectGrabbed;
    public bool water, vegetable;

    GameObject vegetablePlaceHolder;
    public int waterCount, waterToWin;
    public List<Transform> vegetablePos;
    public Transform huerto;
    public string huertoName;

    PlayerController pC;

    private void Start() {
        pC = GetComponentInParent<PlayerController>();
        vegetablePlaceHolder = transform.GetChild(0).gameObject;
    }

    private void Update() {
        HuertoSetup();
    }



    void CheckObject(Transform grabbingObject) {
        switch (grabbingObject.tag) {
            case "Water":
                water = true;
                objecTag = grabbingObject.tag;
                objectGrabbed = true;
                break;
            case "Vegetable":
                if (grabbingObject.parent.TryGetComponent(out PlayerVegetable pV) && pV.playerTag == transform.parent.tag) {
                    return;
                } else if (grabbingObject.parent.TryGetComponent(out PlayerVegetable pV1) && pV1.playerTag != transform.parent.tag) {
                    if (transform.parent.CompareTag("Player1")) {
                        GameManager.instance.vegetablesP2--;
                    } else {
                        GameManager.instance.vegetablesP1--;
                    }
                }
                vegetable = true;
                objecTag = grabbingObject.tag;
                objectGrabbed = true;
                GrabVegetable(grabbingObject);
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
            case "Vegetable":
                VegetableBehaviour();
                break;
            default:
                break;
        }
    }
    #region Huerto
    void HuertoSetup() {
        if (GameManager.instance.currentMiniGame == MiniGames.Huertos && !filledHuerto) {
            filledHuerto = true;
            huerto = GameObject.Find("Huerto" + transform.parent.tag).transform.GetChild(0);
            for (int i = 0; i < huerto.childCount; i++) {
                vegetablePos.Add(huerto.GetChild(i));
            }
        }
    }

    void GrabVegetable(Transform vegetable) {
        vegetablePlaceHolder.SetActive(true);
        if (!vegetable.parent.TryGetComponent(out PlayerVegetable pV)) {
            Destroy(vegetable.gameObject);
        } else {
            vegetable.parent.gameObject.SetActive(false);
        }
        vegetable.rotation = Quaternion.identity;
    }


    void VegetableBehaviour() {
        if (huerto.parent.name != huertoName) {
            return;
        }
        vegetable = false;
        objectGrabbed = false;
        vegetablePlaceHolder.SetActive(false);
        if (transform.parent.CompareTag("Player1")) {
            vegetablePos[GameManager.instance.vegetablesP1++].GetChild(0).gameObject.SetActive(true);

            if (GameManager.instance.vegetablesP1 >= vegetablePos.Count) Debug.Log(transform.parent.tag + " Wins!");
        } else {
            vegetablePos[GameManager.instance.vegetablesP2++].GetChild(0).gameObject.SetActive(true);

            if (GameManager.instance.vegetablesP2 >= vegetablePos.Count) Debug.Log(transform.parent.tag + " Wins!");
        }

    }
    #endregion

    void WaterBehaviour() {
        if (++waterCount >= waterToWin) Debug.Log(transform.parent.tag + " Wins!");
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