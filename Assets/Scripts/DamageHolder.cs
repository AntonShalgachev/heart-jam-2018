﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHolder : MonoBehaviour
{
    [SerializeField]
    private int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var meteor = collision.gameObject.GetComponent<enemy_meteor>();

        if (meteor)
        {
            meteor.healthHit(damage,true);
            Destroy(gameObject);

            var tutorial = TutorialController.Instance;

            if (tutorial.IsEnabled() && tutorial.GetStep() == TutorialController.Step.DestroyEnemiesWithSatellite)
            {
                var movement = collision.gameObject.GetComponent<EnemyMeteorMovement>();
                if (tutorial.OnEnemyMeteorDestroyed(movement))
                    tutorial.CompleteStep(TutorialController.Step.DestroyEnemiesWithSatellite);
            }
        }
    }
}
