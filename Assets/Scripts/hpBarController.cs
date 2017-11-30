using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class hpBarController : MonoBehaviour
{

    public Transform followBy;
    public Image fgImage;

    // Use this for initialization
    void Start()
    {
        print("start bar controller");
        followBy.GetComponent<Health>().PropertyChanged += InstanceOnPropertyChanged;
        UpdateView();
    }

    // Update is called once per frame
    void Update()
    {
        if (followBy == null)
        {
            print("destroy bar controller");
            Destroy(gameObject);
        }
        else
        {
            var _pos = followBy.position;
            _pos.y += 1;// (10 + followBy.GetComponent<SpriteRenderer>().bounds.size.y);
            transform.position = Camera.main.WorldToScreenPoint(_pos);
        }
    }

    private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        UpdateView();
    }
    private void UpdateView()
    {
        var _health = followBy.GetComponent<Health>();
        fgImage.fillAmount = _health.GetHealth() / _health.GetMaxHealth();
    }
}
