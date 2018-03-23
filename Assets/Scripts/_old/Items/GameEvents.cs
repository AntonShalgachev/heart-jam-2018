using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class GameEvents : MonoBehaviour
    {
        public int id;
        public string text;
        public bool end;
        public int index;
        // Use this for initialization
        void Start()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
