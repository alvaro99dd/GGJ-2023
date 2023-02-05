using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour {
    bool filledHuerto;
    public string objectTag;
    public bool objectGrabbed;
    public bool water, vegetable, turnip;

    public GameObject vegetablePlaceHolder, mowerPlaceHolder;
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
                pC.anim.SetTrigger("GrabWater");
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
        pC.anim.SetTrigger("GrabTurnip");
        pC.anim.SetBool("SmallObject", true);
        objectTag = grabbingObject.tag;
        pC.grabZone = false;
        grabbingObject.position = transform.position;
        grabbingObject.SetParent(transform);
    }

    void TurnipBehaviour() {
        pC.anim.SetTrigger("ThrowObject");
        pC.anim.SetBool("SmallObject", false);
        objectGrabbed = false;
        transform.GetChild(2).position = transform.position;
        transform.GetChild(2).GetComponent<TurnipType>().StartCoroutine(transform.GetChild(2).GetComponent<TurnipType>().GetGrabZone());
        transform.GetChild(2).SetParent(GameObject.Find("Turnips").transform);
    }

    void GrabMower(Transform grabbingObject) {
        //particulas podadoras
        pC.anim.SetTrigger("GrabPodadora");
        pC.anim.SetBool("Podadora", true);
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
        pC.anim.SetTrigger("ThrowPodadora");
        pC.anim.SetBool("Podadora", false);
        objectGrabbed = false;
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).GetComponent<LawnMower>().dir = transform.forward;
        transform.GetChild(2).GetComponent<LawnMower>().StartCoroutine(transform.GetChild(2).GetComponent<LawnMower>().GetGrabZone());
        transform.GetChild(2).SetParent(GameObject.Find("Mowers").transform);
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
        pC.anim.SetTrigger("GrabObject");
        pC.anim.SetBool("SmallObject", true);
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
        pC.anim.SetTrigger("ThrowObject");
        pC.anim.SetBool("SmallObject", false);
        vegetable = false;
        objectGrabbed = false;
        vegetablePlaceHolder.SetActive(false);
        if (transform.parent.CompareTag("Player1")) {
            for (int i = 0; i < vegetablePos.Count; i++) {
                if (!vegetablePos[i].GetChild(0).gameObject.activeInHierarchy) {
                    vegetablePos[i].GetChild(0).gameObject.SetActive(true);
                    //particulas verdura en huerto player1
                    GameManager.instance.vegetablesP1++;
                    break;
                }
            }
            if (GameManager.instance.vegetablesP1 >= vegetablePos.Count) GameManager.instance.EndGame();
        } else {
            for (int i = 0; i < vegetablePos.Count; i++) {
                if (!vegetablePos[i].GetChild(0).gameObject.activeInHierarchy) {
                    vegetablePos[i].GetChild(0).gameObject.SetActive(true);
                    //particulas verdura en huerto player1
                    GameManager.instance.vegetablesP2++;
                    break;
                }
            }
            if (GameManager.instance.vegetablesP2 >= vegetablePos.Count) GameManager.instance.EndGame();
        }
    }
    #endregion

    //IEnumerator SetObjectGrabbed() {
    //    yield return new WaitForSeconds(0.5f);
    //    objectGrabbed = false;
    //}

    void WaterBehaviour() {
        pC.anim.SetTrigger("ThrowWater");
        bool isPlayer1 = true;
        if (transform.parent.CompareTag("Player2")) {
            isPlayer1 = false;
        }
        GameManager.instance.CheckWater(isPlayer1);
    }

    public void ObjectFalledOff() {
        objectGrabbed = false;
        switch (objectTag) {
            case "Water":
                //particulas tirar agua
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