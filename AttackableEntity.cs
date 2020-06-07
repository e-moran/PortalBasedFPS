using System;
using Mirror;

public class AttackableEntity : NetworkBehaviour
{
	public float health = 100.0f; // Storing health as an int to allow for fractional health caused by percentage modifiers

	public int GetHealthInt()
    {
	    return (int) Math.Ceiling(health); // Using Ceil to avoid a situation where a player is still alive with "0" health
    }

    public void TakeDamage(float damage)
    {
	    health -= damage;
    }
}
