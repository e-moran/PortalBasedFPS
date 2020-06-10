using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
	private Text text;
	private AttackableEntity ae;

	private void OnEnable()
	{
		text = GetComponent<Text>();
		ae = GameObject.Find("Local").GetComponent<AttackableEntity>();

		ae.OnHealthChanged += UpdateHealth;
		UpdateHealth(ae.GetHealthInt());
	}

	private void OnDisable()
	{
		ae.OnHealthChanged -= UpdateHealth;
	}

	private void UpdateHealth(int health)
	{
		Debug.Log("Updating health");
		text.text = "Health: " + health;
	}
}
