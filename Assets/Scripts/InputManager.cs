using System;
using System.Collections.Generic;
using UnityEngine;

namespace BCIT
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;

        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("InputManager _instance null");
                }

                return _instance;
            }
        }

        public delegate void OnMouseAction(float x, float y);

        public event OnMouseAction OnLeftButton;
        public event OnMouseAction OnMiddleButton;
        public event OnMouseAction OnRightButton;
        public event OnMouseAction OnLeftButtonClick;

        private void Awake()
        {
            _instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                OnLeftButton?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }

            if (Input.GetMouseButton(1))
            {
                OnRightButton?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }

            if (Input.GetMouseButton(2))
            {
                OnMiddleButton?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftButtonClick?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
        }

        private void OnDestroy()
        {
            OnLeftButton = null;
            OnRightButton = null;
            OnMiddleButton = null;
            OnLeftButtonClick = null;
        }
    }
}
