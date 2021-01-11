using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomManag : MonoBehaviour
{

    public GameObject CreateEmptyGameObject()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(obj.GetComponent<MeshFilter>());
        DestroyImmediate(obj.GetComponent<MeshRenderer>());
        DestroyImmediate(obj.GetComponent<BoxCollider>());

        return obj;
    }
    public GameObject CreateEmptyGameObject(string name)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(obj.GetComponent<MeshFilter>());
        DestroyImmediate(obj.GetComponent<MeshRenderer>());
        DestroyImmediate(obj.GetComponent<BoxCollider>());
        obj.name = "Pivot";

        return obj;
    }
    public GameObject CreateEmptyGameObject(string name, string tag)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(obj.GetComponent<MeshFilter>());
        DestroyImmediate(obj.GetComponent<MeshRenderer>());
        DestroyImmediate(obj.GetComponent<BoxCollider>());
        obj.name = "Pivot";
        obj.tag = tag;

        return obj;
    }
    public GameObject CreateEmptyGameObject(string name, Transform parrent)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.SetParent(gameObject.transform);
        DestroyImmediate(obj.GetComponent<MeshFilter>());
        DestroyImmediate(obj.GetComponent<MeshRenderer>());
        DestroyImmediate(obj.GetComponent<BoxCollider>());
        obj.name = "Pivot";

        return obj;
    }
    public GameObject CreateEmptyGameObject(string name, string tag, Transform parrent)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.SetParent(gameObject.transform);
        DestroyImmediate(obj.GetComponent<MeshFilter>());
        DestroyImmediate(obj.GetComponent<MeshRenderer>());
        DestroyImmediate(obj.GetComponent<BoxCollider>());
        obj.name = "Pivot";
        obj.tag = tag;

        return obj;
    }
}
