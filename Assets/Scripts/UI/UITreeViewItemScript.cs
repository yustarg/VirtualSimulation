using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BCIT
{
    public class UITreeViewItemScript : MonoBehaviour
    {
        public Button mExpandBtn;
        public Image mSelectImg;
        public Button mClickBtn;
        public Text mLabelText;
    
        void Start()
        {
            mExpandBtn.onClick.AddListener(OnExpandBtnClicked);
            mClickBtn.onClick.AddListener(OnItemClicked);
        }
    
        public void Init()
        {
            SetExpandBtnVisible(false);
            SetExpandStatus(true);
            IsSelected = false;
        }
    
        void OnExpandBtnClicked()
        {
            UITreeViewItem item = GetComponent<UITreeViewItem>();
            item.DoExpandOrCollapse();
        }
    
    
        public void SetItemInfo(string labelTxt)
        {
            Init();
            mLabelText.text = labelTxt;
        }
    
        void OnItemClicked()
        {
            UITreeViewItem item = GetComponent<UITreeViewItem>();
            item.RaiseCustomEvent(CustomEvent.ItemClicked, null);
            Debug.Log($"TreeViewItem {name} Clicked ");
        }
    
        public void SetExpandBtnVisible(bool visible)
        {
            if (visible)
            {
                mExpandBtn.gameObject.SetActive(true);
            }
            else
            {
                mExpandBtn.gameObject.SetActive(false);
            }
        }
    
        public bool IsSelected
        {
            get
            {
                return mSelectImg.gameObject.activeSelf;
            }
            set
            {
                mSelectImg.gameObject.SetActive(value);
            }
        }
        public void SetExpandStatus(bool expand)
        {
            if (expand)
            {
                mExpandBtn.transform.localEulerAngles = new Vector3(0, 0, -90);
            }
            else
            {
                mExpandBtn.transform.localEulerAngles = new Vector3(0, 0, 0);
    
            }
        }
    }

}
