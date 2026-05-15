using Unity.Cinemachine;
using UnityEngine;

public class DigMod : MonoBehaviour {
    private LayerMask diggableMask;
    private CinemachineCamera holderHead;
    private bool isHeld = false;

    [SerializeField] float digRange = 3f;

    void Start() {
        diggableMask = LayerMask.GetMask("Diggable");

        EventManager.specialAction += TryDig;
    }

    void TryDig() {
        if (!CheckIfHeld()) return;
        Ray ray = new Ray(transform.position, holderHead.transform.forward);

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * digRange,Color.green, 2f);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, digRange, diggableMask)) {
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            DigAt(terrain);
        }
    }

    bool CheckIfHeld() {
        holderHead = GetComponent<ItemGrabbee>().GetHolderHead();
        return holderHead != null;
    }

    void DigAt(Terrain terrain) {
        TerrainData tData = terrain.terrainData;
        Debug.Log(tData);
    }

    void OnDestroy() {
        EventManager.specialAction -= TryDig;
    }
}
