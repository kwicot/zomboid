using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[CreateAssetMenu(menuName = "Items date base")]
public class ItemsDateBase : ScriptableObject
{
    int Id = 0;
    public List<ScriptableItems> lItems = new List<ScriptableItems>();


    public int ItemsHolder;


    private void OnValidate()
    {
        Id = 0;
        for(int i = 0; i< lItems.Count; i++)
        {
            lItems[i].ID = Id;
            Id++;
        }
        ItemsHolder = Id;
    }

    public GameObject GetPrefab(int id)
    {
        GameObject obj = lItems[id].prefab;
        return obj;
    }
    public Sprite GetSprite(int id)
    {
        Sprite sp = lItems[id].sprite;
        return sp;
    }

    public bool isStacable(int id)
    {
        if (lItems[id].stackble == true) return true;
        else return false;
    }
    public float GetWeight(int id)
    {
        float w = lItems[id].weight;
        return w;
    }
    public string GetName(int id)
    {
        string n = lItems[id].itemName;
        return n;
    }


    public ScriptableItems GetItem(int id)
    {
        ScriptableItems item = lItems[id];
        return item;
    }
    public bool IsWeapon(int id)
    {
        if (lItems[id].type == ScriptableItems.ItemTypes.TwoHandWeapon || lItems[id].type == ScriptableItems.ItemTypes.OneHandWeapon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetId(ScriptableItems item)
    {
        int id = 0;
        for(int i = 0; i< lItems.Count; i++)
        {
            if (lItems[i] == item) id = lItems[i].ID;
        }
        return id;
    }
}
