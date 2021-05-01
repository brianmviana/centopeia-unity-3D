using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componetsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    Camera sceneCamera;

    void Start() {
        if (!isLocalPlayer) {
            DisableComponents();
            AssingRemoteLayer();
        }
        else {
            sceneCamera = Camera.main;
            if (sceneCamera != null) {
                sceneCamera.gameObject.SetActive(false);
            }
        }

        RegisterPlayer();

    }

    void RegisterPlayer() {
        string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;

    }

    void AssingRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents() {
        for (int i = 0; i < componetsToDisable.Length; i++) {
            componetsToDisable[i].enabled = false;
        }
    }


    void OnDisable() {
        if (sceneCamera != null) {
            sceneCamera.gameObject.SetActive(true);
        }
    }

}
