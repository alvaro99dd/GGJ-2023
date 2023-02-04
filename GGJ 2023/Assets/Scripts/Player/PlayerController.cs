using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] float speed;
    [SerializeField] float turnSmoothVelocity, turnSmoothTime;

    Vector3 direction;
    Rigidbody rb;
    Coroutine grabbing, earthquakeCoroutine;
    public GrabBehaviour gB;
    public bool dropZone, grabZone, stun;
    public EarthQuake earthquake;
    public BoxCollider bC;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
        gB = GetComponentInChildren<GrabBehaviour>();
    }

    private void FixedUpdate() {
        Movement();
        if (earthquake != null) {
            CheckEarthquake();
        }
        CheckPrompt();
        GetEarthQuake();
    }

    void GetEarthQuake() {
        if (GameManager.instance.currentMiniGame == MiniGames.Regar && earthquake == null) {
            earthquake = GameObject.Find("MiniGameEvents").GetComponent<EarthQuake>();
        }
    }

    void Movement() {
        if (stun) {
            return;
        }
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        rb.velocity = direction * speed * Time.fixedDeltaTime;
        if (!rb.velocity.Equals(Vector3.zero)) {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => direction = ctx.ReadValue<Vector3>();

    public void OnGrab(InputAction.CallbackContext ctx) {
        if (grabZone && !gB.objectGrabbed) {
            grabbing = StartCoroutine(EnableGrab());
        } else if (dropZone && gB.objectGrabbed && grabbing == null) {
            gB.DropObject();
        }
    }

    IEnumerator EnableGrab() {
        bC.enabled = true;
        yield return new WaitForSeconds(0.2f);
        bC.enabled = false;
        grabbing = null;
    }

    void CheckEarthquake() {
        if (!rb.velocity.Equals(Vector3.zero) && gB.objectGrabbed && earthquake.earthquake && earthquakeCoroutine == null) {
            gB.ObjectFalledOff();
            earthquakeCoroutine = StartCoroutine(StunBehaviour());
        }
    }

    IEnumerator StunBehaviour() {
        stun = true;
        //particulas stun aparecen
        yield return new WaitForSeconds(earthquake.stunDuration);
        stun = false;
        //particulas stun fuera
        earthquakeCoroutine = null;
    }

    public IEnumerator StunPlayer(float stunDuration) {
        stun = true;
        //particulas stun aparecen
        yield return new WaitForSeconds(stunDuration);
        //particulas stun fuera
        stun = false;
    }

    void CheckPrompt() {
        if (dropZone && gB.objectGrabbed) {
            Debug.Log("Mostrando prompt");
        } else if (grabZone && !gB.objectGrabbed) {
            Debug.Log("Mostrando prompt");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("DropZone")) {
            gB.huertoName = other.name;
            if (GameManager.instance.currentMiniGame == MiniGames.Huertos) {
                if (gB.huertoName != gB.huerto.parent.name) {
                    dropZone = false;
                } else {
                    dropZone = true;
                }
            } else {
                dropZone = true;
            }
        } else if (other.CompareTag("Water") || other.CompareTag("Vegetable") || other.CompareTag("Mower") || other.CompareTag("Turnip")) {
            grabZone = true;
        }

        if (other.CompareTag("Play")) {
            if (++GameManager.instance.playersInButton == 2) {
                CanvasManager.instance.playText.SetActive(false);
                GameManager.instance.StartCoroutine(GameManager.instance.LobbyCountDown());
            } else {
                CanvasManager.instance.playText.SetActive(true);
            }
        }

        if (other.CompareTag("Controls")) {
            if (++GameManager.instance.playersInButton == 2) {

            }
        }

        if (other.CompareTag("Exit")) {
            if (++GameManager.instance.playersInButton == 2) {
                Application.Quit();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("DropZone")) {
            dropZone = false;
        } else {
            grabZone = false;
        }

        if (other.CompareTag("Play")) {
            GameManager.instance.playersInButton--;
            CanvasManager.instance.playText.SetActive(false);
        }
    }
}
