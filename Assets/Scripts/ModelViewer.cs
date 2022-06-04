using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCIT
{
    public class ModelViewer : MonoBehaviour
    {
        private readonly float rotateSpeeed = 5f;
        private readonly float moveSpeeed = 5f;
        
        
        // Start is called before the first frame update
        void Start()
        {
            InputManager.Instance.OnRightButtonDown += Rotate;
            InputManager.Instance.OnMiddleButtonDown += Move;
            InputManager.Instance.OnLeftButtonDown += MoveSinglePart;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnRightButtonDown -= Rotate;
            InputManager.Instance.OnMiddleButtonDown -= Move;
            InputManager.Instance.OnLeftButtonDown -= MoveSinglePart;
        }

        private void Move(float x, float y)
        {
            transform.Translate(x, y, 0, Space.World);
        }
        
        private void Rotate(float x, float y)
        {
            transform.Rotate(new Vector3(y, x, 0f) * rotateSpeeed);
        }

        private void MoveSinglePart(float x, float y)
        {
            
        }
    }
}
