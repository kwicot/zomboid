using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TakeItemScript : MonoBehaviour
{
    [SerializeField]
    ItemsDateBase dateBase;
    GameObject Prefab;
    public int id;
    public int count;
    void Start()
    {
        Setup();
    }

    [Button]
    void Setup()
    {
        //var _meshFilter = gameObject.GetComponent<MeshFilter>();
        //if (_meshFilter == null) _meshFilter = gameObject.AddComponent<MeshFilter>();
        //var _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        //if (_meshRenderer == null) _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        //var _boxCollider = gameObject.GetComponent<BoxCollider>();
        //if (_boxCollider != null) DestroyImmediate(_boxCollider);
        //var _meshCollider = gameObject.GetComponent<MeshCollider>();
        //if (_meshCollider == null) _meshCollider = gameObject.AddComponent<MeshCollider>();
        //var _rb = GetComponent<Rigidbody>();
        //if (_rb == null) _rb = gameObject.AddComponent<Rigidbody>();

        //GameObject obj = dateBase.GetPrefab(id);
        //_meshFilter.mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        //_meshRenderer.material = obj.GetComponent<MeshRenderer>().sharedMaterial;
        //_meshCollider.sharedMesh = _meshFilter.sharedMesh;
        //_meshCollider.convex = true;
        //_rb.useGravity = true;
        //_rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //_rb.interpolation = RigidbodyInterpolation.Interpolate;

        gameObject.tag = "TakeItem";
        gameObject.name = "Item spawner";
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

}
