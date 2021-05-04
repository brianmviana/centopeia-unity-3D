using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private Weapon primaryWeapon;

    private Weapon currentWeapon;
    private WeaponGraphics currentGraphics;
    private AudioClip currentFireSound;


    private void Start() {
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(Weapon _weapon) {
        currentWeapon = _weapon;
        GameObject _weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null) {
            Debug.Log("No weapon graphics component on the weapon object: " + _weaponIns.name);
        }

        if (isLocalPlayer) {
            Utils.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
            //_weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }

    public Weapon GetCurrentWeapon() {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics() {
        return currentGraphics;
    }

    public AudioClip GetCurrentFireSound() {
        return currentFireSound;
    }
}
