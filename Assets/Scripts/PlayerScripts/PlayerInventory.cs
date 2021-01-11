using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class PlayerInventory : MonoBehaviour
{
    public List<int> startItems = new List<int>();



    public ItemsDateBase datebase;
    public Transform spineTrans;
    public float weight = 0;
    public float maxWeight = 100;
    public float totalMaxWeight;

    PlayerController _player;


    [SerializeField]
    GameObject firstWeapon;
    [SerializeField]
    GameObject secondWeapon;
    [SerializeField]
    GameObject currentWeapon;
    [SerializeField]
    GameObject nextWeapon;
    GameObject _itemBar;
    List<TakeItemScript> _itemsInRadius = new List<TakeItemScript>();
    [SerializeField]
    List<InventoryItem> litems = new List<InventoryItem>();
    Animator _anim;
    bool canShoot = false;


    Text _textCurrentAmmo;
    int _currentAmmoInv;
    int _currentAmmoWeapon;


    bool isOpen = false;
    GameObject _inventoryPanel;
    public GameObject cellPrefab;
    public GameObject CellCanvas;
    List<GameObject> lCells = new List<GameObject>();
    void Start()
    {
        totalMaxWeight = maxWeight;
        _itemBar = GameObject.FindGameObjectWithTag("ItemBar");
        _anim = GetComponent<Animator>();
        _player = FindObjectOfType<PlayerController>();
        _textCurrentAmmo = GameObject.FindGameObjectWithTag("TextAmmo").GetComponent<Text>();
        _inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");

        if(startItems.Count > 0)
        {
            if (startItems.Count > 1)
            {
                for (int i = 0; i < startItems.Count; i++)
                {
                    AddItem(startItems[i]);
                }
            }
            else AddItem(startItems[0]);
        }
    }

    void FixedUpdate()
    {
        if (_itemsInRadius.Count > 0)
        {
            _itemBar.SetActive(true);
            FlashClosetsItem();
        }
        else _itemBar.SetActive(false);
        if (currentWeapon == null) _textCurrentAmmo.text = "";
    }

    void AddItem(int id)
    {
        RecalcWeight();
        ScriptableItems item = datebase.GetItem(id);
        bool add = false;
        if (item.stackble)
        {
            for (int i = 0; i < litems.Count; i++)
            {
                if (litems[i].ID == id)
                {
                    print("Find");
                    if (totalMaxWeight - weight >= item.weight)
                    {
                        print("Added");
                        litems[i].count++;
                        add = true;
                        break;
                    }
                    else
                    {
                        print("No space");
                        add = false;
                        break;
                    }
                }
            }
            if (!add)
            {
                print("Not find");
                if (totalMaxWeight - weight >= item.weight)
                {
                    InventoryItem nitem = ScriptableObject.CreateInstance<InventoryItem>();
                    //string path = "Assets/" + item.itemName;
                    //AssetDatabase.CreateAsset(nitem, path + ".asset");
                    //AssetDatabase.SaveAssets();
                    //nitem.path = path;
                    nitem.ID = item.ID;
                    nitem.count = 1;
                    nitem.weight = item.weight;
                    litems.Add(nitem);
                    add = true;
                    print("Added");
                }
                else
                {
                    print("No space");
                    add = false;
                }
            }
        }
        else
        {
            if (totalMaxWeight - weight >= item.weight)
            {
                InventoryItem nitem = ScriptableObject.CreateInstance<InventoryItem>();
                //string path = "Assets/" + item.itemName;
                //AssetDatabase.CreateAsset(nitem, path + ".asset");
                //AssetDatabase.SaveAssets();
                //nitem.path = path;
                nitem.ID = item.ID;
                nitem.count = 1;
                nitem.weight = item.weight;
                litems.Add(nitem);
                add = true;
            }
            else
            {
                add = false;
            }
        }
        FindAmmo();
        RecalcWeight();
        if (currentWeapon == null | firstWeapon == null | secondWeapon == null) CheckWeapon();
    }
    bool AddItem(TakeItemScript scr)
    {
        RecalcWeight();
        ScriptableItems item = datebase.GetItem(scr.id);
        bool add = false;
        if (item.stackble)
        {
            for (int i = 0; i < litems.Count; i++)
            {
                if (litems[i].ID == scr.id)
                {
                    print("Find");
                    if (totalMaxWeight - weight >= item.weight)
                    {
                        print("Added");
                        litems[i].count++;
                        add = true;
                        break;
                    }
                    else
                    {
                        print("No space");
                        add = false;
                        break;
                    }
                }
            }
            if (!add)
            {
                print("Not find");
                if (totalMaxWeight - weight >= item.weight)
                {
                    InventoryItem nitem = ScriptableObject.CreateInstance<InventoryItem>();
                    //string path = "Assets/" + item.itemName;
                    //AssetDatabase.CreateAsset(nitem, path + ".asset");
                    //AssetDatabase.SaveAssets();
                    //nitem.path = path;
                    nitem.ID = item.ID;
                    nitem.count = 1;
                    nitem.weight = item.weight;
                    litems.Add(nitem);
                    add = true;
                    print("Added");
                }
                else
                {
                    print("No space");
                    add = false;
                }
            }
        }
        else
        {
            if (totalMaxWeight - weight >= item.weight)
            {
                InventoryItem nitem = ScriptableObject.CreateInstance<InventoryItem>();
                //string path = "Assets/" + item.itemName;
                //AssetDatabase.CreateAsset(nitem, path + ".asset");
                //AssetDatabase.SaveAssets();
                //nitem.path = path;
                nitem.ID = item.ID;
                nitem.count = 1;
                nitem.weight = item.weight;
                litems.Add(nitem);
                add = true;
            }
            else
            {
                add = false;
            }
        }
        FindAmmo();
        RecalcWeight();
        if (currentWeapon == null | firstWeapon == null | secondWeapon == null) CheckWeapon();
        return add;
    }

    void CheckWeapon()
    {
        for (int i = 0; i < litems.Count; i++)
        {
            if (datebase.IsWeapon(litems[i].ID)) {
                if (firstWeapon == null)
                {
                    firstWeapon = Instantiate(datebase.GetPrefab(litems[i].ID));
                    litems[i].equipted = true;
                    litems[i].obj = firstWeapon;
                }
                else if (secondWeapon == null)
                {
                    if(litems[i].equipted == false)
                    {
                        secondWeapon = Instantiate(datebase.GetPrefab(litems[i].ID));
                        litems[i].equipted = true;
                        litems[i].obj = secondWeapon;

                    }
                }
            }
        }
        if (currentWeapon == null && firstWeapon != null)
        {
        }
    }
    void PlayAnim(ShootableWeapon.WeaponTypes t, int i)
    {
        canShoot = false;
        switch (t)
        {
            case ShootableWeapon.WeaponTypes.Rifle:
                {
                    if(i == 1)
                    {
                        Debug.Log("Equipt rifle anim");
                        _anim.Play("Equipt rifle", 1);
                        _player.ChangeMoveType(PlayerController.MoveTypes.TwoHandWeapon);
                    }
                    else if(i == 2)
                    {
                        Debug.Log("UnEquipt rifle anim");
                        _anim.Play("unEquipt rifle", 1);
                        _player.ChangeMoveType(PlayerController.MoveTypes.FreeHand);
                    }
                    else if(i == 3)
                    {
                        Debug.Log("Change rifle anim");
                        _anim.Play("unEquipt rifle2", 1);
                        _player.ChangeMoveType(PlayerController.MoveTypes.TwoHandWeapon);
                    }
                    else if(i == 4)
                    {
                        Debug.Log("Reload rifle");
                        _anim.Play("ReloadRifle", 1);
                    }
                }
                break;
            case ShootableWeapon.WeaponTypes.Pistol:
                {
                    if (i == 1)
                    {

                    }
                    else if (i == 2)
                    {

                    }
                    else if (i == 3)
                    {

                    }
                }
                break;
            case ShootableWeapon.WeaponTypes.Sniper:
                {

                }
                break;
            case ShootableWeapon.WeaponTypes.Shootgun:
                {

                }
                break;
        }
    }
    public void EquiptFirstWeapon()
    {
        canShoot = false;
        if(currentWeapon == null & firstWeapon != null)
        {
            currentWeapon = firstWeapon;
            PlayAnim(currentWeapon.GetComponent<ShootableWeapon>().weaponType, 1);
        }
        else if(currentWeapon == firstWeapon)
        {
            PlayAnim(currentWeapon.GetComponent<ShootableWeapon>().weaponType, 2);

        }
        else if( currentWeapon == secondWeapon & firstWeapon != null)
        {
            PlayAnim(currentWeapon.GetComponent<ShootableWeapon>().weaponType, 3);
            nextWeapon = firstWeapon;
        }
    }

    public void EquiptSecondWeapon()
    {
        canShoot = false;
        if(currentWeapon == null & secondWeapon != null)
        {
            currentWeapon = secondWeapon;
            PlayAnim(currentWeapon.GetComponent<ShootableWeapon>().weaponType, 1);
        }
        else if(currentWeapon == secondWeapon)
        {
            PlayAnim(currentWeapon.GetComponent<ShootableWeapon>().weaponType, 2);
        }
        else if(currentWeapon == firstWeapon & secondWeapon != null)
        {
            PlayAnim(currentWeapon.GetComponent<ShootableWeapon>().weaponType, 3);
            nextWeapon = secondWeapon;
        }
    }
    public void UnEquiptWeapon()
    {
        if(currentWeapon != null)
        {
        }
    }
    public void ChangeWeapon()
    {
        currentWeapon.GetComponent<ShootableWeapon>().UnEquipt();
        currentWeapon = nextWeapon;
        currentWeapon.GetComponent<ShootableWeapon>().Equipt();
    }
    public void WeaponEquipted()
    {
        Debug.Log("weapon Equipted");
        canShoot = true;
        _currentAmmoWeapon = currentWeapon.GetComponent<ShootableWeapon>().clipSize;
        FindAmmo();
        ReloadtextAmmo();
    }
    public void WeaponUnEquipted()
    {
        Debug.Log("weapon UnEquipted");
        currentWeapon.GetComponent<ShootableWeapon>().UnEquipt();
        currentWeapon = null;
        canShoot = false;
    }
    public void SetTransform(int i)
    {
        Debug.Log("set transfrom");
        if (i == 1)
        {
            currentWeapon.GetComponent<ShootableWeapon>().Equipt();
        }
        if(i == 2)
        {
            currentWeapon.GetComponent<ShootableWeapon>().UnEquipt();
            currentWeapon = null;
        }
    }
    void RecalcWeight()
    {
        weight = 0;
        for(int i = 0;i<litems.Count; i++)
        {
            weight += litems[i].weight * litems[i].count;
        }
    }
    void RecalcClosetsItem()
    {
        if (_itemsInRadius.Count > 1)
        {
            for (int i = 1; i < _itemsInRadius.Count; i++)
            {
                if (Vector3.Distance(_itemsInRadius[i].gameObject.transform.position, gameObject.transform.position) < Vector3.Distance(_itemsInRadius[0].gameObject.transform.position, gameObject.transform.position))
                {
                    var holder = _itemsInRadius[0];
                    _itemsInRadius[0] = _itemsInRadius[i];
                    _itemsInRadius[i] = holder;
                }
            }
        }
    }
    void TakeClosetsItem()
    {
        TakeItemScript item = _itemsInRadius[0];
        int count = item.count;
        for (; count > 0; count--)
        {
            print(count);
            if (AddItem(item))
            {
                item.count--;
            }
            else
            {
                Debug.Log("Not anought space");
            }
            if (item.count == 0)
            {
                _itemsInRadius.RemoveAt(0);
                Destroy(item.gameObject);
                break;
            }

        }

    }
    void FlashClosetsItem()
    {
        RecalcClosetsItem();
        GameObject.FindGameObjectWithTag("ItemBarImage").GetComponentInChildren<Image>().sprite = datebase.GetSprite(_itemsInRadius[0].id);
        _itemBar.GetComponentInChildren<Text>().text = datebase.GetName(_itemsInRadius[0].id);
        Vector3 pos = Camera.main.WorldToScreenPoint(_itemsInRadius[0].transform.position);
        pos.x -= 90;
        pos.y += 110;
        _itemBar.transform.position = pos;
    }
    void FindAmmo()
    {
        if (currentWeapon != null)
        {
            int count = 0;
            for (int i = 0; i < litems.Count; i++)
            {
                if (currentWeapon.GetComponent<ShootableWeapon>().AmmoPrefab == datebase.GetPrefab(litems[i].ID))
                {
                    count += litems[i].count;
                }
            }
            _currentAmmoInv = count;
            ReloadtextAmmo();
        }
        ReloadInventoryCells();

    }

    public void TakeKeyPressed()
    {
        if(_itemsInRadius.Count > 0)
        TakeClosetsItem();
    }
    
    public void ShootKeyPressed()
    {
        FindAmmo();
        if (currentWeapon != null)
        {
            _currentAmmoWeapon = currentWeapon.GetComponent<ShootableWeapon>().clipSize;
             ReloadtextAmmo();
            if(_currentAmmoWeapon > 0)
            {
                currentWeapon.GetComponent<ShootableWeapon>().Shoot();
            }
            else
            {
                if (_currentAmmoInv > 0)
                {
                    PlayAnim(currentWeapon.GetComponent<ShootableWeapon>().weaponType, 4);
                }
            }
        }
        ReloadtextAmmo();
    }
    public void ReloadKeyPressed()
    {
        FindAmmo();
        if(currentWeapon != null && _currentAmmoInv > 0) PlayAnim(currentWeapon.GetComponent<ShootableWeapon>().weaponType, 4);
    }
    public void WeaponReloaded()
    {
        FindAmmo();
        int need = currentWeapon.GetComponent<ShootableWeapon>().maxClipSize - currentWeapon.GetComponent<ShootableWeapon>().clipSize;
        int can = 0;
        if (need <= _currentAmmoInv)
        {
            RemoveAmmo(need);
            can = need;
        }
        else
        {
            can = _currentAmmoInv;
            RemoveAmmo(_currentAmmoInv);
        }
        currentWeapon.GetComponent<ShootableWeapon>().clipSize += can;
        _currentAmmoWeapon = currentWeapon.GetComponent<ShootableWeapon>().clipSize;
        canShoot = true;
        ReloadtextAmmo();
    }
    void RemoveAmmo(int count)
    {
        for(int i = 0;i< litems.Count; i++)
        {
            if (currentWeapon.GetComponent<ShootableWeapon>().AmmoPrefab == datebase.GetPrefab(litems[i].ID))
            {
                litems[i].count -= count;
                if (litems[i].count <= 0)
                {
                    Destroy(litems[i]);
                    litems.RemoveAt(i);
                }
            }
        }



        FindAmmo();
    }
    void ReloadtextAmmo()
    {
            _textCurrentAmmo.text = _currentAmmoWeapon.ToString() + " / " + _currentAmmoInv.ToString();
    }

    public void InventoryKeyPressed()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            _inventoryPanel.SetActive(true);
            if (litems.Count > 0)
            {
                for (int i = 0; i < litems.Count; i++)
                {
                    GameObject cell = Instantiate(cellPrefab, CellCanvas.transform);
                    cell.GetComponent<InventoryCell>().namee = datebase.GetName(litems[i].ID);
                    cell.GetComponent<InventoryCell>().count = litems[i].count;
                    cell.GetComponent<InventoryCell>().sprite = datebase.GetSprite(litems[i].ID);
                    lCells.Add(cell);
                }
            }
        }
        else
        {
            if (lCells.Count > 0)
            {
                for (int i = 0; i < lCells.Count; i++)
                {
                    Destroy(lCells[i]);
                }
                lCells.Clear();
            }
            _inventoryPanel.SetActive(false);

        }
    }
    void ReloadInventoryCells()
    {
        if (isOpen)
        {
            if (litems.Count > 0)
            {

                for (int i = 0; i < lCells.Count; i++)
                {
                    Destroy(lCells[i]);
                }
                lCells.Clear();
                for (int i = 0; i < litems.Count; i++)
                {
                    GameObject cell = Instantiate(cellPrefab, CellCanvas.transform);
                    cell.GetComponent<InventoryCell>().namee = datebase.GetName(litems[i].ID);
                    cell.GetComponent<InventoryCell>().count = litems[i].count;
                    cell.GetComponent<InventoryCell>().sprite = datebase.GetSprite(litems[i].ID);
                    lCells.Add(cell);
                }
            }
        }
    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TakeItem")
        {
            TakeItemScript item = other.GetComponent<TakeItemScript>();
            _itemsInRadius.Add(item);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "TakeItem")
        {
            if (_itemsInRadius.Count > 1)
            {
                for(int i=0;i < _itemsInRadius.Count; i++)
                {
                    if (other.GetComponent<TakeItemScript>() == _itemsInRadius[i]) _itemsInRadius.RemoveAt(i);
                }
            }
            else _itemsInRadius.RemoveAt(0);
        }
    }

    ~PlayerInventory()
    {
        Debug.Log("Deconstructor");
        for(int i =0; i< litems.Count; i++)
        {
            //AssetDatabase.RemoveObjectFromAsset(litems[i]);
            ////AssetDatabase.DeleteAsset(litems[i].path);
            //AssetDatabase.SaveAssets();
        }
    }
}

public class InventoryItem : ScriptableObject
{
    public int ID;
    public int count;
    public float weight;
    public string path;
    public bool equipted = false;
    public GameObject obj;
    InventoryItem()
    {
    }
}
