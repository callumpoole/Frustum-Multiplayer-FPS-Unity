using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
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
    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    private bool firstSetup = true;

    // Use this for initialization
    public void SetupPlayer() {
        if (isLocalPlayer) {
            //Switch Camera
            GameManager.inst.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().GUIInst.SetActive(true);
        } 
        CmdBroadcastNewPlayerSetup();

    }

    [Command]
    private void CmdBroadcastNewPlayerSetup() {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients() {
        if (firstSetup) {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++) {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }

    void Update() {
        if (!isLocalPlayer)
            return;
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
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++) {
            disableGameObjectsOnDeath[i].SetActive(false);
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        //Switch Camera
        if (isLocalPlayer) {
            GameManager.inst.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().GUIInst.SetActive(false);
        }

        Debug.Log(transform.name + " is Dead");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.inst.matchSettings.respawnTime);

        Transform spawnPos = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        yield return new WaitForSeconds(0.1f); //Only needed if respawn particles

        SetupPlayer();

        Debug.Log(transform.name + " Respawned");
    }

    public void SetDefaults() {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++) {
            disableGameObjectsOnDeath[i].SetActive(true);
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

    }
}
