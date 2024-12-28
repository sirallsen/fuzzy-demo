using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] Animator sword;

    private CharacterController player;
    private Camera playerCamera;

    private float moveFB;
    private float moveLR;

    private float rotX;
    private float rotY;

    void Start()
    {
        player = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity;

        rotY = Mathf.Clamp(rotY, -60f, 60f);

        Vector3 movement = new Vector3(moveLR, 0, moveFB);
        transform.Rotate(0, rotX, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(rotY, 0, 0);

        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);

        player.SimpleMove(Physics.gravity);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            sword.enabled = true;
            sword.SetTrigger("Attack");
            StartCoroutine(AwaitThenAttack());
        }
    }

    IEnumerator AwaitThenAttack()
    {
        yield return new WaitForSeconds(1.0f);
        sword.enabled = false;
    }
}
