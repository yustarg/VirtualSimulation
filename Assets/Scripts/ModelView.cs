using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCIT
{
    public class ModelView : MonoBehaviour
    {
        private GameObject curSelectGameObject;
        private GameObject prevHoveredGameObject;
        private Material prevHoveredMat;

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
                go.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
}
