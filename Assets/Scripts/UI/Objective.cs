using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
	public GameObject tick;

	public enum ObjectiveState
	{
		Inactive,
		Active,
		Completed
	}

	void Start ()
	{
		SetState(ObjectiveState.Inactive);
	}

	public void SetState(ObjectiveState state)
	{
		switch (state)
		{
			case ObjectiveState.Inactive:
				gameObject.SetActive(false);
				break;
			case ObjectiveState.Active:
				gameObject.SetActive(true);
				tick.SetActive(false);
				break;
			case ObjectiveState.Completed:
				gameObject.SetActive(true);
				tick.SetActive(true);
				break;
			default:
				break;
		}
	}
}
