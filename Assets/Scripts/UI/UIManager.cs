using Assets.Scripts.Common.Helpers;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public GameObject HPBar;
    public GameObject inventory;
    public Sprite emptySlot;

    private GameObject player;
    private List<Image> slots;
    // Use this for initialization
    void Start()
    {
        var _player = GameObject.FindWithTag("Player");
        if (_player != null)
        {
            player = _player;
            player.GetComponent<Health>().PropertyChanged += InstanceOnPropertyChanged;
            player.GetComponent<Inventory>().PropertyChanged += InstanceOnPropertyChanged;
            print("find player!");
        }
        slots = new List<Image>();
        for (int i = 0; i < 3; i++)
        {
            slots.Add(inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>());
        }
    }

    private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        UpdateView();
    }

    private void UpdateView()
    {
        if (player != null)
        {
            setHP(player.GetComponent<Health>().GetHealth(), player.GetComponent<Health>().GetMaxHealth());
            updateInventory(player.GetComponent<Inventory>().GetItems());
        }
    }

    private void setHP(float _val, float _max_val)
    {
        HPBar.transform.GetChild(0).GetComponent<Image>().fillAmount = _val / _max_val;
        HPBar.GetComponentInChildren<Text>().text = _val.ToString() + "/" + _max_val.ToString();
    }
    private void updateInventory(List<Collectible> _list)
    {
        int count = 0;
        foreach(Image img in slots)
        {
            img.sprite = emptySlot;
            img.color = Color.white;
        }
        foreach (Collectible item in _list)
        {
            var _sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
            var _color = item.gameObject.GetComponent<SpriteRenderer>().color;
            if(_sprite != null)
            {
                slots[count].sprite = _sprite;
                if(_color != null)
                {
                    slots[count].color = _color;
                }
                count++;
            }
            if (count > 3) break;
        }
    }
}
