using System;
using Mirror;
using UnityEngine;

public class AttackableEntity : NetworkBehaviour
{
	public float health = 100.0f; // Storing health as an int to allow for fractional health caused by percentage modifiers

	public int GetHealthInt()
    {
	    return (int) Math.Ceiling(health); // Using Ceil to avoid a situation where a player is still alive with "0" health
    }

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		Debug.Log("Interesting event");
		if (hasAuthority)
		{
			GameObject.Find("Health").GetComponent<UIHealth>().SetAttackableEntity(this);
		}
	}

	[Server]
    public void TakeDamage(float damage)
    {
	    health -= damage;
	    if (health <= 0)
		    Destroy(gameObject);
    }

}
