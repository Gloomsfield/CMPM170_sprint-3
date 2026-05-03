#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

// Copied and modified from https://github.com/INedelcu/SimpleVolumeEditor/blob/main/Assets/SimpleVolume/SimpleVolumeEditor.cs
namespace Items
{
    [CustomEditor(typeof(WeightedItemSpawnZoneEditor), true)]
    public class WeightedItemSpawnZoneEditor : UnityEditor.Editor
    {
        private BoxBoundsHandle m_BoundsHandle = new();
        
        private static Color kGizmoHandleColor = new(0xFF / 255f, 0xE5 / 255f, 0xAA / 255f, 0xFF / 255f);
        private static Color kGizmoBoxColor = new(0xFF / 255f, 0xE5 / 255f, 0x94 / 255f, 0x80 / 255f);

        public void OnEnable()
        {
            m_BoundsHandle.handleColor = kGizmoHandleColor;
            m_BoundsHandle.wireframeColor = Color.clear;
        }

        static Matrix4x4 GetLocalSpace(WeightedItemSpawnZone volume)
        {
            return Matrix4x4.TRS(volume.transform.position, Quaternion.identity, Vector3.one);
        }

        private bool ValidateAABB(ref Vector3 center, ref Vector3 size)
        {
            WeightedItemSpawnZone zone = (WeightedItemSpawnZone)target;

            Matrix4x4 localSpace = GetLocalSpace(zone);
            Vector3 localTransformPosition = localSpace.inverse.MultiplyPoint3x4(zone.transform.position);

            Bounds b = new Bounds(center, size);

            if (b.Contains(localTransformPosition))
                return false;

            b.Encapsulate(localTransformPosition);

            center = b.center;
            size = b.size;

            return true;
        }

        [DrawGizmo(GizmoType.Active)]
        static void RenderBoxGizmo(WeightedItemSpawnZone zone, GizmoType gizmoType)
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = kGizmoBoxColor;
            Gizmos.matrix = GetLocalSpace(zone);
            Gizmos.DrawCube(zone.GetCenterOffset(), -1f * zone.GetSize());
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = oldColor;
        }
    }
}
#endif
