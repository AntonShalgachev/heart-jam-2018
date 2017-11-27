using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDrop : MonoBehaviour
{
    public enum Mode
    {
        OnDeath,
        Manual,
    }

    public Mode mode;
    public List<GameObject> drops;
    public float probability;

    GameObject GetRandomDrop()
    {
        return RandomHelper.Instance().GetItem(drops);
    }

    public void Drop()
    {
        if (RandomHelper.Instance().GetBool(probability))
        {
            var drop = GetRandomDrop();
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
#warning antonsh fix me pleeease (OnDestroy is called when exiting game)
        if (mode == Mode.OnDeath)
            Drop();
    }
}
