using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager: NetworkBehaviour
{
	private GameState state;
	private GameObject inGameHUD;
	private GameObject respawnHUD;
	private Rigidbody rb;

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

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		if (!inGameHUD)
			inGameHUD = Instantiate(Resources.Load<GameObject>("Prefabs/InGameHUD"));
		inGameHUD.GetComponentInChildren<UIHealth>().SetAttackableEntity(GetComponent<AttackableEntity>());
		if (!respawnHUD)
			respawnHUD = Instantiate(Resources.Load<GameObject>("Prefabs/AwaitingRespawnHUD"));
		respawnHUD.SetActive(false);
		Debug.Log(respawnHUD);
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
	public void TargetRpcEnterDeadAwaitingRespawn(NetworkConnection identity, int respawnTime, GameObject player)
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
		Text respawnText = respawnHUD.transform.Find("Countdown").gameObject.GetComponent<Text>();
		for (int i = respawnTime; i >= 0; i--)
		{
			respawnText.text = "Respawning in: " + i;
			yield return new WaitForSeconds(1);
		}
	}

	[TargetRpc]
	public void TargetRpcRespawn(NetworkConnection identity)
	{
		rb.position = Vector3.zero;
		respawnHUD.SetActive(false);
		inGameHUD.SetActive(true);
		state = GameState.IN_GAME;
	}

	public override void OnStartLocalPlayer()
	{
		State = GameState.IN_GAME;
		Debug.Log("Connected");
	}

	public enum GameState
    {
	    IN_GAME,
	    PAUSED,
	    DISCONNECTED,
	    DEAD_AWAITING_RESPAWN
    }
}
