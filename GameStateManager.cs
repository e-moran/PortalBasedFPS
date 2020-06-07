using Mirror;
using UnityEngine;

public class GameStateManager: NetworkBehaviour
{
	private GameState state;

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
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = false;
					state = value;
					break;
				case GameState.IN_GAME :
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

	public override void OnStartLocalPlayer()
	{
		State = GameState.IN_GAME;
		Debug.Log("Connected");
	}

	public enum GameState
    {
	    IN_GAME,
	    PAUSED,
	    DISCONNECTED
    }
}
