using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCIT
{
    public class ModelViewer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            InputManager.Instance.OnRightButtonDown += Rotate;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Rotate(float x, float y)
        {
            Debug.Log(x + " " + y);
        }
    }
}
