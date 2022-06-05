using System.Collections;
using System.Collections.Generic;
using BCIT;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BCIT
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Button btnReset;
        [SerializeField] private Button btnModel;
        [SerializeField] private Button btnXRay;
        [SerializeField] private Button btnTransparent;
        [SerializeField] private UITipPanel panelTip;

        private Camera mainCam;
        private Canvas canvas;
        
        // Start is called before the first frame update
        void Start()
        {
            btnReset.onClick.AddListener(OnBtnResetClick);
            btnModel.onClick.AddListener(OnBtnModelClick);
            btnXRay.onClick.AddListener(OnBtnXRayClick);
            btnTransparent.onClick.AddListener(OnBtnTransparentClick);
            
            mainCam = Camera.main;
            canvas = GetComponent<Canvas>();
        }
        
        public void ShowTip(GameObject go)
        {
            if (go != null)
            {
                var pos = go.transform.TransformPoint(go.GetComponent<MeshFilter>().mesh.bounds.max);
                panelTip.Show(go.name, canvas.WorldToCanvasPosition(new Vector3(pos.x, pos.y / 2, pos.z)));
            }
            else
            {
                panelTip.Hide();
            }
        }

        private void OnBtnResetClick()
        {
            // reload the main scene
            SceneManager.LoadScene(0);
        }

        private void OnBtnModelClick()
        {

        }

        private void OnBtnXRayClick()
        {

        }

        private void OnBtnTransparentClick()
        {

        }
    }
}
