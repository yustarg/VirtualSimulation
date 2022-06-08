using System.Collections.Generic;
using UnityEngine;

namespace BCIT
{
    public enum ViewState
    {
        Normal,
        XRay,
        Transparent
    }

    public class ModelView : MonoBehaviour
    {
        [SerializeField] private Material normalMat;
        [SerializeField] private Material xRayMat;
        [SerializeField] private Material transparentMat;
        private GameObject curSelectGameObject;
        private GameObject prevHoveredGameObject;
        
        private Dictionary<string, Transform> nodeMap = new Dictionary<string, Transform>();
        private ViewState state = ViewState.Normal;
        
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
            prevHoveredGameObject = null;
            
            if (curSelectGameObject != null && curSelectGameObject == go) return;
            
            if (curSelectGameObject != null)
            {
                switch (state)
                {
                    case ViewState.Normal:
                        curSelectGameObject.GetComponent<Renderer>().material = normalMat;
                        curSelectGameObject.GetComponent<Renderer>().material.color = Color.white;
                        break;
                    case ViewState.XRay:
                        curSelectGameObject.GetComponent<Renderer>().material = xRayMat;
                        break;
                    case ViewState.Transparent:
                        curSelectGameObject.GetComponent<Renderer>().material = transparentMat;
                        break;
                }
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
                if (child.gameObject.GetComponent<Renderer>() != null)
                {
                    OnSelect(child.gameObject);
                }
            }
        }

        public void ChangeState(ViewState state)
        {
            this.state = state;
            switch (state)
            {
                case ViewState.Normal:
                    SwitchMaterial(normalMat);
                    break;
                case ViewState.XRay:
                    SwitchMaterial(xRayMat);
                    break;
                case ViewState.Transparent:
                    SwitchMaterial(transparentMat);
                    break;
            }
        }

        private void SwitchMaterial(Material mat)
        {
            foreach (var kv in nodeMap)
            {
                if (curSelectGameObject != null && kv.Value == curSelectGameObject.transform) continue;

                var renderer = kv.Value.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = mat;
                }
            }
        }
    }
}
