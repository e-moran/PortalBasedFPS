using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class AttackableEntity : NetworkBehaviour
{
	private float health; // Storing health as a float to allow for fractional health caused by percentage modifiers
	public float maxHealth = 100.0f;
	public bool respawns = false;
	public int respawnTimer = 1;

	public event Action<int> OnHealthChanged;

	private GameStateManager gsm;

	private void Start()
	{
		gsm = GetComponent<GameStateManager>();
		health = maxHealth;
	}

	public int GetHealthInt()
    {
	    return (int) Math.Ceiling(health); // Using Ceil to avoid a situation where a player is still alive with "0" health
    }

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		Debug.Log("Interesting event");
	}

	[Server]
    public void TakeDamage(float damage)
    {
	    health -= damage;
	    OnHealthChanged?.Invoke(GetHealthInt());
	    if (health <= 0)
		    Death();
    }

    [Server]
    public void Death()
    {
	    if (gameObject.CompareTag("Player") && respawns)
	    {
		    gsm.TargetRpcEnterDeadAwaitingRespawn(GetComponent<NetworkIdentity>().connectionToClient, respawnTimer, gameObject);
		    StartCoroutine(WaitForRespawn());
	    }
	    else
	    {
		    Destroy(gameObject); // Simply destroy any killed GO that's not a player
	    }
    }

    [Server]
    IEnumerator WaitForRespawn()
    {
	    Debug.Log("Waiting for respawn");
	    for (int i = respawnTimer; i >= 0; i--)
	    {
		    yield return new WaitForSeconds(1);
	    }

	    Debug.Log("Respawning");
	    health = maxHealth;
	    OnHealthChanged?.Invoke(GetHealthInt());
		gsm.TargetRpcRespawn(GetComponent<NetworkIdentity>().connectionToClient);
    }

}
