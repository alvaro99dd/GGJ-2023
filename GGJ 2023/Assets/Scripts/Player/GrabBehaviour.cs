using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour {
    bool filledHuerto;
    public string objectTag;
    public bool objectGrabbed;
    public bool water, vegetable, turnip;

    GameObject vegetablePlaceHolder, mowerPlaceHolder;
    public int waterCount, waterToWin;
    public List<Transform> vegetablePos;
    public Transform huerto;
    public string huertoName;

    PlayerController pC;

    private void Start() {
        pC = GetComponentInParent<PlayerController>();
        vegetablePlaceHolder = transform.GetChild(0).gameObject;
        mowerPlaceHolder = transform.GetChild(1).gameObject;
    }

    private void Update() {
        HuertoSetup();
    }



    void CheckObject(Transform grabbingObject) {
        switch (grabbingObject.tag) {
            case "Water":
                water = true;
                objectTag = grabbingObject.tag;
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
                objectTag = grabbingObject.tag;
                objectGrabbed = true;
                GrabVegetable(grabbingObject);
                break;
            case "Mower":
                GrabMower(grabbingObject);
                break;
            case "Turnip":
                objectGrabbed = true;
                GrabTurnip(grabbingObject);
                break;
            default:
                break;
        }
    }

    public void DropObject() {
        switch (objectTag) {
            case "Water":
                water = false;
                objectGrabbed = false;
                WaterBehaviour();
                break;
            case "Vegetable":
                VegetableBehaviour();
                break;
            case "Mower":
                MowerBehaviour();
                break;
            case "Turnip":
                TurnipBehaviour();
                break;
            default:
                break;
        }
    }

    void GrabTurnip(Transform grabbingObject) {
        objectTag = grabbingObject.tag;
        pC.grabZone = false;
        grabbingObject.position = transform.position;
        grabbingObject.SetParent(transform);
        //grabbingObject.GetComponent<TurnipType>().trigger.enabled = false;
        //grabbingObject.GetComponentInChildren<Rigidbody>().useGravity = false;
        //grabbingObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        //grabbingObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    void TurnipBehaviour() {
        objectGrabbed = false;
        transform.GetChild(2).position = transform.position + transform.forward;
        //transform.GetChild(2).GetComponentInChildren<Rigidbody>().useGravity = true;
        //transform.GetChild(2).GetComponentInChildren<CapsuleCollider>().enabled = true;
        transform.GetChild(2).GetComponent<TurnipType>().StartCoroutine(transform.GetChild(2).GetComponent<TurnipType>().GetGrabZone());
        transform.GetChild(2).SetParent(null);
    }

    void GrabMower(Transform grabbingObject) {
        objectTag = grabbingObject.tag;
        grabbingObject.GetComponent<LawnMower>().trigger.enabled = false;
        pC.grabZone = false;
        objectGrabbed = true;
        grabbingObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        grabbingObject.position = transform.position + transform.forward*2;
        grabbingObject.SetParent(transform);
        grabbingObject.gameObject.SetActive(false);
        mowerPlaceHolder.SetActive(true);
    }

    void MowerBehaviour() {
        objectGrabbed = false;
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).GetComponent<LawnMower>().dir = transform.forward;
        transform.GetChild(2).GetComponent<LawnMower>().StartCoroutine(transform.GetChild(2).GetComponent<LawnMower>().GetGrabZone());
        transform.GetChild(2).SetParent(null);
        mowerPlaceHolder.SetActive(false);
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
            for (int i = 0; i < vegetablePos.Count; i++) {
                if (!vegetablePos[i].GetChild(0).gameObject.activeInHierarchy) {
                    vegetablePos[i].GetChild(0).gameObject.SetActive(true);
                    GameManager.instance.vegetablesP1++;
                    break;
                }
            }
            if (GameManager.instance.vegetablesP1 >= vegetablePos.Count) Debug.Log(transform.parent.tag + " Wins!");
        } else {
            for (int i = 0; i < vegetablePos.Count; i++) {
                if (!vegetablePos[i].GetChild(0).gameObject.activeInHierarchy) {
                    vegetablePos[i].GetChild(0).gameObject.SetActive(true);
                    GameManager.instance.vegetablesP2++;
                    break;
                }
            }
            if (GameManager.instance.vegetablesP2 >= vegetablePos.Count) Debug.Log(transform.parent.tag + " Wins!");
        }

    }
    #endregion

    void WaterBehaviour() {
        if (++waterCount >= waterToWin) Debug.Log(transform.parent.tag + " Wins!");
    }

    public void ObjectFalledOff() {
        objectGrabbed = false;
        switch (objectTag) {
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