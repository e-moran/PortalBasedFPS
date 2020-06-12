using System;
using UnityEngine;

public class LocalMessenger : MonoBehaviour
{
	// Fires when the player connects, takes the player's GameObject as a param
	public event Action<GameObject> OnPlayerConnected;

	// Fires when the player disconnects, takes the player's GameObject as a param
	public event Action<GameObject> OnPlayerDisconnected;

	// Fires the OnPlayerConnected action, takes the player as a parameter
	public void InvokePlayerConnected(GameObject player)
	{
		Debug.Log("Player connected");
		OnPlayerConnected?.Invoke(player);
	}

	// Fires the OnPlayerConnected action, takes the player as a parameter
	public void InvokePlayerDisconnected(GameObject player)
	{
		OnPlayerDisconnected?.Invoke(player);
	}
}
