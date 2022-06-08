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
        [SerializeField] private Button btnReset = null;
        [SerializeField] private Button btnModel = null;
        [SerializeField] private Button btnXRay = null;
        [SerializeField] private Button btnTransparent = null;
        [SerializeField] private UITipPanel panelTip = null;
        [SerializeField] private GameObject treeRoot = null;
        [SerializeField] private UITreeView treeView = null;

        private bool isShowTreeView = false;
        private Canvas canvas = null;
        private ModelView modelView;
        private int curSelectedItemId;
        
        // Start is called before the first frame update
        void Start()
        {
            btnReset.onClick.AddListener(OnBtnResetClick);
            btnModel.onClick.AddListener(OnBtnModelClick);
            btnXRay.onClick.AddListener(OnBtnXRayClick);
            btnTransparent.onClick.AddListener(OnBtnTransparentClick);
            
            canvas = GetComponent<Canvas>();
        }

        public void SetView(ModelView view)
        {
            modelView = view;
            InitializeTreeView();
        }

        private void InitializeTreeView()
        {
            treeView.OnTreeListAddOneItem = OnTreeListAddOneItem;
            treeView.OnItemExpand = OnItemExpandBegin;
            treeView.OnItemCollapse = OnItemCollapseBegin;
            treeView.OnItemCustomEvent = OnItemCustomEvent;
            treeView.InitView();
            var rootTrans = modelView.GetRootTransform();
            CreateTreeRecursively(treeView, rootTrans);
        }

        private void CreateTreeRecursively(UITreeList treeList, Transform root)
        {
            var rootItem = treeList.AppendItem();
            rootItem.name = root.name;
            rootItem.GetComponent<UITreeViewItemScript>().SetItemInfo(root.name);
            
            for (int i = 0; i < root.childCount; i++)
            {
                CreateTreeRecursively(rootItem.ChildTree, root.GetChild(i));
            }
        }

        private void OnTreeListAddOneItem(UITreeList treeList)
        {
            int count = treeList.ItemCount;
            UITreeViewItem parentTreeItem = treeList.ParentTreeItem;
            if (count > 0 && parentTreeItem != null)
            {
                UITreeViewItemScript st = parentTreeItem.GetComponent<UITreeViewItemScript>();
                st.SetExpandBtnVisible(true);
                st.SetExpandStatus(parentTreeItem.IsExpand);
            }
        }
        
        void OnItemExpandBegin(UITreeViewItem item)
        {
            UITreeViewItemScript st = item.GetComponent<UITreeViewItemScript>();
            st.SetExpandStatus(true);
        }

        void OnItemCollapseBegin(UITreeViewItem item)
        {
            UITreeViewItemScript st = item.GetComponent<UITreeViewItemScript>();
            st.SetExpandStatus(false);
        }
        
        void OnItemCustomEvent(UITreeViewItem item, CustomEvent customEvent, System.Object param)
        {
            if (customEvent == CustomEvent.ItemClicked)
            {
                UITreeViewItemScript st = item.GetComponent<UITreeViewItemScript>();
                if (curSelectedItemId > 0)
                {
                    if (item.ItemId == curSelectedItemId)
                    {
                        return;
                    }
                    UITreeViewItem curSelectedItem = treeView.GetTreeItemById(curSelectedItemId);
                    if (curSelectedItem != null)
                    {
                        curSelectedItem.GetComponent<UITreeViewItemScript>().IsSelected = false;
                    }
                    curSelectedItemId = 0;
                }
                st.IsSelected = true;
                curSelectedItemId = item.ItemId;
                
                modelView.OnSelect(item.name);
            }
        }

        public void ShowTip(GameObject go)
        {
            if (go != null)
            {
                //TODO refactor here
                var pos = go.transform.TransformPoint(go.GetComponent<MeshFilter>().mesh.bounds.max);
                panelTip.Show(go.name, canvas.WorldToCanvasPosition(new Vector3(pos.x, pos.y / 2, pos.z)));
            }
            else
            {
                panelTip.Hide();
            }
        }

        public void OnModelPartSelect(GameObject go)
        {
            var item = treeView.GetTreeItemByName(go.name);
            if (item != null)
            {
                OnItemCustomEvent(item, CustomEvent.ItemClicked, null);
            }
        }

        private void OnBtnResetClick()
        {
            // reload the main scene
            SceneManager.LoadScene(0);
        }

        private void OnBtnModelClick()
        {
            isShowTreeView = !isShowTreeView;
            ShowTreeView(isShowTreeView);
        }

        private void OnBtnXRayClick()
        {
            modelView.ChangeToXRay();
        }

        private void OnBtnTransparentClick()
        {
            modelView.ChangeToTransparent();
        }
    
        private void ShowTreeView(bool isShow)
        {
            var root = modelView.GetRootTransform();
            treeRoot.SetActive(isShow);
        }
    }
}
