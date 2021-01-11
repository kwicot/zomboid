using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon propertie")]
public class WeaponProperties : ScriptableObject
{
    [InspectorName("Position on hand")]
    public Vector3 positionOnHand;
    [InspectorName("Rotation on hand")]
    public Vector3 rotationOnHand;
    [InspectorName("Position on back")]
    public Vector3 positionOnBack;
    [InspectorName("Rotation on back")]
    public Vector3 rotationOnBack;
    [InspectorName("Left hand position")]
    public Vector3 LeftHandIkPosition;
}
