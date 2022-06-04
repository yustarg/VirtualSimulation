using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BCIT
{
    public class EditorTools : Editor
    {
        [MenuItem("Tools/Add MeshCollider to All Children")]
        public static void AddMeshColliders()
        {
            var curGo = Selection.activeObject as GameObject;
            if (curGo == null)
            {
                Debug.LogError("AddBoxColliders Cur GameObject NULL");
                return;
            }

            Debug.Log($"Adding BoxColliders to {curGo.name}");

            var q = new Queue<GameObject>();
            q.Enqueue(curGo);

            while (q.Count > 0)
            {
                var size = q.Count;
                for (var i = 0; i < size; i++)
                {
                    var go = q.Dequeue();
                    if (go.GetComponent<Renderer>() != null)
                    {
                        if (go.GetComponent<MeshCollider>() == null)
                        {
                           go.AddComponent<MeshCollider>();
                        }

                        Debug.Log($"BoxCollider added to {go.name}");
                    }
                    for (var j = 0; j < go.transform.childCount; j++)
                    {
                        q.Enqueue(go.transform.GetChild(j).gameObject);
                    }
                }
            }

            PrefabUtility.ApplyPrefabInstance(curGo, InteractionMode.AutomatedAction);
        }
    
    }
}
