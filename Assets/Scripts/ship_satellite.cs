using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ShipSatellite
{
    public enum workType { idle, miner, defer };

    public class ship_satellite : MonoBehaviour
    {
        public workType work;
        public GameObject owner;
        public GameObject ship;
        public float speed = 2.0f;
        public float minDistance = 0.01f;
        public float maxDistance = 3f;
        public float defDistance = 1f;
        public float defRotateSpeed = 40f;

        private bool isMining = false;
        private bool loseConnect = false;
        private bool isDefing = false;
        // Use this for initialization
        void Start()
        {
            work = workType.idle;
        }

        // Update is called once per frame
        void Update()
        {
            if(ship == null)
            {
                Destroy(gameObject);
                return;
            }

            float step = speed * Time.deltaTime;
            switch (work)
            {
                case workType.idle:
                    transform.position = Vector3.MoveTowards(transform.position, ship.transform.position, step);
                    break;
                case workType.defer:
                    if (ship != null)
                    {
                        if (isDefing)
                        {
                            transform.RotateAround(ship.transform.position, Vector3.forward, defRotateSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, ship.transform.position + Vector3.up * 10, step);
                            if (Vector2.Distance(transform.position, ship.transform.position) >= defDistance)
                            {
                                isDefing = true;
                            }
                        }
                    }
                    break;
                case workType.miner:
                    if (owner != null && ship != null)
                    {
                        if (isMining)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, ship.transform.position, step);
                            if (Vector2.Distance(transform.position, ship.transform.position) <= minDistance)
                            {
                                isMining = false;
                            }
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, owner.transform.position, step);
                            if (Vector2.Distance(transform.position, owner.transform.position) <= minDistance)
                            {
                                isMining = true;
                            }
                        }
                    }
                break;
            }
            if(Vector2.Distance(transform.position, ship.transform.position) > maxDistance)
            {
                loseConnect = true;
            }
            else
            {
                loseConnect = false;
            }
        }
    }
}
