using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabBehaviour : MonoBehaviour {
    bool filledHuerto, interfazRegar, interfazHuerto, interfazNabos;
    public string objectTag;
    public bool objectGrabbed;
    public bool water, vegetable, turnip;

    public GameObject vegetablePlaceHolder, mowerPlaceHolder;
    public int waterCount, waterToWin;
    public List<Transform> vegetablePos;
    public Transform huerto;
    public Transform playerInterfaceRegar;
    public Text playerInterfaceHuerto1;
    public Text playerInterfaceHuerto2;
    public Text playerInterfaceNabos;
    public Sprite raizCrecida;
    public string huertoName;

    PlayerController pC;

    private void Start() {
        pC = GetComponentInParent<PlayerController>();
        vegetablePlaceHolder = transform.GetChild(0).gameObject;
        mowerPlaceHolder = transform.GetChild(1).gameObject;
    }

    private void Update() {
        HuertoSetup();
        GetPlayerInterfaceRegar();
        GetPlayerInterfaceHuerto();
        GetPlayerInterfaceNabo();
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
                        playerInterfaceHuerto2.text = GameManager.instance.vegetablesP2.ToString() + "/12";
                    } else {
                        GameManager.instance.vegetablesP1--;
                        playerInterfaceHuerto1.text = GameManager.instance.vegetablesP1.ToString() + "/12";
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
                pC.waterThrowPSystem.Play();
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
        bool isPlayer1 = true;
        pC.anim.SetTrigger("GrabTurnip");
        pC.anim.SetBool("SmallObject", true);
        objectTag = grabbingObject.tag;
        grabbingObject.GetComponent<SphereCollider>().enabled = false;
        if (grabbingObject.GetComponent<TurnipType>().inBaseP1) {
            grabbingObject.GetComponent<TurnipType>().inBaseP1 = false;
            GameManager.instance.DeleteTurnips(isPlayer1, grabbingObject.GetComponent<TurnipType>().score);
        } else if (grabbingObject.GetComponent<TurnipType>().inBaseP2) {
            grabbingObject.GetComponent<TurnipType>().inBaseP2 = false;
            GameManager.instance.DeleteTurnips(!isPlayer1, grabbingObject.GetComponent<TurnipType>().score);
        }
        grabbingObject.position = transform.position;
        grabbingObject.SetParent(transform);
    }

    void TurnipBehaviour() {
        pC.anim.SetTrigger("ThrowObject");
        pC.anim.SetBool("SmallObject", false);
        objectGrabbed = false;
        transform.GetChild(2).position = transform.position;
        transform.GetChild(2).GetComponent<SphereCollider>().enabled = true;
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
        Vector3 pos = transform.position + transform.forward * 2;
        pos.y = -1.17f;
        grabbingObject.position = pos;
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
        transform.GetChild(2).forward = transform.forward;
        Vector3 pos = transform.position + transform.forward * 2;
        pos.y = -1.17f;
        transform.GetChild(2).position = pos;
        transform.GetChild(2).GetComponent<LawnMower>().StartCoroutine(transform.GetChild(2).GetComponent<LawnMower>().GetGrabZone());
        transform.GetChild(2).SetParent(GameObject.Find("Mowers").transform);
        mowerPlaceHolder.SetActive(false);
    }

    void GetPlayerInterfaceRegar() {
        if (GameManager.instance.currentMiniGame == MiniGames.Regar && !interfazRegar) {
            if (transform.parent.CompareTag("Player1")) {
                interfazRegar = true;
                playerInterfaceRegar = GameObject.Find("P1Interface").transform;
            } else {
                interfazRegar = true;
                playerInterfaceRegar = GameObject.Find("P2Interface").transform;
            }

        }
    }

    void GetPlayerInterfaceHuerto() {
        if (GameManager.instance.currentMiniGame == MiniGames.Huertos && !interfazHuerto) {
            interfazHuerto = true;
            playerInterfaceHuerto1 = GameObject.Find("P1Interface").GetComponentInChildren<Text>();
            playerInterfaceHuerto2 = GameObject.Find("P2Interface").GetComponentInChildren<Text>();

        }
    }

    void GetPlayerInterfaceNabo() {
        if (GameManager.instance.currentMiniGame == MiniGames.Nabos && !interfazNabos) {
            if (transform.parent.CompareTag("Player1")) {
                interfazNabos = true;
                playerInterfaceNabos = GameObject.Find("P1Interface").GetComponentInChildren<Text>();
            } else {
                interfazNabos = true;
                playerInterfaceNabos = GameObject.Find("P2Interface").GetComponentInChildren<Text>();
            }

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
        pC.anim.SetTrigger("GrabObject");
        pC.anim.SetBool("SmallObject", true);
        vegetablePlaceHolder.SetActive(true);
        switch (vegetable.parent.GetComponent<VegetableType>().vTypes) {
            case Types.Cebolla:
                vegetablePlaceHolder.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case Types.Rabano:
                vegetablePlaceHolder.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case Types.Zanahoria:
                vegetablePlaceHolder.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case Types.Patata:
                vegetablePlaceHolder.transform.GetChild(0).gameObject.SetActive(true);
                break;
            default:
                break;
        }
        vegetablePlaceHolder.GetComponent<VegetableType>().vTypes = vegetable.parent.GetComponent<VegetableType>().vTypes;
        if (!vegetable.parent.TryGetComponent(out PlayerVegetable pV)) {
            SoundManager.instance.aS.PlayOneShot(SoundManager.instance.excavar);
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
        SoundManager.instance.aS.PlayOneShot(SoundManager.instance.excavar);
        GameObject tempVegetable = vegetablePlaceHolder.transform.GetChild(0).gameObject;
        VegetableType vType = vegetablePlaceHolder.GetComponent<VegetableType>();
        pC.anim.SetTrigger("ThrowObject");
        pC.anim.SetBool("SmallObject", false);
        vegetable = false;
        objectGrabbed = false;
        for (int i = 0; i < vegetablePlaceHolder.transform.childCount; i++) {
            if (vegetablePlaceHolder.transform.GetChild(i).gameObject.activeInHierarchy) {
                tempVegetable = vegetablePlaceHolder.transform.GetChild(i).gameObject;
                vType = vegetablePlaceHolder.GetComponent<VegetableType>();
            }
            vegetablePlaceHolder.transform.GetChild(i).gameObject.SetActive(false);
        }
        vegetablePlaceHolder.SetActive(false);
        if (transform.parent.CompareTag("Player1")) {
            for (int i = 0; i < vegetablePos.Count; i++) {
                if (!vegetablePos[i].GetChild(0).gameObject.activeInHierarchy) {
                    vegetablePos[i].GetChild(0).gameObject.SetActive(true);
                    vegetablePos[i].GetChild(0).GetComponent<VegetableType>().vTypes = vType.vTypes;
                    vegetablePos[i].GetChild(0).GetChild(0).Find(tempVegetable.name).gameObject.SetActive(true);
                    //particulas verdura en huerto player1
                    GameManager.instance.vegetablesP1++;
                    playerInterfaceHuerto1.text = GameManager.instance.vegetablesP1.ToString() + "/12";
                    break;
                }
            }
            if (GameManager.instance.vegetablesP1 >= vegetablePos.Count) GameManager.instance.EndGame();
        } else {
            for (int i = 0; i < vegetablePos.Count; i++) {
                if (!vegetablePos[i].GetChild(0).gameObject.activeInHierarchy) {
                    vegetablePos[i].GetChild(0).gameObject.SetActive(true);
                    vegetablePos[i].GetChild(0).GetChild(0).Find(tempVegetable.name).gameObject.SetActive(true);
                    //particulas verdura en huerto player1
                    GameManager.instance.vegetablesP2++;
                    playerInterfaceHuerto2.text = GameManager.instance.vegetablesP2.ToString() + "/12";
                    break;
                }
            }
            if (GameManager.instance.vegetablesP2 >= vegetablePos.Count) GameManager.instance.EndGame();
        }
    }
    #endregion

    //IEnumerator SetObjectGrabbed() {
    //    yield return new WaitForSeconds(0.3f);
    //    objectGrabbed = false;
    //}

    void WaterBehaviour() {
        pC.anim.SetTrigger("ThrowWater");
        SoundManager.instance.aS.PlayOneShot(SoundManager.instance.salpicar);
        bool isPlayer1 = true;
        if (transform.parent.CompareTag("Player2")) {
            isPlayer1 = false;
        }

        if (isPlayer1) {
            playerInterfaceRegar.GetChild(GameManager.instance.waterP1).GetComponent<Image>().sprite = raizCrecida;
        } else {
            playerInterfaceRegar.GetChild(GameManager.instance.waterP2).GetComponent<Image>().sprite = raizCrecida;
        }

        GameManager.instance.CheckWater(isPlayer1);
    }

    public void ObjectFalledOff() {
        objectGrabbed = false;
        switch (objectTag) {
            case "Water":
                //particulas tirar agua
                if (water) {
                    //particulas tirar agua
                    pC.waterDropPSystem.Play();
                    SoundManager.instance.aS.PlayOneShot(SoundManager.instance.salpicar);
                    water = false;
                }
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