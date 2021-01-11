using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    CustomManag custom;



    public float playerWalkSpeed;
    public float playerSprintSpeed;
    public float airControll;
    public float jumpStrange;

    public bool rotateToCursor = true;

    [SerializeField]
    InputManager _input;
    [SerializeField]
    PlayerInventory _inventory;
    [SerializeField]
    PlayerCameraController _camera;
    Animator _animator;
    [SerializeField]
    GameObject _model;


    Vector3 _moveDirect;
    Vector3 _lastCorrectDirect;
    Vector3 _velocity;
    Vector3 _rotationDirect;


    Rigidbody _rb;
    enum MoveStates
    {
        Walk,
        JumpStart,
        Failing
    };
    public enum MoveTypes
    {
        FreeHand,
        TwoHandWeapon,
        OneHandWeapon
    };
    public enum AimTypes
    {
        Mouse,
        Auto
    };
    public AimTypes aimType = AimTypes.Auto;
    public MoveTypes _moveType = MoveTypes.FreeHand;
    [SerializeField]
    MoveStates _state;


    float _vertical;
    float _horizontal;
    float _moveAmount;


    float _targetPlayerSpeed;

    float currentYRotation;


    float _jumpReload;


    bool isGround;


    List<GameObject> enemysinRadius = new List<GameObject>();
    void Start()
    {
        Reload();
        
    }

    void FixedUpdate()
    {
        MoveUpdate();
        AnimationUpdate();
        RotationUpdate();
    }
    void MoveUpdate()
    {
        IsGround();
        ReloadState();
        GravityForce();

        _vertical = _input.GetVertical();
        _horizontal = _input.GetHorizontal();
        _moveAmount = Mathf.Clamp01(Mathf.Abs(_vertical) + Mathf.Abs(_horizontal));
        if(_state != MoveStates.JumpStart && _state != MoveStates.Failing)
        {


            _moveDirect = new Vector3(_horizontal, 0, _vertical).normalized * _targetPlayerSpeed;
            _moveDirect.y = _rb.velocity.y;
            _rb.velocity = _moveDirect;
            
        }
        else if(_state == MoveStates.Failing || _state == MoveStates.JumpStart)
        {
            _moveDirect = new Vector3(_horizontal, 0, _vertical).normalized * _targetPlayerSpeed;
            _moveDirect.y = _rb.velocity.y;
            _rb.velocity = _moveDirect;

        }

    }
    void reloadList()
    {
        if (enemysinRadius.Count > 0)
        {
            float dis;
            float mindis = 100;
            GameObject closets = enemysinRadius[0];
            if (enemysinRadius.Count > 1)
            {
                for (int i = 0; i < enemysinRadius.Count; i++)
                {
                    dis = Vector3.Distance(transform.position, enemysinRadius[i].transform.position);
                    if(mindis > dis)
                    {
                        Vector3 dir = transform.position - enemysinRadius[i].transform.position;
                        RaycastHit hit;
                        if(Physics.Raycast(transform.position, dir, out hit))
                        {
                            if (hit.transform.tag == "Enemy")
                            {
                                GameObject onj = enemysinRadius[0];
                                enemysinRadius[0] = enemysinRadius[i];
                                enemysinRadius[i] = onj;
                                mindis = dis;
                            }
                        }
                    }
                }
            }
        }
    }
    void RotationUpdate()
    {
        if(enemysinRadius.Count > 0)
        reloadList();
        if (rotateToCursor & aimType == AimTypes.Mouse)
        {
            Vector3 target = _input.GetCursorPosition();
            Quaternion targetRotation = Quaternion.LookRotation(target - _model.transform.position);
            targetRotation.z = 0;
            targetRotation.x = 0;
            _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, targetRotation, 100 * Time.deltaTime);

            Vector3 dir = new Vector3(Vector3.Dot(_rb.velocity, _model.transform.right), 0, Vector3.Dot(_rb.velocity, _model.transform.forward));
            dir.Normalize();
            float chHorizontal = dir.x;
            float chVertical = dir.z;
            _animator.SetFloat("vertical", chVertical);
            _animator.SetFloat("horizontal", chHorizontal);
        }
        else if (aimType == AimTypes.Auto && enemysinRadius.Count > 0 && Vector3.Distance(transform.position, enemysinRadius[0].transform.position) < 10)
        {
            
            if (enemysinRadius.Count > 0)
            {

                Vector3 target = enemysinRadius[0].transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(target - _model.transform.position);
                targetRotation.z = 0;
                targetRotation.x = 0;
                _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, targetRotation, 100 * Time.deltaTime);

                Vector3 dir = new Vector3(Vector3.Dot(_rb.velocity, _model.transform.right), 0, Vector3.Dot(_rb.velocity, _model.transform.forward));
                dir.Normalize();
                float chHorizontal = dir.x;
                float chVertical = dir.z;
                _animator.SetFloat("vertical", chVertical);
                _animator.SetFloat("horizontal", chHorizontal);
            }
        }
        else
        {
            Vector3 _velocity;
            _velocity = _rb.velocity;
            _velocity = Vector3.ProjectOnPlane(_velocity, transform.up);
            float _magnitudeThreshold = 0.001f;
            if (_velocity.magnitude < _magnitudeThreshold)
                return;
            _velocity.Normalize();
            Vector3 _currentForward = _model.transform.forward;
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
            _model.transform.localRotation = Quaternion.Euler(0f, currentYRotation, 0f);
            _animator.SetFloat("vertical", _vertical);
            _animator.SetFloat("horizontal", _horizontal);
        }
    }
    float CheckDistanceToGround()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        origin.y += 0.01f;
        if (Physics.Raycast(origin, -transform.up, out hit))
        {
            float distance = Vector3.Distance(origin, hit.point);
            return distance;
        }
        else return 1;
    }
    void IsGround()
    {
        float dis = CheckDistanceToGround();
        _animator.SetFloat("distance", dis);
        if (dis > 0.1f)
            isGround = false;
        else
            isGround = true;
    }
    void GravityForce()
    {
        Vector3 vel = _rb.velocity;
        if (isGround)
            vel.y = -0.1f;
        else
            vel.y -= 0.5f;
            _rb.velocity = vel;

    }
    void ReloadState()
    {

        if (isGround) _state = MoveStates.Walk;
        else if (_rb.velocity.y > 0.1f) _state = MoveStates.JumpStart;
        else _state = MoveStates.Failing;
    }
    public void JumpKeyPressed()
    {
        ReloadState();
        if(_state != MoveStates.JumpStart && _state != MoveStates.Failing)
        {
            _animator.Play("Jump start", 0, 0.5f);
            _state = MoveStates.JumpStart;
            _rb.AddForce(transform.up * jumpStrange, ForceMode.Impulse);
        }
    }
    public void SprintKeyPressed()
    {
        _targetPlayerSpeed = playerSprintSpeed;
    }
    public void SprintKeyRelised()
    {
        _targetPlayerSpeed = playerWalkSpeed;
    }
    public void MovementTypeChanged()
    {

    }
    void AnimationUpdate()
    {
        _animator.SetFloat("vertical", _vertical);
        _animator.SetFloat("horizontal", _horizontal);
        _animator.SetFloat("moveAmount", _moveAmount);
    }
    [Button]
    public void Reload()
    {
        AddStartComponents();
        Setup();
    }
    public void ChangeMoveType(MoveTypes type)
    {
            _moveType = type;
            switch (type)
            {
                case MoveTypes.FreeHand:
                    {
                        Debug.Log("Change type to freeHand");
                        _animator.Play("Non-equipt locomotion", 0);
                        rotateToCursor = false;
                        _targetPlayerSpeed = playerSprintSpeed;
                    _animator.SetBool("weapon", false);
                    }break;
                case MoveTypes.TwoHandWeapon:
                    {
                        Debug.Log("Change type to twoHandWeapon");
                        _animator.Play("Rifle locomotion", 0);
                        rotateToCursor = true;
                        _targetPlayerSpeed = playerWalkSpeed;
                    _animator.SetBool("weapon", true);

                }
                break;
                case MoveTypes.OneHandWeapon:
                    {
                        rotateToCursor = false;

                    }
                    break;
            }
    }

    void AddStartComponents()
    {
        gameObject.AddComponent<CustomManag>();
        custom = GetComponent<CustomManag>();

        //GameObject _pivot = custom.CreateEmptyGameObject("Pivot");
        GameObject _pivot = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _pivot.transform.SetParent(gameObject.transform);
        DestroyImmediate(_pivot.GetComponent<MeshFilter>());
        DestroyImmediate(_pivot.GetComponent<MeshRenderer>());
        DestroyImmediate(_pivot.GetComponent<BoxCollider>());
        _pivot.name = "Pivot";


        _pivot.transform.localPosition = Vector3.zero;
        _pivot.transform.rotation = Quaternion.Euler(75, 0, 0);
        Camera cam = Camera.main;
        if (cam != null)
        {
            _camera.transform.SetParent(_pivot.transform);
            _camera.transform.localPosition = new Vector3(0, 0, -9);
            _camera.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        if (GetComponent<Animator>() == null) gameObject.AddComponent<Animator>();
        gameObject.AddComponent<Rigidbody>();
        if(GetComponent<PlayerInventory>() == null) gameObject.AddComponent<PlayerInventory>();
        CapsuleCollider col = gameObject.AddComponent<CapsuleCollider>();
        //GameObject im = custom.CreateEmptyGameObject(name = "Input manager");


        col.center = new Vector3(0, 0.9f, 0);
        col.radius = 0.3f;
        col.height = 1.8f;
        PhysicMaterial mat = new PhysicMaterial();
        mat.dynamicFriction = 0;
        mat.staticFriction = 0;
        mat.bounciness = 0;
        mat.frictionCombine = PhysicMaterialCombine.Minimum;
        mat.bounceCombine = PhysicMaterialCombine.Minimum;
        col.material = mat;

        InputManager im = FindObjectOfType<InputManager>();
        _input = im;
    }
    void Setup()
    {
        _state = MoveStates.Failing;

        _rb = GetComponent<Rigidbody>();
       // _input = GetComponent<InputManager>();
        _inventory = GetComponent<PlayerInventory>();
        _camera = FindObjectOfType<PlayerCameraController>();
        _animator = GetComponent<Animator>();
        

        _rb.freezeRotation = true;
        _rb.useGravity = false;


        _targetPlayerSpeed = playerWalkSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            enemysinRadius.Add(other.gameObject);
        }
    }
    private void OnValidate()
    {
        _targetPlayerSpeed = playerWalkSpeed;
    }
}
