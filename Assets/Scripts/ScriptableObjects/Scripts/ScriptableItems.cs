using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ScriptableItems : ScriptableObject
{
    
    public int ID;
    public string itemName;
    public string itemDescription;
    public bool stackble;
    public int maxStackSize;

    public int count;
    public float weight;
    public Sprite sprite;
    public GameObject prefab;
    public enum ItemTypes
    {
        Item,
        TwoHandWeapon,
        OneHandWeapon,
        Ammo,
        Food,
        Medical,
        Armor,
        BackPack
    };
    public ItemTypes type;
}
