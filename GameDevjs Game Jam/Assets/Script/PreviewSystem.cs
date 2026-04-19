using System;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewZOffset = - 0.06f; 

    [SerializeField] private GameObject CellIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;


    private Renderer CellIndicatorRenderer;


    void Start()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        CellIndicator.SetActive(false);

        CellIndicatorRenderer = CellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab , Vector2Int size)
    {
        previewObject = Instantiate(prefab);

        PreparePreview(previewObject);
        PrepareCursor(size);

        CellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            CellIndicator.transform.localScale = new Vector3(size.x , size.y , 1);
            // CellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }

            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        CellIndicator.SetActive(false);
        Destroy(previewObject);
    }


    public void updatePosition(Vector3 position , bool validity)
    {
        MovePreview(position);

        MoveCursor(position);

        applyFeedBack(validity);
    }

    private void applyFeedBack(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        if (c == Color.red)
        {
            c.a = 0.5f;
        }
        else c.a = 0.1f;
        CellIndicatorRenderer.material.color = c;
        previewMaterialInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        CellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x , position.y , position.z + previewZOffset);
    }
}
