using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;

    public float initialDelay;
    public float delayBeforeCancel;
    public float delayBeforeEnemies;
    public float delayBeforePause1;
    public float delayBeforePause2;

    public string UIText1;
    public string UIText2;
    public string UIText3;
    public string UIText4;
    public string UIText5;

    public event Action OnStepCompleted;

    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private GameObject tutorialOverlay;

    public enum Step
    {
        InitialDelay,

        ShowUI1, // show that we are loosing fuel

        MineMeteor,
        DelayBeforeCancel,
        ShowUI2, // you have gained enough fuel, now stop before it flies away
        CancelMining,
        RemoveMineMeteor,

        DelayBeforeEnemies,
        ShowUI3, // meet meteors
        SpawnEnemies1,
        DelayBeforePause1,
        PauseEnemies1,
        ShowUI4, // Tap twice to explode meteors. You will gain money for that
        DestroyEnemiesWithFinger,
        SpawnEnemies2,
        DelayBeforePause2,
        PauseEnemies2,
        ShowUI5, // Tap on the ship to send your drone to protect it. For that you will gain no money
        DestroyEnemiesWithSatellite,

        Completed,
    }

    private Step step = Step.InitialDelay;

    private bool uiShown = false;
    private Step uiStep;

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

    public string GetDescription(Step step)
    {
        switch(step)
        {
            case Step.ShowUI1:
                return UIText1;
            case Step.ShowUI2:
                return UIText2;
            case Step.ShowUI3:
                return UIText3;
            case Step.ShowUI4:
                return UIText4;
            case Step.ShowUI5:
                return UIText5;
        }

        Debug.Assert(false, "Can't find description for step " + step.ToString());
        return "";
    }

    public void ShowUIForStep(Step step)
    {
        uiStep = step;
        uiShown = true;

        tutorialOverlay.SetActive(true);
        descriptionText.text = GetDescription(step);
    }

    public void OnUIDismissed()
    {
        tutorialOverlay.SetActive(false);

        if (uiShown)
            CompleteStep(uiStep);
        uiShown = false;
    }
}
