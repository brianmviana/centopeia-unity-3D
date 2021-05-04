using System;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    GameObject pauseMenu;

    private PlayerController playerController;

    void SetFuilAmount(float _amount) {
        thrusterFuelFill.localScale = new Vector3(1, _amount, 1f);
    }

    public void SetPlayerController(PlayerController _playerController) {
        playerController = _playerController;
    }

    private void Start() {
        PauseMenu.IsOn = false;
    }

    private void Update() {
        SetFuilAmount(playerController.GetThrusterFuelAmount());
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
    }
}
