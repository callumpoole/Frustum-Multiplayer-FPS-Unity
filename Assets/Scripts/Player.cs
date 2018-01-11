using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    [SyncVar]
    private bool _isDead = false;
    public bool isDead { get { return _isDead; } protected set { _isDead = value; } }


    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;
    public int GetCurrentHealth() { return currentHealth; }

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    // Use this for initialization
    public void Setup() {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++) {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    void Update() {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.K))
            RpcTakeDamage(789);
    }
    public void SetDefaults() {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }
    [ClientRpc]
    public void RpcTakeDamage(int damage) {
        if (isDead)
            return;
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    private void Die() {
        isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;


        Debug.Log(transform.name + " is Dead");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.inst.matchSettings.respawnTime);

        SetDefaults();
        Transform spawnPos = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;
        Debug.Log(transform.name + " Respawned");
    }
}
