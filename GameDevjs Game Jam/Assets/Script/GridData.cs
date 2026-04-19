using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> PlacedObject = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (PlacedObject.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contain this cell pos {pos}");
            }

            PlacedObject[pos] = data;
        }
        
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x,y,0));
            }
        }

        return returnVal;
    }

    public bool CanPlaceObject(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> PositionToOccupy = CalculatePositions(gridPosition ,  objectSize);

        foreach (var pos in PositionToOccupy)
        {
            if (PlacedObject.ContainsKey(pos))
            {
                return false;
            }
        }
            return true;
    }
}


public class PlacementData
{
    public List<Vector3Int> OccupiedPosition;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int ID, int PlacedObjectIndex)
    {
        this.OccupiedPosition = occupiedPositions;
        this.ID = ID;
        this.PlacedObjectIndex = PlacedObjectIndex;
    }
}

