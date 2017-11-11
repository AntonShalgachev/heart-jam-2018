using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
	public Objective[] objectives;
	public ObjectivePanel objectivePanel;

	enum ObjectiveType
	{
		Treasure,
		Spawn
	}

	private void Update()
	{

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
