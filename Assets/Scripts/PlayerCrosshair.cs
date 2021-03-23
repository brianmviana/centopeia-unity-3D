using UnityEngine;

public class PlayerCrosshair : MonoBehaviour {

    public LineRenderer mira;
    private Ray ray;
    RaycastHit raycastHit;

    void Update(){
        ray = new Ray(mira.transform.position, mira.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
            this.raycastHit = raycastHit;
            //mira.Se
            //crosshairPoint.transform.position = raycastHit.point;
        }
        if (Input.GetMouseButtonDown(0)) {
       //     atira();
        }
    }

   /* private void atira() {
        if (this.raycastHit.rigidbody) {
            this.raycastHit.rigidbody.AddForceAtPosition(ray.direction * shootForce, this.raycastHit.point);
        }
    }*/
}
