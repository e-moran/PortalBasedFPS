﻿using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
	public int damage = 10;
	public float secondsPerShot = 0.2f;

	private bool shootPressed
	{
		get
		{
			return Input.GetButtonDown("Shoot") || (int) Math.Floor(Input.GetAxis("Shoot")) != 0;
		}
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		if (hasAuthority)
			StartCoroutine(Shoot());
	}

	IEnumerator Shoot()
	{
		while (true)
		{
			if (Input.GetButton("Shoot"))
			{
				CmdShoot();
				yield return new WaitForSeconds(secondsPerShot);
			}

			if (Input.GetKeyDown("p")) // Debug key to kill player
			{
				Debug.Log("Sending debug take damage command");
				GetComponent<AttackableEntity>().CmdTakeDamage(75);
			}

			yield return null;
		}
	}

	[Command]
	void CmdShoot()
	{
		RaycastHit hit;
		Transform cameraTransform = Camera.main.transform;
		if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit))
		{
			Debug.Log(hit);
			AttackableEntity e;
			if ((e = hit.collider.gameObject.GetComponent<AttackableEntity>()) != null) // If the target was an attackable entity
			{
				e.TakeDamage(damage);
			}
		}
	}
}
