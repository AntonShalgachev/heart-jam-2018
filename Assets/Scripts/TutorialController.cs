using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;

    public float initialDelay;
    public float delayBeforeCancel;
    public float delayBeforePause1;
    public float delayBeforePause2;

    public event Action OnStepCompleted;

    public enum Step
    {
        InitialDelay,

        MineMeteor,
        DelayBeforeCancel,
        CancelMining,
        RemoveMineMeteor,

        SpawnEnemies1,
        DelayBeforePause1,
        PauseEnemies1,
        DestroyEnemiesWithFinger,
        SpawnEnemies2,
        DelayBeforePause2,
        PauseEnemies2,
        DestroyEnemiesWithSatellite,

        Completed,
    }

    private Step step = Step.InitialDelay;

    public MineMeteorMovement TutorialMineMeteor { get; set; }

    public List<EnemyMeteorMovement> TutorialEnemyMeteors { get; private set; }

    public void AddEnemyMeteor(EnemyMeteorMovement meteor)
    {
        TutorialEnemyMeteors.Add(meteor);
    }
    public bool OnEnemyMeteorDestroyed(EnemyMeteorMovement meteor)
    {
        TutorialEnemyMeteors.Remove(meteor);

        return TutorialEnemyMeteors.Count == 0;
    }

    private void Awake()
    {
        Debug.Assert(!Instance, "There is another tutorial controller");
        Instance = this;

        TutorialEnemyMeteors = new List<EnemyMeteorMovement>();
    }

    private bool isEnabled = false;

    public void SetTutorialEnabled(bool enabled)
    {
        isEnabled = enabled;
    }

    public bool IsEnabled()
    {
        return isEnabled;
    }

    public Step GetStep()
    {
        return step;
    }

    public bool IsStepCompleted(Step step)
    {
        return this.step > step;
    }

    public bool IsCompleted()
    {
        return step == Step.Completed;
    }

    public void CompleteStep(Step step)
    {
        Debug.Assert(step == this.step, "Wrong tutorial step: " + step);
        Debug.Assert(step != Step.Completed, "Can't complete 'Completed' step");

        Debug.Log("Tutorial completed: " + step.ToString());

        this.step++;

        if (OnStepCompleted != null)
            OnStepCompleted();
    }
}
