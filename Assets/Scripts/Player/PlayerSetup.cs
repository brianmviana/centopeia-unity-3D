using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componetsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    void Start() {
        if (!isLocalPlayer) {
            DisableComponents();
            AssingRemoteLayer();
        }
        else {

            // Disable player graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            // Create Player UI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            // Configure Player UI
            PlayerUI playerUI = playerUIInstance.GetComponent<PlayerUI>();
            if (playerUI == null) {
                Debug.Log("No PlayerUI componet on PlayerUI prefab.");
            }
            playerUI.SetPlayerController(GetComponent<PlayerController>());

            GetComponent<Player>().SetupPlayer();
        }

    }

    private void SetLayerRecursively(GameObject obj, int newLayer) {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
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

        Destroy(playerUIInstance);
        if (isLocalPlayer) {
            GameManager.instance.SetSceneCameraActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);

    }

}
