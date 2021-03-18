using UnityEngine;

public class PlayerMoviment : MonoBehaviour {

    public float MoveSpeed;
    public float TurnSpeed;
    public float Velocity;
    public float JumpForce = 300f;
    private bool _jumping = false;

    public Rigidbody rigidbody;
    public GameObject piso;

    public Transform cam;

    /*    public CharacterController controller;
        public Transform cam;
        public float speed = 6f;
        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;
    */
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    void Start() {
        MoveSpeed = 10f;
        TurnSpeed = 90f;

    }

    /*    void Update() {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f) {
                float targeAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targeAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targeAngle, 0f) * Vector3.forward;
                transform.Translate(direction * speed * Time.deltaTime);
            }
        }*/



    void Update() {
        if (Input.GetKey(KeyCode.W)) {
            walking(1f, 0f);
        }
        if (Input.GetKey(KeyCode.S)) {
            walking(-1f, 0f);
        }
        if (Input.GetKey(KeyCode.A)) {
            walking(0f, -1f);
        }
        if (Input.GetKey(KeyCode.D)) {
            walking(0f, 1f);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            rotate(-TurnSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            rotate(TurnSpeed);
        }
    }

    private void walking(float direction_forward, float direction_right) {
        float x = (transform.right.x * MoveSpeed * Time.deltaTime * direction_right);
        float y = 0f;
        float z = (transform.forward.z * MoveSpeed * Time.deltaTime * direction_forward);
        Vector3 direction = new Vector3(x, y, z);
        rigidbody.MovePosition(rigidbody.position + direction);
    }

    private void rotate(float TurnSpeed) {
        transform.Rotate(0, TurnSpeed * Time.deltaTime * 1f, 0);

    }

    /*    private void Jump() {
            rigidbody.AddForce(0, JumpForce, 0);
        }

        private void OnCollisionStay(Collision collision) {
            if (collision.gameObject.CompareTag(GameTags.Plaform) && Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }*/
    }
