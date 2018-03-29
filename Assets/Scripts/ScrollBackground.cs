using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {

    public float scrollSpeed;
    public float tileSizeZ;
    public bool smooth = false;
    public Sprite[] backs;
    [SerializeField]
    private RandomHelper.Range lifeRange;
    [SerializeField]
    private RandomHelper.Range xCoordRange;
    [SerializeField]
    private RandomHelper.Range speedRange;

    private Vector3 startPosition;
    private float alpha = 0;
    private float alphaSpeed = 1;
    private GameObject layer;
    private bool started = false;
    private float life_time;
    void Start()
    {
        startPosition = transform.position;
        layer = transform.GetChild(0).gameObject;
        life_time = lifeRange.GetRandom();
        if (smooth)
        {
            transform.position = new Vector3(xCoordRange.GetRandom(), 0);
            if (backs != null && backs.Length > 0)
            {
                var random = RandomHelper.Instance();
                var sprite = backs[random.GetInt(0, backs.Length)];
                setSprite(gameObject, sprite);
                setSprite(layer, sprite);
            }
            scrollSpeed += speedRange.GetRandom();
        }
        else
        {
            setAlpha(gameObject, 1);
            setAlpha(layer, 1);
        }
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        transform.position = startPosition + Vector3.up * newPosition;
        if(smooth)
        {
            if (!started)
            {
                alpha += alphaSpeed * Time.deltaTime;
                setAlpha(gameObject, alpha);
                setAlpha(layer, alpha);
                if(alpha >= 1)
                {
                    started = true;
                }
            }
            else
            {
                life_time -= Time.deltaTime;
                if(life_time <= 0)
                {
                    alpha -= alphaSpeed * Time.deltaTime;
                    setAlpha(gameObject, alpha);
                    setAlpha(layer, alpha);
                    if(alpha <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void setAlpha(GameObject _obj, float _value)
    {
        if(_obj != null)
        {
            var _renderer = _obj.GetComponent<SpriteRenderer>();
            if(_renderer != null)
            {
                _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, _value);
            }
        }
    }
    private void setSprite(GameObject _obj, Sprite _value)
    {
        if (_obj != null)
        {
            var _renderer = _obj.GetComponent<SpriteRenderer>();
            if (_renderer != null)
            {
                _renderer.sprite = _value;
            }
        }
    }
}
