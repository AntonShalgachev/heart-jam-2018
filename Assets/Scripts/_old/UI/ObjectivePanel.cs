using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePanel : MonoBehaviour
{
	Animation anim;

	private void Awake()
	{
		anim = GetComponent<Animation>();
	}

	public void NotifyUser()
	{
		anim.Play();
	}
}
