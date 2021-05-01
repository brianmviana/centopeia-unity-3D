using System.Collections;
using UnityEngine;
using UnityEngine.Networking ;

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


    public void Setup() {
        wasEnebled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnebled.Length; i++) {
            wasEnebled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    /*private void Update() {
        if (!isLocalPlayer) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            RpcTakeDamage(9999);
        }
    }*/

    private void SetDefaults() {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnebled[i];
        }

        Collider _collider = GetComponent<Collider>();
        if (_collider != null) {
            _collider.enabled = true;
        }

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

        // DISABLE COMPONENSTS
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }

        Collider _collider = GetComponent<Collider>();
        if (_collider != null) {
            _collider.enabled = false;
        }

        Debug.Log(transform.name + " is Dead!");

        // RESPAWN METHOD
        StartCoroutine(Respawn());

    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " respawned!");
    }

}
