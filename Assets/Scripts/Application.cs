using System;
using UnityEngine;

namespace BCIT
{
    public class Application : MonoBehaviour
    {
        public ModelView view = null;
        public ModelController controller = null;

        private void Awake()
        {
            controller.SetView(view);
        }

        private void Start()
        {
            UIManager.Instance.SetView(view);
        }
    }
}