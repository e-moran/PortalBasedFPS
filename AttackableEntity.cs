using System;
using System.Collections;
using Mirror;
using UnityEditor.Hardware;
using UnityEngine;

public class AttackableEntity : NetworkBehaviour
{
	public float maxHealth = 100.0f;
	public bool respawns = false;
	public int respawnTimer = 1;

	/*private float Health
	{
		get
		{
			return health;
		}
		set
		{
			health = value;
			OnHealthChanged?.Invoke(GetHealthInt());
			Debug.Log("Health changed");
		}
	} // Storing health as a float to allow for fractional health caused by percentage modifiers*/

	[SyncVar(hook = nameof(OnHealthChange))]
	private float health;

	public event Action<int> OnHealthChanged;

	private GameStateManager gsm;

	private void Start()
	{
		gsm = GetComponent<GameStateManager>();
	}

	public int GetHealthInt()
    {
	    return (int) Math.Ceiling(health); // Using Ceil to avoid a situation where a player is still alive with "0" health
    }

	public override void OnStartServer()
	{
		// Set the player's starting health once the game object is created on the server
		SetHealth(maxHealth);
	}

	[Client]
	public void OnHealthChange(float oldHealth, float newHealth)
	{
		OnHealthChanged?.Invoke((int) Math.Ceiling(newHealth));
	}

	// Change the health by the amount specified, a negative amount decreases the players health
	[Server]
	public void SetHealth(float health)
	{
		this.health = health;
	}

	[Command]
	public void CmdTakeDamage(float damage)
	{
		TakeDamage(damage);
	}

	[Server]
    public void TakeDamage(float damage)
    {
	    SetHealth(health - damage);
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
	    SetHealth(maxHealth);
	    OnHealthChanged?.Invoke(GetHealthInt());
		gsm.TargetRpcRespawn(GetComponent<NetworkIdentity>().connectionToClient);
    }

}
