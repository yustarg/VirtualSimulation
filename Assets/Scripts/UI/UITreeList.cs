using System.Collections.Generic;
using UnityEngine;

namespace BCIT
{
    public class UITreeList : MonoBehaviour
    {
        private float treeListMaxWidth = 9999f;
        protected List<UITreeViewItem> treeItemList = new List<UITreeViewItem>();
        protected UITreeView rootTreeView;
        protected UITreeViewItem parentTreeItem;

        protected RectTransform cachedRectTransform;
        protected bool needReposition = true;

        protected float contentTotalWidth;
        protected float contentTotalHeight;

        private float TreeListMaxWidth
        {
            get => treeListMaxWidth;
            set => treeListMaxWidth = value;
        }

        public float ContentTotalHeight => contentTotalHeight;

        public float ContentTotalWidth => contentTotalWidth;

        public bool NeedReposition => needReposition;

        public RectTransform CachedRectTransform
        {
            get
            {
                if (cachedRectTransform == null)
                {
                    cachedRectTransform = gameObject.GetComponent<RectTransform>();
                }
                return cachedRectTransform;
            }
        }

        public UITreeView RootTreeView
        {
            get => rootTreeView;
            set => rootTreeView = value;
        }
        public UITreeViewItem ParentTreeItem
        {
            get => parentTreeItem;
            set => parentTreeItem = value;
        }
        
        public int ItemCount => treeItemList.Count;

        private bool IsRootTree => ReferenceEquals(RootTreeView, this);

        public void Init()
        {
            needReposition = true;
            contentTotalHeight = 0;
            contentTotalWidth = 0;
            treeItemList.Clear();
        }

        void InitTreeViewItem(UITreeViewItem tViewItem)
        {
            tViewItem.CachedRectTransform.SetParent(CachedRectTransform);
            tViewItem.CachedRectTransform.localEulerAngles = Vector3.zero;
            tViewItem.CachedRectTransform.localScale = Vector3.one;
            tViewItem.RootTreeView = RootTreeView;
            tViewItem.ParentTreeList = this;
        }
        
        public UITreeViewItem AppendItem(System.Object userData = null)
        {
            UITreeViewItem tViewItem = rootTreeView.NewTreeItem();
            if(tViewItem == null)
            {
                Debug.LogError("AppendItem return null ");
                return null;
            }
            InitTreeViewItem(tViewItem);
            tViewItem.Init();
            tViewItem.ItemIndex = treeItemList.Count;
            treeItemList.Add(tViewItem);
            UpdateAllItemSiblingIndex();
            needReposition = true;
            if (IsRootTree)
            {
                RootTreeView.NeedRepositionView = true;
            }
            if (RootTreeView.OnTreeListAddOneItem != null)
            {
                RootTreeView.OnTreeListAddOneItem(this);
            }
            return tViewItem;
        }

        private void UpdateAllItemSiblingIndex()
        {
            int count = treeItemList.Count;
            for (int i = 0; i < count; ++i)
            {
                treeItemList[i].CachedRectTransform.SetSiblingIndex(i);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < treeItemList.Count; ++i)
            {
                treeItemList[i].Clear();
            }
            treeItemList.Clear();
        }

        public void OnUpdate()
        {
            int count = treeItemList.Count;
            for (int i = 0; i < count; ++i)
            {
                UITreeViewItem tItem = treeItemList[i];
                tItem.OnUpdate();
                if (tItem.NeedReposition)
                {
                    needReposition = true;
                }
            }
        }

        public UITreeViewItem GetItemByIndex(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= treeItemList.Count)
            {
                return null;
            }
            return treeItemList[itemIndex];
        }

        public void Reposition()
        {
            if (NeedReposition == false)
            {
                return;
            }
            DoReposition();
        }

        protected void DoReposition()
        {
            needReposition = false;
            int count = treeItemList.Count;
            contentTotalHeight = 0;
            contentTotalWidth = 0;
            CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            if(IsRootTree == false)
            {
                CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TreeListMaxWidth);
            }
            if (count == 0)
            {
                return;
            }
            float xOffset = CachedRectTransform.pivot.x* CachedRectTransform.sizeDelta.x;
            float itemPadding = 0;
            if (ParentTreeItem != null)
            {
                itemPadding = ParentTreeItem.ChildTreeItemPadding;
            }
            else
            {
                itemPadding = RootTreeView.ItemPadding;
            }
            float curY = 0;
            if (ParentTreeItem != null)
            {
                curY = -ParentTreeItem.ChildTreeListPadding;
            }
            for (int i = 0; i < count; ++i)
            {
                UITreeViewItem tItem = treeItemList[i];
                tItem.CachedRectTransform.anchoredPosition3D = new Vector3(xOffset, curY, 0);
                tItem.Reposition();
                
                curY = curY - tItem.TotalHeight - itemPadding;
                if (tItem.MaxWidth > contentTotalWidth)
                {
                    contentTotalWidth = tItem.MaxWidth;
                }
            }
            contentTotalHeight = -curY - itemPadding;
            CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentTotalHeight);
            if (IsRootTree)
            {
                CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentTotalWidth);
            }
        }
    }
}