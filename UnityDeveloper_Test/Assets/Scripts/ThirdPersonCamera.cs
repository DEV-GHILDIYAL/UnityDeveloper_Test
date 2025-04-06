using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Transform cam;
    public float dist = 5f;
    public float ht = 2f;
    public float mouseSpeed = 3f;
    public float smooth = 10f;
    public float minAngle = -30f;
    public float maxAngle = 60f;

    public float yaw = 0f;
    public float pitch = 0f;
    public Vector3 upDir = Vector3.up;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yaw = transform.eulerAngles.y;
        pitch = 15f;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSpeed;
        pitch -= Input.GetAxis("Mouse Y") * mouseSpeed;
        pitch = Mathf.Clamp(pitch, minAngle, maxAngle);

        Quaternion gravRot = Quaternion.FromToRotation(Vector3.up, upDir);
        Quaternion rot = gravRot * Quaternion.Euler(pitch, yaw, 0f);

        Vector3 offset = new Vector3(0f, ht, -dist);
        Vector3 newPos = target.position + rot * offset;
        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
        transform.rotation = rot;

        cam.position = transform.position;
        cam.LookAt(target, upDir);
    }

    public void SetGravityDir(Vector3 newGrav)
    {
        upDir = -newGrav;
    }
}