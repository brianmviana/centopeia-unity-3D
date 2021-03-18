using UnityEngine;

public class PlayerCrosshair : MonoBehaviour {

    public bool firstPerson;
    public GameObject mira;
    public GameObject crosshairPoint;
    public float shootForce;
    private Ray ray;
    RaycastHit raycastHit;

    void Update(){
        ray = new Ray(mira.transform.position, mira.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
            this.raycastHit = raycastHit;
            crosshairPoint.transform.position = raycastHit.point;
        }
        if (Input.GetMouseButtonDown(0)) {
            atira();
        }
    }

    private void atira() {
        if (this.raycastHit.rigidbody) {
            this.raycastHit.rigidbody.AddForceAtPosition(ray.direction * shootForce, this.raycastHit.point);
        }
    }
}
