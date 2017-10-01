using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
	public Objective[] objectives;

	enum ObjectiveType
	{
		Treasure,
		Spawn
	}

	private void Start()
	{
		SetObjectiveState(ObjectiveType.Treasure, Objective.ObjectiveState.Active);
		SetObjectiveState(ObjectiveType.Spawn, Objective.ObjectiveState.Inactive);
	}

	void SetObjectiveState(ObjectiveType objective, Objective.ObjectiveState state)
	{
		objectives[(int)objective].SetState(state);
	}

	public void OnTreasureCollected()
	{
		SetObjectiveState(ObjectiveType.Treasure, Objective.ObjectiveState.Completed);
		SetObjectiveState(ObjectiveType.Spawn, Objective.ObjectiveState.Active);
	}

	public void OnSpawnReached()
	{
		SetObjectiveState(ObjectiveType.Spawn, Objective.ObjectiveState.Completed);
	}
}
