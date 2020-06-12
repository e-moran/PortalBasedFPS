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
		text = GetComponent<Text>();
		_messenger = GameObject.Find("LocalMessenger").GetComponent<LocalMessenger>();
		_messenger.OnPlayerConnected += OnPlayerConnected;
	}

	private void OnPlayerConnected(GameObject player)
	{
		ae = player.GetComponent<AttackableEntity>();
		ae.OnHealthChanged += UpdateHealth;
		UpdateHealth(ae.GetHealthInt());
	}

	private void OnPlayerDisconnected(GameObject player)
	{
		ae.OnHealthChanged -= UpdateHealth;
	}

	private void UpdateHealth(int health)
	{
		text.text = "Health: " + health;
	}
}
