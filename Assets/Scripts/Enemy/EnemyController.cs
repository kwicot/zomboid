using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float health;
    public float speed;
    public float damage;
    public GameObject HealthBarPrefab;
    public GameObject head;
    public LayerMask mask;
    AudioListener _audioListener;
    Animator _animator;
    Rigidbody _rb;

    NavMeshAgent agent;


    GameObject _player;
    GameObject healthBar;
    Image img;
    float distance;
    float currentYRotation;


    float movespeed;
    Vector3 lastpos;

    List<Vector3> points = new List<Vector3>();
    Transform trans;

    void Start()
    {
        trans = transform;
        //_audioListener = gameObject.AddComponent<AudioListener>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Idle());
        health = 100;
        GameObject can = FindObjectOfType<Canvas>().gameObject;
        healthBar = Instantiate(HealthBarPrefab, can.transform);
        img = healthBar.transform.GetChild(1).GetComponent<Image>();
        img.fillAmount = 1;
        //healthBar.SetActive(false);
        float angle = 160 / 15;
        float step = -80;
        for(int i = 0; i < 15; i++)
        {
            Vector3 point = Quaternion.AngleAxis(step, trans.up) * trans.forward;
            points.Add(point);
            step += angle;
        }

    }

    void Update()
    {
        //_animator.SetFloat("speed");
        if (healthBar.activeSelf == true)
        {
            Vector3 newpos = Camera.main.WorldToScreenPoint(head.transform.position);
            newpos.y += 20;
            healthBar.transform.position = newpos;
        }
        RaycastCheck();
    }



    void RaycastCheck()
    {
        Debug.Log("Ray");
        RaycastHit hit;
        Vector3 pos = trans.position;
        pos.y = 1.5f;
        for(int i = 0; i < 15; i++)
        {
            Debug.DrawRay(pos, points[i]* 6, Color.red);
            if(Physics.Raycast(pos, points[i], out hit, 6,mask))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.Log("Plyer");
                    Debug.DrawRay(pos, points[i] * 6, Color.green);
                    PlayerDetected(hit.transform.gameObject);
                    return;
                }
            }
        }
        PlayerLost();

    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");
        //if(other.tag == "Player")
        //{
        //    PlayerDetected(other.gameObject);
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        //if(other.tag == "Player")
        //{
        //    PlayerLost();
        //}
    }

    void PlayerDetected(GameObject obj)
    {
        _player = obj;
        StopAllCoroutines();
        StartCoroutine(MoveToPlayer());
    }
    void PlayerLost()
    {
        _player = null;
        StopAllCoroutines();
        RandomState();
    }

    IEnumerator MoveToPlayer()
    {
        Debug.Log("Move to player");
        distance = Vector3.Distance(transform.position, _player.transform.position);
        
        while (distance > 1)
        {
            if (_player != null)
            {
                distance = Vector3.Distance(transform.position, _player.transform.position);
                agent.SetDestination(_player.transform.position);

                //Vector3 _velocity;
                //_velocity = _rb.velocity;
                //_velocity = Vector3.ProjectOnPlane(_velocity, transform.up);
                //float _magnitudeThreshold = 0.001f;
                //if (_velocity.magnitude < _magnitudeThreshold)
                //_velocity.Normalize();
                //Vector3 _currentForward = transform.forward;
                //float _angleDifference;
                //float _angle = Vector3.Angle(_currentForward, _velocity);
                //float _sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(_currentForward, _velocity)));
                //float _signedAngle = _angle * _sign;
                //_angleDifference = _signedAngle;
                //float _factor = Mathf.InverseLerp(0f, 90, Mathf.Abs(_angleDifference));
                //float _step = Mathf.Sign(_angleDifference) * _factor * Time.deltaTime * 1000;
                //if (_angleDifference < 0f && _step < _angleDifference)
                //    _step = _angleDifference;
                //else if (_angleDifference > 0f && _step > _angleDifference)
                //    _step = _angleDifference;
                //currentYRotation += _step;
                //if (currentYRotation > 360f)
                //    currentYRotation -= 360f;
                //if (currentYRotation < -360f)
                //    currentYRotation += 360f;
                //transform.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
            }
            yield return new WaitForFixedUpdate();
        }
        if (_player != null && distance < 1.5f)
        {
            agent.isStopped = true;
            Attack();
        }
        else
        RandomState();
        
    }
    IEnumerator MoveToRandomPosition()
    {
        Debug.Log("move to random position");
        float distanceToPoint = 10;
        while(distanceToPoint > 1)
        {

            Vector3 _velocity;
            _velocity = _rb.velocity;
            _velocity = Vector3.ProjectOnPlane(_velocity, transform.up);
            float _magnitudeThreshold = 0.001f;
            if (_velocity.magnitude < _magnitudeThreshold)
                _velocity.Normalize();
            Vector3 _currentForward = transform.forward;
            float _angleDifference;
            float _angle = Vector3.Angle(_currentForward, _velocity);
            float _sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(_currentForward, _velocity)));
            float _signedAngle = _angle * _sign;
            _angleDifference = _signedAngle;
            float _factor = Mathf.InverseLerp(0f, 90, Mathf.Abs(_angleDifference));
            float _step = Mathf.Sign(_angleDifference) * _factor * Time.deltaTime * 1000;
            if (_angleDifference < 0f && _step < _angleDifference)
                _step = _angleDifference;
            else if (_angleDifference > 0f && _step > _angleDifference)
                _step = _angleDifference;
            currentYRotation += _step;
            if (currentYRotation > 360f)
                currentYRotation -= 360f;
            if (currentYRotation < -360f)
                currentYRotation += 360f;
            transform.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
            yield return new WaitForEndOfFrame();
        }
        RandomState();
    }
    IEnumerator Idle()
    {
        Debug.Log("idle");
        float time = Random.Range(1, 5);
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        RandomState();

    }
    void Attack()
    {
        Debug.Log("attack");
        _animator.Play("Attack1",0);
    }
    public void AttackEnd()
    {
        Debug.Log("attack end");
        if (_player != null)
        {
            distance = Vector3.Distance(transform.position, _player.transform.position);
            if(distance > 1)
            {
                StopAllCoroutines();
                StartCoroutine(MoveToPlayer());
            }
            else
            {
                StopAllCoroutines();
                Attack();
            }
        }
        else
        {
            RandomState();
        }
    }

    void RandomState()
    {
        StopAllCoroutines();
        Debug.Log("random state");
        int i = Random.Range(0, 2);
        switch (i)
        {
            case 0:
                {

                    StartCoroutine(Idle());
                }break;
            case 1:
                {

                    StartCoroutine(MoveToRandomPosition());
                }break;
        }
    }








    public void MakeDamage(float damage)
    {
        healthBar.SetActive(true);
        if(damage < health)
        {
            health -= damage;
            img.fillAmount = health / 100;
        }
        else
        {
            Destroy(healthBar);
            Destroy(gameObject);
        }
    }
}
