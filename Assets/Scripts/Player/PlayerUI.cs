using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    private PlayerController playerController;

    void SetFuilAmount(float _amount) {
        thrusterFuelFill.localScale = new Vector3(1, _amount, 1f);
    }

    public void SetPlayerController(PlayerController _playerController) {
        playerController = _playerController;
    }

    private void Update() {
        SetFuilAmount(playerController.GetThrusterFuelAmount());
    }

}
