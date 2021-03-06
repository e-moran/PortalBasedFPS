﻿using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager: NetworkBehaviour
{
	private GameState state;
	private GameObject inGameHUD;
	private GameObject respawnHUD;
	private Text _respawnText;
	private Rigidbody rb;
	private LocalMessenger _messenger;

	public GameState State
	{
		get
		{
			return state;
		}
		set
		{
			switch (value)
			{
				case GameState.PAUSED :
					// TODO Implement actual pause screen
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					state = value;
					break;
				case GameState.IN_GAME :
					// TODO Implement starting HUD
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
					state = value;
					break;
				default:
					state = value;
					break;
			}

			Debug.Log(state);
		}
	}

	void Awake()
	{

		State = GameState.DISCONNECTED;
	}

	void Update()
	{
		if (Input.GetButtonDown("Escape") && state == GameState.IN_GAME)
		{
			State = GameState.PAUSED;
		} else if (Input.GetButtonDown("Escape") && state == GameState.PAUSED)
		{
			State = GameState.IN_GAME;
		}
	}

	[TargetRpc]
	public void TargetRpcEnterDeadAwaitingRespawn(int respawnTime)
	{
		state = GameState.DEAD_AWAITING_RESPAWN;
		inGameHUD.SetActive(false);
		respawnHUD.SetActive(true);
		rb.position += Vector3.right * 1000;
		StartCoroutine(RespawnTimer(respawnTime));
	}

	[Client]
	IEnumerator RespawnTimer(int respawnTime)
	{
		for (int i = respawnTime; i >= 0; i--)
		{
			_respawnText.text = "Respawning in: " + i;
			yield return new WaitForSeconds(1);
		}
	}

	[TargetRpc]
	public void TargetRpcRespawn()
	{
		rb.position = Vector3.zero;
		respawnHUD.SetActive(false);
		inGameHUD.SetActive(true);
		state = GameState.IN_GAME;
	}

	public override void OnStartLocalPlayer()
	{
		Debug.Log(new System.Diagnostics.StackTrace().ToString());
		State = GameState.IN_GAME;
		gameObject.name = "Local";
		Debug.Log("Connected");

		rb = GetComponent<Rigidbody>();
		_messenger = GameObject.Find("LocalMessenger").GetComponent<LocalMessenger>();

		inGameHUD = GameObject.Find("InGameHUD");
		respawnHUD = GameObject.Find("AwaitingRespawnHUD");
		respawnHUD.SetActive(false);
		_respawnText = respawnHUD.transform.Find("Countdown").gameObject.GetComponent<Text>();
		_messenger.InvokePlayerConnected(gameObject);
	}

	public override void OnStopClient()
	{
		_messenger.InvokePlayerDisconnected(gameObject);
	}

	public enum GameState
    {
	    IN_GAME,
	    PAUSED,
	    DISCONNECTED,
	    DEAD_AWAITING_RESPAWN
    }
}
