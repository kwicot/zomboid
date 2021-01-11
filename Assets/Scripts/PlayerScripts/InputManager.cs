using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public LayerMask rayMask;
    
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode ReloadKey = KeyCode.R;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode InventoryKey = KeyCode.I;
    public KeyCode FirstWeapon = KeyCode.Alpha1;
    public KeyCode SecondWeapon = KeyCode.Alpha2;
    public KeyCode MeleWeapon = KeyCode.Alpha3;
    public KeyCode PunchKey = KeyCode.F;
    public KeyCode TakeKey = KeyCode.E;

    [SerializeField]
    PlayerController _player;
    [SerializeField]
    PlayerCameraController _camera;
    [SerializeField]
    PlayerInventory _inv;
    [SerializeField]
    Camera _cam;

    Transform _cursor;

    RaycastHit _hit;

    float mouseScroll;

    float _vertical;
    float _horizontal;
    void Start()
    {
        _cursor = CreateCursorPoint().transform;
    }

    void Update()
    {
        MoveCursor();

        mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll > 0) _camera.MoveForward();
        else if (mouseScroll < 0) _camera.MoveBackward();

        if (Input.GetKeyDown(JumpKey)) _player.JumpKeyPressed();

        if (Input.GetKey(SprintKey)) _player.SprintKeyPressed();
        else if (Input.GetKeyUp(SprintKey)) _player.SprintKeyRelised();

        //if (Input.GetMouseButtonDown(1)) _player.ChangeMoveType(PlayerController.MoveTypes.TwoHandWeapon);
        //else if (Input.GetMouseButtonUp(1)) _player.ChangeMoveType(PlayerController.MoveTypes.FreeHand);

        if (Input.GetMouseButton(0))
        {
            _inv.ShootKeyPressed();
        }
        if (Input.GetKeyDown(ReloadKey)) _inv.ReloadKeyPressed();

        if (Input.GetKeyDown(FirstWeapon))
        {
            Debug.Log("FirstWeapon key pressed");
            _inv.EquiptFirstWeapon();
        }
        if (Input.GetKeyDown(SecondWeapon)) _inv.EquiptSecondWeapon();

        if (Input.GetKeyDown(KeyCode.J)) Time.timeScale = 0.1f;
        if (Input.GetKeyDown(KeyCode.K)) Time.timeScale = 1;

        if (Input.GetKeyDown(TakeKey)) _inv.TakeKeyPressed();

        if (Input.GetKeyDown(InventoryKey)) _inv.InventoryKeyPressed();

        _vertical = Input.GetAxis("Vertical");
        _horizontal = Input.GetAxis("Horizontal");
    }


    public float GetVertical()
    {
        float v = _vertical;
        return v;
    }
    public float GetHorizontal()
    {
        float h = _horizontal;
        return h;
    }

    GameObject CreateCursorPoint()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //DestroyImmediate(obj.GetComponent<MeshFilter>());
        //DestroyImmediate(obj.GetComponent<MeshRenderer>());
        DestroyImmediate(obj.GetComponent<BoxCollider>());
        obj.name = "Cursor";
        obj.tag = "Cursor";
        obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        obj.GetComponent<MeshRenderer>().enabled = false;
        return obj;
    }

    void MoveCursor()
    {
        if (_cam == null) Debug.Log("NotCamera");
        if(Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition),out _hit,rayMask))
        {
            _cursor.position = _hit.point;
            _cursor.LookAt(_cam.transform);

            Vector3 newpos = _cursor.localPosition;
            newpos += _cursor.transform.forward * 1.5f;
            _cursor.position = newpos;
        }
    }
    public Transform GetCursorTransform()
    {
        return _cursor;
    }
    public Vector3 GetCursorPosition()
    {
        return _cursor.position;
    }
}
