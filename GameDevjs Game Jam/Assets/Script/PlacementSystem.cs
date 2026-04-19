using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    // [SerializeField] private GameObject MouseIndicator;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDateBaseSO DataBase;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject GridVisualization;
    [SerializeField] private GameObject SpawnedObjectParent;
    [SerializeField] private Color MouseIndicatorNormalCOlor;
    [SerializeField] private Color MouseIndicatorCantPlaceColor = Color.red;
    private GridData Objects;

    // private SpriteRenderer previewRenderer;
    [SerializeField] private PreviewSystem previewSystem;

    private List<GameObject> placedGameObjects = new();

    private Vector3Int lastDetectedPosition = Vector3Int.zero;


    private void Start()
    {
        StopPlacement();
        Objects = new();

        // previewRenderer = MouseIndicator.GetComponent<SpriteRenderer>();
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
        // MouseIndicator.SetActive(true);
        previewSystem.StartShowingPlacementPreview(DataBase.objectsDate[selectedObjectIndex].prefab, DataBase.objectsDate[selectedObjectIndex].Size);

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

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (placementValidity == false)
        {
            return;
        }

        GameObject placeObject = Instantiate(DataBase.objectsDate[selectedObjectIndex].prefab, SpawnedObjectParent.transform);
        placeObject.transform.position = gridPosition;

        placedGameObjects.Add(placeObject);

        Objects.AddObjectAt(gridPosition, DataBase.objectsDate[selectedObjectIndex].Size, DataBase.objectsDate[selectedObjectIndex].ID, placedGameObjects.Count - 1);

        previewSystem.updatePosition(grid.CellToWorld(gridPosition), false);

    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {

        return Objects.CanPlaceObject(gridPosition, DataBase.objectsDate[selectedObjectIndex].Size);

    }

    public void StopPlacement()
    {
        selectedObjectIndex = -1;
        GridVisualization.SetActive(false);
        // MouseIndicator.SetActive(false);

        previewSystem.StopShowingPreview();

        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        lastDetectedPosition = Vector3Int.zero;
    }


    void Update()
    {

        if (selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        // MouseIndicator.transform.position = grid.WorldToCell(mousePos);
        Vector3Int CellPos = grid.WorldToCell(mousePos);





        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);


            previewSystem.updatePosition(grid.CellToWorld(CellPos), placementValidity);

            lastDetectedPosition = gridPosition;


            // if (placementValidity == true)
            // {
            //     // previewRenderer.color = MouseIndicatorNormalCOlor;
            //     previewSystem.applyFeedBack(true);
            // }
            // else
            // {
            //     previewRenderer.color = MouseIndicatorCantPlaceColor;
            // }

            Debug.Log(grid.WorldToCell(mousePos));
        }



    }
}
