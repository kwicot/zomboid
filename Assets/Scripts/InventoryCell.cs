using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField]
    Text _textName;
    [SerializeField]
    Text _textCount;
    [SerializeField]
    Image _sprite;


    public string namee;
    public int count;
    public Sprite sprite;
    void Start()
    {
        _textCount.text = count.ToString();
        _textName.text = namee;
        _sprite.sprite = sprite;
    }
}
