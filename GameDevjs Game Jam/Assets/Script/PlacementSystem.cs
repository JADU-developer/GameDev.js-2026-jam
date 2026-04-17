using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject MouseIndicator;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDateBaseSO DataBase;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject GridVisualization;


    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = DataBase.objectsDate.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        GridVisualization.SetActive(true);
        MouseIndicator.SetActive(true);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePos = inputManager.GetSelectedMapPosition();

        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        GameObject placeObject = Instantiate(DataBase.objectsDate[selectedObjectIndex].prefab);
        placeObject.transform.position = gridPosition;

    }

    public void StopPlacement()
    {
        selectedObjectIndex = -1;
        GridVisualization.SetActive(false);
        MouseIndicator.SetActive(false);

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }


    void Update()
    {

        if (selectedObjectIndex < 0)
        {
            
        }
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        MouseIndicator.transform.position = grid.GetCellCenterWorld(grid.WorldToCell(mousePos));

        Debug.Log(grid.WorldToCell(mousePos));
    }
}
