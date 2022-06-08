using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BCIT
{
    public class InputManager : Singleton<InputManager>
    {
        private const string mouseAxisX = "Mouse X";
        private const string mouseAxisY = "Mouse Y";
        
        public delegate void OnMouseAction(float x, float y);

        public event OnMouseAction OnLeftButton;
        public event OnMouseAction OnLeftButtonUp;
        public event OnMouseAction OnMiddleButton;
        public event OnMouseAction OnMiddleButtonUp;
        public event OnMouseAction OnRightButton;
        public event OnMouseAction OnRightButtonUp;
        public event OnMouseAction OnLeftButtonClick;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                OnLeftButton?.Invoke(Input.GetAxis(mouseAxisX), Input.GetAxis(mouseAxisY));
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnLeftButtonUp?.Invoke(Input.GetAxis(mouseAxisX), Input.GetAxis(mouseAxisY));
            }

            if (Input.GetMouseButton(1))
            {
                OnRightButton?.Invoke(Input.GetAxis(mouseAxisX), Input.GetAxis(mouseAxisY));
            }
            
            if (Input.GetMouseButtonUp(1))
            {
                OnRightButtonUp?.Invoke(Input.GetAxis(mouseAxisX), Input.GetAxis(mouseAxisY));
            }
            
            if (Input.GetMouseButton(2))
            {
                OnMiddleButton?.Invoke(Input.GetAxis(mouseAxisX), Input.GetAxis(mouseAxisY));
            }
            
            if (Input.GetMouseButtonUp(2))
            {
                OnMiddleButtonUp?.Invoke(Input.GetAxis(mouseAxisX), Input.GetAxis(mouseAxisY));
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftButtonClick?.Invoke(Input.GetAxis(mouseAxisX), Input.GetAxis(mouseAxisY));
            }
        }

        private void OnDestroy()
        {
            OnLeftButton = null;
            OnRightButton = null;
            OnMiddleButton = null;
            OnLeftButtonUp = null;
            OnRightButtonUp = null;
            OnMiddleButtonUp = null;
            OnLeftButtonClick = null;
        }
    }
}
