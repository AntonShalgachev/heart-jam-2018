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

        public float workTimeMine = 1f;

        private bool isMining = false;
        private bool loseConnect = false;
        private bool isDefing = false;
        private LineRenderer lineRenderer;
		
        private float workTimeMine_amount = 5f;
        private float mine_amount = 0;
        private MeteorShooter shooter;
        // Use this for initialization
        void Start()
        {
            shooter = GetComponent<MeteorShooter>();
            Debug.Assert(shooter);

            work = workType.idle;
            lineRenderer = GetComponent<LineRenderer>();
            if(lineRenderer != null)
            {
                lineRenderer.endColor = new Color(0, 0, 0, 0.2f);
            }
            workTimeMine_amount = workTimeMine;
        }

        // Update is called once per frame
        void Update()
        {
            if(ship == null)
            {
                Destroy(gameObject);
                return;
            }

            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, ship.transform.position);
                float _dist = Vector2.Distance(transform.position, ship.transform.position) / maxDistance;
                lineRenderer.startColor = new Color(_dist, 0, 0, _dist);
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
                                shooter.SetActive(true);
                            }
                        }
                    }
                    break;
                case workType.miner:
                    if (owner != null && ship != null)
                    {
                        if (isMining)
                        {

                            workTimeMine_amount -= Time.deltaTime;
                            if (workTimeMine_amount <= 0)
                            {
                                transform.position = Vector3.MoveTowards(transform.position, ship.transform.position, step);
                                if (Vector2.Distance(transform.position, ship.transform.position) <= minDistance)
                                {
                                    isMining = false;
                                }
                            }
                            else
                            {
                                var _mine = owner.GetComponent<mine_meteor>();
                                if (_mine != null)
                                {
                                    mine_amount += _mine.mine_params.mineSpeed; 
                                }
                            }
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, owner.transform.position, step);
                            if (Vector2.Distance(transform.position, owner.transform.position) <= minDistance)
                            {
                                isMining = true;
                                workTimeMine_amount = workTimeMine;

                                var _ship = ship.GetComponent<ship>();
                                if (_ship != null)
                                {
                                    _ship.addEnergy(mine_amount);
                                }
                                mine_amount = 0;
                            }
                        }
                    }
                break;
            }
            if(Vector2.Distance(transform.position, ship.transform.position) > maxDistance)
            {
                loseConnect = true;
                wrapDestroy();

            }
            else
            {
                loseConnect = false;
            }

        }
        void wrapDestroy()
        {
            Destroy(gameObject);
        }
        public void stopWork()
        {
            if(work != workType.idle)
            {
                work = workType.idle;
            }
        }
    }
}
