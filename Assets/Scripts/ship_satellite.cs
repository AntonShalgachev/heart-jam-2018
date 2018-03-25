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
        public Sprite eyeLight_green;
        public Sprite eyeLight_red;

        private bool isMining = false;
        private bool isDefing = false;
        private LineRenderer lineRenderer;
		
        private float workTimeMine_amount = 5f;
        private float mine_amount = 0;
        private MeteorShooter shooter;
        private GameObject eye;
        private bool eyeState;
        private GameObject engine_left;
        private GameObject engine_right;
        private bool engine_dir_left;
        private bool engine_dir_right;
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
            eye = transform.GetChild(0).gameObject;
            engine_left = transform.GetChild(1).gameObject;
            engine_right = transform.GetChild(2).gameObject;
            eyeState = false;
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
                    if (Vector2.Distance(transform.position, ship.transform.position) <= minDistance)
                    {
                        var _ship = ship.GetComponent<Ship>();
                        if (_ship != null && mine_amount > 0)
                        {
                            _ship.addEnergy(mine_amount);
                            mine_amount = 0;
                        }
                    }
                    isDefing = false;
                    isMining = false;
                    setEye(true);
                    shooter.SetActive(false);
                    break;
                case workType.defer:
                    if (ship != null)
                    {
                        if (isDefing)
                        {
                            transform.RotateAround(ship.transform.position, Vector3.forward, defRotateSpeed * Time.deltaTime);
                            transform.rotation = Quaternion.identity;
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, ship.transform.position + Vector3.up * 10, step);
                            if (Vector2.Distance(transform.position, ship.transform.position) >= defDistance)
                            {
                                isDefing = true;
                                shooter.SetActive(true);
                                setEye(false);
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
                                setEye(true);
                                transform.position = Vector3.MoveTowards(transform.position, ship.transform.position, step);
                                if (Vector2.Distance(transform.position, ship.transform.position) <= minDistance)
                                {
                                    isMining = false;
                                    var _ship = ship.GetComponent<Ship>();
                                    if (_ship != null)
                                    {
                                        _ship.addEnergy(mine_amount);
                                    }
                                }
                            }
                            else
                            {
                                var _mine = owner.GetComponent<mine_meteor>();
                                if (_mine != null)
                                {
                                    mine_amount += _mine.MineSpeed;
                                }
                                transform.position = Vector3.MoveTowards(transform.position, owner.transform.position, step);
                            }
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, owner.transform.position, step);
                            if (Vector2.Distance(transform.position, owner.transform.position) <= minDistance)
                            {
                                isMining = true;
                                workTimeMine_amount = workTimeMine;
                                mine_amount = 0;
                                setEye(false);
                            }
                        }
                    }
                    else
                    {
                        work = workType.idle;
                    }
                break;
            }
            if(Vector2.Distance(transform.position, ship.transform.position) > maxDistance)
            {
                wrapDestroy();

            }
            /// engine anim
            if(engine_left != null)
            {
                var _spr = engine_left.GetComponent<SpriteRenderer>();
                engine_dir_left = Ship.engineAnim_ext(0, _spr, engine_left.transform, engine_dir_left);
            }
            if (engine_right != null)
            {
                var _spr = engine_right.GetComponent<SpriteRenderer>();
                engine_dir_right = Ship.engineAnim_ext(0, _spr, engine_right.transform, engine_dir_right);
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

        void setEye(bool _val)
        {
            if (eyeState != _val)
            {
                eyeState = _val;
                if (eye != null)
                {
                    var _sprite = eye.GetComponent<SpriteRenderer>();
                    if (_sprite != null)
                    {
                        if (_val)
                        {
                            if (eyeLight_green != null)
                                _sprite.sprite = eyeLight_green;
                        }
                        else
                        {
                            if (eyeLight_red != null)
                                _sprite.sprite = eyeLight_red;
                        }
                    }
                }
            }
        }
    }
}
