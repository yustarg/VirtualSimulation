using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCIT
{
    public class ModelView : MonoBehaviour
    {
        [SerializeField] private Material normalMat;
        [SerializeField] private Material xRayMat;
        [SerializeField] private Material transparentMat;
        private GameObject curSelectGameObject;
        private GameObject prevHoveredGameObject;
        
        private Dictionary<string, Transform> nodeMap = new Dictionary<string, Transform>();

        void Awake()
        {
            var root = GetRootTransform();
            var allNodes = root.GetComponentsInChildren<Transform>();
            // each node should hava a unique name
            foreach (var t in allNodes)
            {
                nodeMap.Add(t.name, t);
            }
        }

        public Transform GetRootTransform()
        {
            return transform;
        }

        public Transform GetSelectedTransform()
        {
            return curSelectGameObject == null ? null : curSelectGameObject.transform;
        }

        public void OnHover(GameObject go)
        {
            if (prevHoveredGameObject != null)
            {
                prevHoveredGameObject.GetComponent<Renderer>().material.color = Color.white;
            }
            
            if (go != null && curSelectGameObject != null && curSelectGameObject == go) return;

            if (go != null)
            {
                go.GetComponent<Renderer>().material.color = Color.yellow;
                prevHoveredGameObject = go;
            }
        }

        public void OnSelect(GameObject go)
        {
            if (prevHoveredGameObject != null) prevHoveredGameObject = null;
            
            if (curSelectGameObject != null && curSelectGameObject == go) return;
            
            if (curSelectGameObject != null)
            {
                curSelectGameObject.GetComponent<Renderer>().material.color = Color.white;
                if (curSelectGameObject == go)
                {
                    curSelectGameObject = null;
                    return;
                }
            }

            if (go != null)
            {
                curSelectGameObject = go;
                go.GetComponent<Renderer>().material = normalMat;
                go.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        public void OnSelect(string name)
        {
            if(nodeMap.TryGetValue(name, out var child))
            {
                OnSelect(child.gameObject);
            }
        }

        public void ChangeToXRay()
        {
            foreach (var kv in nodeMap)
            {
                var renderer = kv.Value.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = xRayMat;
                }
            }
        }
        
        public void ChangeToTransparent()
        {
            foreach (var kv in nodeMap)
            {
                var renderer = kv.Value.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = transparentMat;
                }
            }
        }
    }
}
