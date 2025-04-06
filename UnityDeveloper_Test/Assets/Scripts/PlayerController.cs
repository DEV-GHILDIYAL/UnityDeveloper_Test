using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public Transform cam;
    public GameObject holo;
    public Transform groundCheck;
    public Transform neck;

    public float speed = 5f;
    public float jumpPower = 8f;
    public float groundDist = 0.2f;
    public LayerMask ground;

    public float gravity = 9.81f;
    public Vector3 gravityDir = Vector3.down;
    public Vector3 newGravityDir = Vector3.down;
    public bool pickingGravity = false;
    public bool onGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        holo.SetActive(false);
        rb.useGravity = false;

        if (!neck) Debug.LogError("Neck Gameobject noT found");
    }

    void Update()
    {
        if (transform.position.x >= 100 || transform.position.x <= -100 || transform.position.z >= 100 || transform.position.z <= -100 || transform.position.y >= 100 || transform.position.y <= -100)
    {
        Timer.instance.OutOfBound();
        return;
    }
        onGround = Physics.Raycast(groundCheck.position, gravityDir, groundDist, ground);
        anim.SetBool("OnGround", onGround);

        if (Input.GetKeyDown(KeyCode.Space) && onGround) DoJump();
        CheckGravityInput();
        if (Input.GetKeyDown(KeyCode.Return) && pickingGravity) SetNewGravity();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = Vector3.zero;

        if (cam)
        {
            Vector3 camFwd = cam.forward;
            Vector3 camRight = cam.right;
            Vector3 upDir = -gravityDir.normalized;
            camFwd = Vector3.ProjectOnPlane(camFwd, upDir).normalized;
            camRight = Vector3.ProjectOnPlane(camRight, upDir).normalized;
            move = (camFwd * v + camRight * h).normalized;
        }

        if (move.magnitude >= 0.1f)
        {
            Vector3 moveVel = move * speed;
            Vector3 gravVel = Vector3.Project(rb.linearVelocity, gravityDir);
            rb.linearVelocity = moveVel + gravVel;
            Quaternion rot = Quaternion.LookRotation(move, -gravityDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * 10f);
        }
        else
        {
            rb.linearVelocity = Vector3.Project(rb.linearVelocity, gravityDir);
        }

        anim.SetFloat("Speed", move.magnitude);
        rb.AddForce(gravityDir * gravity, ForceMode.Acceleration);
    }

    void DoJump()
    {
        rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, -gravityDir);
        rb.AddForce(-gravityDir * jumpPower, ForceMode.Impulse);
    }

    void CheckGravityInput()
    {
        Vector3 dir = newGravityDir;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            dir = GetCardinalDir(transform.forward);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            dir = GetCardinalDir(-transform.forward);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            dir = GetCardinalDir(transform.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dir = GetCardinalDir(-transform.right);
        }

        if (dir != newGravityDir)
        {
            newGravityDir = dir;
            pickingGravity = true;
            holo.SetActive(true);

            holo.transform.position = neck.position + newGravityDir * 0.5f;
            holo.transform.rotation = Quaternion.FromToRotation(Vector3.up, -newGravityDir);
        }

        if (pickingGravity && (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) ||
                               Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)))
        {
            pickingGravity = false;
            holo.SetActive(false);
        }
    }

    Vector3 GetCardinalDir(Vector3 dir)
    {
        Vector3 abs = new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z));
        Vector3 snap = Vector3.zero;

        if (abs.x > abs.y && abs.x > abs.z)
        {
            snap = dir.x > 0 ? Vector3.right : Vector3.left;
        }
        else if (abs.y > abs.x && abs.y > abs.z)
        {
            snap = dir.y > 0 ? Vector3.up : Vector3.down;
        }
        else if (abs.z > abs.x && abs.z > abs.y)
        {
            snap = dir.z > 0 ? Vector3.forward : Vector3.back;
        }
        return snap;
    }

    void SetNewGravity()
    {
        gravityDir = newGravityDir;
        pickingGravity = false;
        holo.SetActive(false);

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, -gravityDir);
        transform.rotation = rot;

        ThirdPersonCamera camScript = cam.GetComponentInParent<ThirdPersonCamera>();
        if (camScript) camScript.SetGravityDir(gravityDir);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundCheck.position, gravityDir * groundDist);
    }
}