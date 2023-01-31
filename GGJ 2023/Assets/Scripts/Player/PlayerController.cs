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
        rb = GetComponent<Rigidbody>();
        gB = GetComponentInChildren<GrabBehaviour>();
        earthquake = GameObject.Find("MiniGameEvents").GetComponent<EarthQuake>();
    }

    private void FixedUpdate() {
        Movement();
        if (earthquake != null) {
            CheckEarthquake();
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
        } else if (dropZone && gB.objectGrabbed) {
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
        yield return new WaitForSeconds(earthquake.stunDuration);
        stun = false;
        earthquakeCoroutine = null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("DropZone")) {
            dropZone = true;
        } else if (other.CompareTag("Water") || other.CompareTag("Vegetable") || other.CompareTag("LawnMower")) {
            grabZone = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        dropZone = false;
        grabZone = false;
    }
}
