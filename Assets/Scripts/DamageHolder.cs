using System.Collections;
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
            meteor.healthHit(damage);
            Destroy(gameObject);

            var tutorial = TutorialController.Instance;

            var movement = collision.gameObject.GetComponent<EnemyMeteorMovement>();
            if (tutorial.OnEnemyMeteorDestroyed(movement))
            {
                tutorial.CompleteStep(TutorialController.Step.DestroyEnemiesWithSatellite);
            }
        }
    }
}
