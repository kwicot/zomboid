using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableWeapon : MonoBehaviour
{
    public GameObject AmmoPrefab;
    public WeaponProperties weaponProperties;
    public Transform AmmoSpawnPoint;
    public Transform MuzzleFlashPoint;

    public Transform RightHandTransform;
    public Transform BackTransform;

    public float fireRate;
    public int maxClipSize;

    Transform cursor;








    public int clipSize;
    public enum FireTypes
    {
        Single,
        Auto
    };
    public enum WeaponTypes
    {
        Rifle,
        Pistol,
        Sniper,
        Shootgun
    };
    public FireTypes fireType;
    public WeaponTypes weaponType;









    float delay;
    InputManager _input;
    void Start()
    {
        RightHandTransform = GameObject.FindGameObjectWithTag("Rhand").GetComponent<Transform>();
        BackTransform = GameObject.FindGameObjectWithTag("Back").GetComponent<Transform>();
        UnEquipt();
        clipSize = Random.Range(0, 32);
        cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Transform>();

    }

    void Update()
    {
        delay -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (delay < 0)
        {
            GameObject ammo = Instantiate(AmmoPrefab, AmmoSpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            Vector3 offset = cursor.position;
            offset.y = AmmoSpawnPoint.transform.position.y;
            Vector3 dir = offset - AmmoSpawnPoint.transform.position;
            ammo.GetComponent<AmmoScript>().shoot(dir);
            //RaycastHit hit;
            //if (Physics.Raycast(AmmoSpawnPoint.transform.position, dir, out hit))
            //{
            //    if(hit.transform.tag == "Enemy")
            //    {
            //        Debug.Log("hit");
            //        hit.transform.gameObject.GetComponent<EnemyController>().MakeDamage(30);
            //    }
            //}
            clipSize--;
            delay = fireRate;
        }
        else
        {
            //some sound
        }
    }
    public void Reload()
    {

    }

    public void Equipt()
    {
        transform.SetParent(RightHandTransform);
        transform.localPosition = weaponProperties.positionOnHand;
        transform.localRotation = Quaternion.Euler(weaponProperties.rotationOnHand);
    }
    public void UnEquipt()
    {
        transform.SetParent(BackTransform);
        transform.localPosition = weaponProperties.positionOnBack;
        transform.localRotation = Quaternion.Euler(weaponProperties.rotationOnBack);
    }

}
