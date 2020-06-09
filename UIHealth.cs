using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
	private Text text;

	void Start()
	{
		text = GetComponent<Text>();
	}

	private void OnEnable()
	{
		GameObject.Find("Local").GetComponent<AttackableEntity>().OnHealthChanged += UpdateHealth;
	}

	private void UpdateHealth(int health)
	{
		text.text = "Health: " + health;
	}
}
