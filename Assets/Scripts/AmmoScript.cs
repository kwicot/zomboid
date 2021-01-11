using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    public float speed;
    public float damage;
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 dir = _rb.velocity - transform.position;
        transform.LookAt(dir);
    }
    public void shoot(Vector3 dir)
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(dir * speed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit" + collision.gameObject.name);
        Destroy(gameObject);
        if(collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().MakeDamage(damage);
        }
    }
}
