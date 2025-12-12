using UnityEngine;
using System.Collections.Generic;
public class Building : MonoBehaviour
{
    [SerializeField] GameObject objectToPlace;
    [SerializeField] float gridSize = 1f;
    private GameObject proyectionObject;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    [SerializeField] float timer;
    private void Update()
    {

        if (exist)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }
    public void CreateProyectionObject()
    {
        proyectionObject = Instantiate(objectToPlace);
        proyectionObject.GetComponent<Collider>().enabled = false;

        Renderer[] renderers = proyectionObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = 0.5f;
            mat.color = color;

            mat.SetFloat("_Mode", 2);
            mat.SetInt("_ScrBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
    }
    public void DestroProyectionObject()
    {
        Destroy(proyectionObject);
    }
    bool exist = false;
    public void UpdateProyectionPosition(BuildingCharacter b)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;

            Vector3 snappedPosition = new Vector3(
                Mathf.Round(point.x / gridSize) * gridSize,
                Mathf.Round(point.y / gridSize) * gridSize,
                Mathf.Round(point.z / gridSize) * gridSize);
            proyectionObject.transform.position = snappedPosition;
            if (occupiedPositions.Contains(snappedPosition))
            {
                SetProyectionColor(Color.red);
                if (Input.GetMouseButton(0) && hit.collider.gameObject.CompareTag("NewFloor") && timer >= 2)
                {
                    Destroy(hit.collider.gameObject);
                    occupiedPositions.Remove(snappedPosition);
                    b.buildsCount += 1;
                    SetProyectionColor(new Color(1f, 1f, 1f, 0.5f));
                    if (b.buildsCount >= 6)
                    {
                        exist = false;
                    }
                }
            }
                
            else
            {
                SetProyectionColor(new Color(1f, 1f, 1f, 0.5f));

                if (Input.GetMouseButton(0) && b.buildsCount > 0)
                {
                    PlaceObject();
                    b.buildsCount -= 1;
                }
            }

        }
    }
    void SetProyectionColor(Color color)
    {
        Renderer[] renderers = proyectionObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            mat.color = color;
        }
    }
    void PlaceObject()
    {
        Vector3 placementPosition = proyectionObject.transform.position;
        if (!occupiedPositions.Contains(placementPosition))
        {
            Instantiate(objectToPlace, placementPosition, Quaternion.identity);
            occupiedPositions.Add(placementPosition);
            timer = 0;
            exist = true;
        }
    }
}
