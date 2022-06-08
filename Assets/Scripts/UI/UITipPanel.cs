using System;
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

        private void Awake()
        {
            tipText.text = "";
            offset = new Vector3(tipBgRect.sizeDelta.x / 2, tipBgRect.sizeDelta.y, 0);
        }

        public void Show(string tip, Vector3 pos)
        {
            gameObject.SetActive(true);
            tipText.text = tip;
            transform.position = pos + offset;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
