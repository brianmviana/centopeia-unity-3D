using System.Collections;
using UnityEngine;
using UnityEngine.Networking ;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

    private bool _isDead = false;
    public bool isDead {
        get {
            return _isDead;
        }

        protected set {
            _isDead = value;
        }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnebled;

    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;

    [SerializeField]
    private GameObject spawnEffect;

    [SerializeField]
    private GameObject deathEffect;

    private bool isfirstSetup = true;


    public void SetupPlayer() {

        if (isLocalPlayer) {
            // Switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }
    
    [Command]
    private void CmdBroadCastNewPlayerSetup() {
        RpcPlayerSetupOnAllClients();
    }

    [ClientRpc]
    private void RpcPlayerSetupOnAllClients() {
        if (isfirstSetup) {
            wasEnebled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnebled.Length; i++) {
                wasEnebled[i] = disableOnDeath[i].enabled;
            }
            isfirstSetup = false;
        }
        SetDefaults();
    }

    private void Update() {
        if (!isLocalPlayer) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            RpcTakeDamage(9999);
        }
    }

    private void SetDefaults() {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnebled[i];
        }
        
        for (int i = 0; i < disableGameObjectOnDeath.Length; i++) {
            disableGameObjectOnDeath[i].SetActive(true);
        }

        Collider _collider = GetComponent<Collider>();
        if (_collider != null) {
            _collider.enabled = true;
        }

        // Create Spawn effect
        GameObject _spawnGraphics = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_spawnGraphics, 3f);
    }

    [ClientRpc]
    internal void RpcTakeDamage(int damage) {

        if (isDead) {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0) {
            Die();
        }

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

    private void Die() {
        isDead = true;

        // Disable Components
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }

        // Disable GameObjes
        for (int i = 0; i < disableGameObjectOnDeath.Length; i++) {
            disableGameObjectOnDeath[i].SetActive(false);
        }

        // Disable Collider
        Collider _collider = GetComponent<Collider>();
        if (_collider != null) {
            _collider.enabled = false;
        }

        Debug.Log(transform.name + " is Dead!");

        // Spawn a death effect
        GameObject _explosionGraphics =  Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_explosionGraphics, 3f);

        // RESPAWN METHOD
        StartCoroutine(Respawn());

    }

    IEnumerator Respawn() {

        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.2f);

        SetupPlayer();

        Debug.Log(transform.name + " respawned!");
    }

}
