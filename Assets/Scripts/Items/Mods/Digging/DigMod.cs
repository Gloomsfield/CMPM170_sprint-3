using Unity.Cinemachine;
using UnityEngine;

public class DigMod : MonoBehaviour {
    private LayerMask diggableMask;
    private CinemachineCamera holderHead;

    [SerializeField] float digRange = 3f;

    void Start() {
        diggableMask = LayerMask.GetMask("Diggable");
        // Get the head of the whoever is grabbing this obj
        ItemGrabbee holder = GetComponent<ItemGrabbee>();
        holderHead = holder.gameObject.GetComponent<PlayerGrabber>().GetHead();
    }

    void TryDig() {
        Ray ray = new Ray(transform.position, holderHead.transform.forward);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * digRange,Color.green, 2f);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, digRange, diggableMask)) {
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            DigAt(terrain);
        }
    }

    void DigAt(Terrain terrain) {
        TerrainData tData = terrain.terrainData;
        Debug.Log(tData);
    }
}
