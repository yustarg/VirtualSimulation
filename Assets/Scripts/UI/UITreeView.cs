using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCIT
{
    public enum CustomEvent
    {
        ItemClicked,
    }

    public class UITreeView : UITreeList
    {
        public Action<UITreeList> OnTreeListAddOneItem;
        public Action<UITreeViewItem> OnItemExpand;
        public Action<UITreeViewItem> OnItemCollapse;
        public Action<UITreeViewItem, CustomEvent, System.Object> OnItemCustomEvent;

        private Dictionary<int, UITreeViewItem> treeViewItemDict = new Dictionary<int, UITreeViewItem>();

        private List<UITreeViewItem> allTreeViewItemList = null;

        private bool needRepositionView = true;

        private bool needRepositionAll = true;

        private int treeItemId = 0;
        [SerializeField] private GameObject itemPrefab = null;
        
        public bool NeedRepositionAll
        {
            get => needRepositionAll;
            set => needRepositionAll = value;
        }
        
        [SerializeField] private float itemIndent;
        [SerializeField] private float childTreeListPadding;
        [SerializeField] private float itemPadding;

        public float ItemIndent
        {
            get => itemIndent;
            set
            {
                itemIndent = value;
                NeedRepositionAll = true;
            }
        }

        public float ChildTreeListPadding
        {
            get => childTreeListPadding;
            set
            {
                childTreeListPadding = value;
                NeedRepositionAll = true;
            }
        }

        public float ItemPadding
        {
            get => itemPadding;
            set
            {
                itemPadding = value;
                NeedRepositionAll = true;
            }
        }

        public bool NeedRepositionView
        {
            get => needRepositionView;
            set => needRepositionView = value;
        }

        public void InitView()
        {
            CachedRectTransform.anchorMax = new Vector2(0, 1);
            CachedRectTransform.anchorMin = CachedRectTransform.anchorMax;
            CachedRectTransform.pivot = new Vector2(0, 1);
            RootTreeView = this;
            ParentTreeItem = null;
        }
        
        public UITreeViewItem GetTreeItemById(int itemId)
        {
            UITreeViewItem item = null;
            if (treeViewItemDict.TryGetValue(itemId, out item))
            {
                return item;
            }
            return null;
        }

        public UITreeViewItem NewTreeItem()
        {
            treeItemId++;
            var go = Instantiate(itemPrefab);
            go.SetActive(true);
            var item = go.GetComponent<UITreeViewItem>();
            item.ItemId = treeItemId;
            treeViewItemDict.Add(item.ItemId, item);
            return item;
        }

        void Update()
        {
            int count = treeItemList.Count;
            for (int i = 0; i < count; ++i)
            {
                UITreeViewItem tItem = treeItemList[i];
                tItem.OnUpdate();
                if (tItem.NeedReposition)
                {
                    NeedRepositionView = true;
                }
            }
            NeedRepositionAll = false;
            if (NeedRepositionView)
            {
                NeedRepositionView = false;
                DoReposition();
            }
        }
    }
}
