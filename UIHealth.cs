using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
	private AttackableEntity health;
	private Text text;

	void Start()
	{
		text = GetComponent<Text>();
	}

	public void SetAttackableEntity(AttackableEntity entity)
	{
		Debug.Log("Set attackable entity");
		health = entity;
	}

	void Update()
	{
		text.text = "Health: " + health.GetHealthInt();
	}
}
