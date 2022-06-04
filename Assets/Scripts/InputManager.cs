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

        public delegate void OnMouseDown(float x, float y);

        public event OnMouseDown OnLeftButtonDown;
        public event OnMouseDown OnMiddleButtonDown;
        public event OnMouseDown OnRightButtonDown;

        private void Awake()
        {
            _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                OnLeftButtonDown?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }

            if (Input.GetMouseButton(1))
            {
                OnRightButtonDown?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }

            if (Input.GetMouseButton(2))
            {
                OnMiddleButtonDown?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
        }
    }
}
