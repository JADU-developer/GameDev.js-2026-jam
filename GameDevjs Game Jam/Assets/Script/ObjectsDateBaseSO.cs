using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Objects DataBase" , menuName = "Scriptable Objects/Object DataBase" , order = 0)]
public class ObjectsDateBaseSO : ScriptableObject
{
    public List<ObjectsDate> objectsDate;
}

[Serializable]
public class ObjectsDate 
{
    [field: SerializeField] public string Name {get; private set;}

    [field: SerializeField] public int ID {get; private set;}

    [field: SerializeField] public Vector2Int Size {get; private set;} = Vector2Int.one;

    [field: SerializeField] public GameObject prefab {get; private set;}

}