using UnityEngine;

namespace BCIT
{
    public class Utils
    {
        public static Vector3 GetMeshCenter(GameObject go)
        {
            return go.transform.TransformPoint(go.GetComponent<MeshFilter>().mesh.bounds.center);
        }
    }
}