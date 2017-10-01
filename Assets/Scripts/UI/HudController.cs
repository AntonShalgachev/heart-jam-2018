using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
	public Objective[] objectives;
	public ObjectivePanel objectivePanel;
	public GameObject mouseKnobPrefab;
	public float maxKnobDistance;

	GameObject mouseDownKnob;
	GameObject mousePosKnob;
	
	Vector2 downPos;

	enum ObjectiveType
	{
		Treasure,
		Spawn
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			downPos = Input.mousePosition;
			mouseDownKnob = Instantiate(mouseKnobPrefab, downPos, Quaternion.identity);
			mousePosKnob = Instantiate(mouseKnobPrefab, Vector2.zero, Quaternion.identity);

			mouseDownKnob.transform.parent = transform;
			mousePosKnob.transform.parent = mouseDownKnob.transform;
		}

		if (mousePosKnob)
		{
			var dir = (Vector2)Input.mousePosition - downPos;
			dir = Vector2.ClampMagnitude(dir, maxKnobDistance);
			mousePosKnob.transform.localPosition = dir;
		}

		if (!Input.GetMouseButton(0))
		{
			if (mouseDownKnob)
				Destroy(mouseDownKnob);

			if (mousePosKnob)
				Destroy(mousePosKnob);
		}
	}

	public void InitObjectives()
	{
		SetObjectiveState(ObjectiveType.Treasure, Objective.ObjectiveState.Active);
		SetObjectiveState(ObjectiveType.Spawn, Objective.ObjectiveState.Inactive);

		objectivePanel.NotifyUser();
	}

	void SetObjectiveState(ObjectiveType objective, Objective.ObjectiveState state)
	{
		objectives[(int)objective].SetState(state);
	}

	public void OnTreasureCollected()
	{
		SetObjectiveState(ObjectiveType.Treasure, Objective.ObjectiveState.Completed);
		SetObjectiveState(ObjectiveType.Spawn, Objective.ObjectiveState.Active);

		objectivePanel.NotifyUser();
	}

	public void OnSpawnReached()
	{
		SetObjectiveState(ObjectiveType.Spawn, Objective.ObjectiveState.Completed);

		objectivePanel.NotifyUser();
	}
}
