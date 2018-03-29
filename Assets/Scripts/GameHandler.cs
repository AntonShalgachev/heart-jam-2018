using Assets.Scripts.ShipSatellite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

    public Ship ship;
    public int mouse_damage = 1;
    public GameObject hud;
    public GameObject explosionPrefub;
    public GameObject backPrefub;

    public float gameDistance = 0;
    public float gameSpeed = 10;
    public float gameSpeedDelta = 1;
    public float gameDistanceAddPeriod = 0.1f;

    [SerializeField]
    private RandomHelper.Range backTimeRange;

    [SerializeField]
    private MeteorSpawner meteorSpawner;

    public Transform shipGamePoint;

    private GameObject gui_hud;
    private GameObject gui_lose;
    private GameObject gui_start;
    private GameObject gui_tutorial;

    private GameObject gui_hp_bar;
    private GameObject gui_energy_bar;
    private GameObject gui_distance;
    private GameObject gui_money;
    private GameObject gui_helpOverlay;
    private static bool restartScene = false;
    // Use this for initialization
    void Awake () {
        if(hud != null)
        {
            gui_hud = hud.transform.GetChild(2).gameObject;
            gui_lose = hud.transform.GetChild(3).gameObject;
            gui_start = hud.transform.GetChild(0).gameObject;
            gui_helpOverlay = hud.transform.GetChild(1).gameObject;
            gui_tutorial = hud.transform.GetChild(4).gameObject;
            if (gui_hud != null)
            {
                gui_hp_bar = gui_hud.transform.GetChild(4).GetChild(0).gameObject;
                gui_energy_bar = gui_hud.transform.GetChild(5).GetChild(0).gameObject;
                gui_distance = gui_hud.transform.GetChild(6).gameObject;
                gui_money = gui_hud.transform.GetChild(7).gameObject;
            }
        }
        gameDistance = 0;
        updateDistance(0);
        ship.OnCurrencyChanged += OnMoneyChanged;
        PlayerPrefs.GetString("needTutor", "true");
        restartScene = false;
    }

    private void Start()
    {
        StartCoroutine(StartBackAdd());
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                check_mine(hit.collider.gameObject);
                check_ship(hit.collider.gameObject);
                check_enemy(hit.collider.gameObject);
                check_worker(hit.collider.gameObject);
            }
        }
        if(ship == null)
        {
            if(gui_lose != null)
            {
                gui_lose.SetActive(true);
                var _text = gui_lose.transform.GetChild(2).gameObject.GetComponent<Text>();
                if (_text != null)
                {
                    _text.text = "Passed distance - " + gameDistance.ToString();
                }
            }
        }
        else
        {
            if(gui_hp_bar != null)
            {
                var _img = gui_hp_bar.GetComponent<Image>();
                if (_img != null)
                {
                    _img.fillAmount = ship.getHealthAmount();
                }
            }
            if (gui_energy_bar != null)
            {
                var _img = gui_energy_bar.GetComponent<Image>();
                if (_img != null)
                {
                    _img.fillAmount = ship.getEnergyAmount();
                }
            }
        }
    }

    private void check_mine(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<mine_meteor>();
        if (_cmp != null && IsActionAllowed(TutorialController.Step.MineMeteor))
        {
            addWorker(ship.try_to_get_satellite(), _cmp.workers, workType.miner, _cmp.gameObject);
            TryCompleteStep(TutorialController.Step.MineMeteor);
        }
    }
    private void check_ship(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<Ship>();
        if (_cmp != null && IsActionAllowed(TutorialController.Step.DestroyEnemiesWithSatellite))
        {
            addWorker(ship.try_to_get_satellite(), _cmp.workers, workType.defer, _cmp.gameObject);
        }
    }
    private void check_enemy(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<enemy_meteor>();
        if (_cmp != null && IsActionAllowed(TutorialController.Step.DestroyEnemiesWithFinger))
        {
            _cmp.healthHit(mouse_damage);
            if (!_cmp.IsAlive())
            {
                ship.OnMeteorDestroyedByMouse();

                EnemyMeteorMovement movement = hit.GetComponent<EnemyMeteorMovement>();
                if (TutorialController.Instance.OnEnemyMeteorDestroyed(movement))
                    TryCompleteStep(TutorialController.Step.DestroyEnemiesWithFinger);
            }
        }
    }

    private void check_worker(GameObject hit)
    {
        var _cmp = hit.transform.gameObject.GetComponent<ship_satellite>();
        if (_cmp != null && IsActionAllowed(TutorialController.Step.CancelMining))
        {
            _cmp.stopWork();
            TryCompleteStep(TutorialController.Step.CancelMining);
        }
    }

    private bool addWorker(ship_satellite _sat, List<ship_satellite> _workers, workType _wType, GameObject _owner)
    {
        bool _res = false;
        if (_sat != null && _workers != null)
        {
            _workers.Add(_sat);
            _sat.work = _wType;
            _sat.owner = _owner;
            _res = true;
        }
        return _res;
    }

    private void updateDistance(float _value)
    {
        var tutorial = TutorialController.Instance;
        if (!tutorial.IsEnabled() || !tutorial.IsStepCompleted(TutorialController.Step.InitialDelay))
        {
            gameDistance += _value;
            if (gui_distance != null)
            {
                var _text = gui_distance.GetComponent<Text>();
                if (_text != null)
                {
                    _text.text = "Distance : " + gameDistance.ToString();
                }
            }
        }
    }

    private IEnumerator StartAddDistance()
    {
        while (true)
        {
            if (ship != null)
            {
                updateDistance(gameSpeed);
                setText(gui_hud.transform.GetChild(0).GetChild(0).gameObject, ship.getPrice(Ship.Items.HP).ToString());
                setText(gui_hud.transform.GetChild(1).GetChild(0).gameObject, ship.getPrice(Ship.Items.Satellite).ToString());
                setText(gui_hud.transform.GetChild(2).GetChild(0).gameObject, ship.getPrice(Ship.Items.Shield).ToString());
                yield return new WaitForSeconds(gameDistanceAddPeriod);
            }
            else
            {
                break;
            }
        }
    }
    private IEnumerator StartBackAdd()
    {
        while (true)
        {
            if (backPrefub != null)
            {
                var _time = backTimeRange.GetRandom();
                var _inst = Instantiate(backPrefub).GetComponent<ScrollBackground>();
                _inst.smooth = true;
                yield return new WaitForSeconds(_time);
            }
            else
            {
                break;
            }
        }
    }

    public void btn_restart()
    {
        /*var _objs = GameObject.FindObjectsOfType<MonoBehaviour>();
        foreach(MonoBehaviour m in _objs)
        {
            m.StopAllCoroutines();
        }*/
        restartScene = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void btn_start_game()
    {
        if(gui_start && gui_tutorial)
        {
            gui_start.SetActive(false);
            gui_tutorial.SetActive(false);
            StartCoroutine(StartGameAnim());
            StartCoroutine(StartAddDistance());
        }
        gameDistance = 0;
    }

    public void StartTutorial()
    {
        TutorialController.Instance.SetTutorialEnabled(true);
        meteorSpawner.OnTutorialStarted();
        btn_start_game();
        ProcessNextTutorialStep();

        TutorialController.Instance.OnStepCompleted += ProcessNextTutorialStep;
    }

    private IEnumerator StartGameAnim()
    {
        while (true)
        {
            if (ship != null)
            {
                float step = 5 * Time.deltaTime;
                var _point = Vector3.zero;
                if(shipGamePoint != null)
                {
                    _point = shipGamePoint.position;
                }
                ship.transform.position = Vector3.MoveTowards(ship.transform.position, _point, step);
                if (Vector2.Distance(_point, ship.transform.position) <= 0.1)
                {
                    if (ship != null)
                    {
                        ship.godModeSwitch(false);
                        createExplode();
                        if(gui_hud != null)
                        {
                            gui_hud.SetActive(true);
                        }
                    }
                    break;
                }
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void createExplode()
    {
        if(explosionPrefub != null)
        {
            var _inst = Instantiate(explosionPrefub);
            _inst.transform.position = ship.transform.position;
        }
    }

    public void OnMoneyChanged(int value)
    {
        if (gui_money != null)
        {
            var _text = gui_money.GetComponent<Text>();
            if (_text != null)
            {
                _text.text = "Money: " + value.ToString();
            }
        }
    }

    public static bool isRestarting()
    {
        return restartScene;
    }
    public float GetDistance()
    {
        return gameDistance;
    }
    
    public void helpOverlaySetActive(bool _val)
    {
        if(gui_helpOverlay != null)
        {
            gui_helpOverlay.SetActive(_val);
        }
    }

    private void TryCompleteStep(TutorialController.Step step)
    {
        var controller = TutorialController.Instance;
        if (controller.IsEnabled() && controller.GetStep() == step)
        {
            controller.CompleteStep(step);
        }
    }

    private bool IsActionAllowed(TutorialController.Step step)
    {
        var controller = TutorialController.Instance;
        if (controller.IsEnabled())
            return controller.GetStep() == step;

        return true;
    }

    public void ProcessNextTutorialStep()
    {
        var controller = TutorialController.Instance;

        if (!controller.IsEnabled())
            return;

        if (controller.IsCompleted())
        {
            Debug.Log("Tutorial completed");
            restartScene = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        var step = controller.GetStep();

        switch(step)
        {
            case TutorialController.Step.InitialDelay:
                StartCoroutine(
                    WaitTutorialDelay(TutorialController.Step.InitialDelay,
                    TutorialController.Instance.initialDelay));
                break;
            case TutorialController.Step.ShowUI1:
                controller.ShowUIForStep(step);
                break;
            case TutorialController.Step.MineMeteor:
                ProcessSpawnMiner();
                break;
            case TutorialController.Step.DelayBeforeCancel:
                StartCoroutine(
                    WaitTutorialDelay(TutorialController.Step.DelayBeforeCancel,
                    TutorialController.Instance.delayBeforeCancel));
                break;
            case TutorialController.Step.ShowUI2:
                controller.ShowUIForStep(step);
                break;
            case TutorialController.Step.CancelMining:
                // nothing
                break;
            case TutorialController.Step.RemoveMineMeteor:
                ProcessRemoveMineMeteor();
                break;

            case TutorialController.Step.DelayBeforeEnemies:
                StartCoroutine(
                    WaitTutorialDelay(TutorialController.Step.DelayBeforeEnemies,
                    TutorialController.Instance.delayBeforeEnemies));
                break;
            case TutorialController.Step.ShowUI3:
                controller.ShowUIForStep(step);
                break;
            case TutorialController.Step.SpawnEnemies1:
                ProcessSpawnEnemies1();
                break;
            case TutorialController.Step.DelayBeforePause1:
                StartCoroutine(
                    WaitTutorialDelay(TutorialController.Step.DelayBeforePause1,
                    TutorialController.Instance.delayBeforePause1));
                break;
            case TutorialController.Step.PauseEnemies1:
                ProcessPauseEnemies1();
                break;
            case TutorialController.Step.ShowUI4:
                controller.ShowUIForStep(step);
                break;
            case TutorialController.Step.DestroyEnemiesWithFinger:
                // nothing
                break;

            case TutorialController.Step.SpawnEnemies2:
                ProcessSpawnEnemies2();
                break;
            case TutorialController.Step.DelayBeforePause2:
                StartCoroutine(
                    WaitTutorialDelay(TutorialController.Step.DelayBeforePause2,
                    TutorialController.Instance.delayBeforePause2));
                break;
            case TutorialController.Step.PauseEnemies2:
                ProcessPauseEnemies2();
                break;
            case TutorialController.Step.ShowUI5:
                controller.ShowUIForStep(step);
                break;
            case TutorialController.Step.DestroyEnemiesWithSatellite:
                // nothing
                break;
            case TutorialController.Step.ShowUI6:
                controller.ShowUIForStep(step);
                break;

            default:
                Debug.Assert(false, "Unknown tutorial step: " + step.ToString());
                break;
        }
    }

    private IEnumerator WaitTutorialDelay(TutorialController.Step step, float delay)
    {
        yield return new WaitForSeconds(delay);
        TutorialController.Instance.CompleteStep(step);
    }

    public void ProcessSpawnMiner()
    {
        meteorSpawner.SpawnTutorialMinerMeteor();
    }

    public void ProcessRemoveMineMeteor()
    {
        TutorialController.Instance.TutorialMineMeteor.Abort();
        TutorialController.Instance.CompleteStep(TutorialController.Step.RemoveMineMeteor);
    }

    public void ProcessSpawnEnemies1()
    {
        meteorSpawner.SpawnTutorialEnemyMeteors();
        TutorialController.Instance.CompleteStep(TutorialController.Step.SpawnEnemies1);
    }

    public void ProcessPauseEnemies1()
    {
        foreach (var meteor in TutorialController.Instance.TutorialEnemyMeteors)
            meteor.Paused = true;

        TutorialController.Instance.CompleteStep(TutorialController.Step.PauseEnemies1);
    }

    public void ProcessSpawnEnemies2()
    {
        meteorSpawner.SpawnTutorialEnemyMeteors();
        TutorialController.Instance.CompleteStep(TutorialController.Step.SpawnEnemies2);
    }

    public void ProcessPauseEnemies2()
    {
        foreach (var meteor in TutorialController.Instance.TutorialEnemyMeteors)
            meteor.Paused = true;

        TutorialController.Instance.CompleteStep(TutorialController.Step.PauseEnemies2);
    }
    void setText(GameObject _obj, string _str)
    {
        if(_obj != null)
        {
            var _text = _obj.GetComponent<Text>();
            if(_text != null)
            {
                _text.text = _str;
            }
        }
    }
}
