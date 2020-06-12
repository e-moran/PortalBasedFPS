using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class UIHealth : MonoBehaviour
{
	private Text text;
	private AttackableEntity ae;
	private LocalMessenger _messenger;

	void Start()
	{
		Debug.Log("Running UIHealth start");
		text = GetComponent<Text>();
		_messenger = GameObject.Find("LocalMessenger").GetComponent<LocalMessenger>();
		_messenger.OnPlayerConnected += OnPlayerConnected;
	}

	private void OnPlayerConnected(GameObject player)
	{
		Debug.Log("OnPlayerConnected in UI HEALTH");
		ae = player.GetComponent<AttackableEntity>();
		ae.OnHealthChanged += UpdateHealth;
		UpdateHealth(ae.GetHealthInt());
	}

	private void OnPlayerDisconnected(GameObject player)
	{
		ae.OnHealthChanged -= UpdateHealth;
	}

	/* private void OnEnable()
	{
		text = GetComponent<Text>();
		ae = GameObject.Find("Local").GetComponent<AttackableEntity>();

		ae.OnHealthChanged += UpdateHealth;
		UpdateHealth(ae.GetHealthInt());
	}

	private void OnDisable()
	{
		ae.OnHealthChanged -= UpdateHealth;
	} */

	private void UpdateHealth(int health)
	{
		Debug.Log("Updating health " + health);
		text.text = "Health: " + health;
	}
}
