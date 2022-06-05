using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCIT
{
    public class UITipPanel : MonoBehaviour
    {
        [SerializeField] private Text tipText;
        [SerializeField] private RectTransform tipBgRect;
        private Vector3 offset;
        
        // Start is called before the first frame update
        void Start()
        {
            tipText.text = "";
            offset = tipBgRect.sizeDelta / 2;
        }

        public void Show(string tip, Vector3 pos)
        {
            tipText.text = tip;
            transform.position = pos + offset;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
