using UnityEngine;
using UnityEngine.Networking ;


[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot: NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    private Weapon currentWeapon;
    private WeaponManager weaponManager;

    [SerializeField]
    private AudioSource _audioBox;

    [SerializeField]
    private LayerMask mask;

    private void Start() {
        if (cam == null) {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update() {

        if (PauseMenu.IsOn) {
            return;
        }

        currentWeapon = weaponManager.GetCurrentWeapon();
        
               
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
        else if (Input.GetButtonDown("Fire2")) {
            currentWeapon.fireRate = 5f;
            InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
        }
        else if (Input.GetButtonUp("Fire2")) {
            CancelInvoke("Shoot");
            currentWeapon.fireRate = 0f;
        }
        

        /*        if (currentWeapon.fireRate <= 0f) {
                    if (Input.GetButtonDown("Fire1")) {
                        Shoot();
                    }
                }
                else {
                    if (Input.GetButtonDown("Fire1")) {
                        InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                    }
                    else if (Input.GetButtonUp("Fire1")) {
                        CancelInvoke("Shoot");
                    }
                }*/
    }

    // Is called on the server when a player shoots
    [Command]
    void CmdOnShoot() {
        RpcDoShootEffect();
    }

    // Is called on all clients when we eed to a shoot effect
    [ClientRpc]
    void RpcDoShootEffect() {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
        _audioBox.pitch = Random.Range(0.5f, 1.1f);
        _audioBox.Play();
    }

    // Is called on the server when we hit something
    // Takes in the hit point and the normal of the surface
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal) {
        RpcDoHitEffect(_pos, _normal);
    }

    // Is called on all clients
    // Here we can spawn in cool effects
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal) {
        GameObject _hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2f);
    }


    [Client]
    void Shoot() {

        if (!isLocalPlayer) {
            return;
        }
        

        // We are the shoot method on the server
        CmdOnShoot();

        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask)) {
            if (_hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }

            // We hit something, call the OnHit method on the server
            CmdOnHit(_hit.point, _hit.normal);
        }

    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage) {
        Debug.Log(_playerID + " has been shot");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);

//        Destroy(GameObject.Find(_ID));
    }

}
