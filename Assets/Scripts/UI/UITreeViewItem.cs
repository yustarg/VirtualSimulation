using UnityEngine;
using UnityEngine.UI;

namespace BCIT
{
    public enum ExpandStatus
    {
        Expand,
        Collapse,
    }
    
    public class UITreeViewItem : MonoBehaviour
    {
        int itemIndex;
        int itemId; 
        
        UITreeView rootTreeView;
        UITreeList parentTreeList;
        UITreeList childTreeList;
        
        public float MaxWidth
        {
            get
            {
                var selfWidth = CachedRectTransform.rect.width;
                if (HasChildItem == false)
                {
                    return selfWidth;
                }
                if (IsCollapse)
                {
                    return selfWidth;
                }
                var childListWidth = childTreeList.ContentTotalWidth;
                return Mathf.Max(selfWidth, childListWidth + ChildTreeIndent);
            }
        }

        public float ChildTreeItemPadding => RootTreeView.ItemPadding;
        
        public float ChildTreeListPadding => RootTreeView.ChildTreeListPadding;

        private float ChildTreeIndent => RootTreeView.ItemIndent;


        private RectTransform cachedRectTransform;
        private  bool needReposition = true;

        private float totalHeight = 0f;
        private ExpandStatus expandStatus = ExpandStatus.Expand;

        private ExpandStatus CurExpandStatus
        {
            get => expandStatus;
            set => expandStatus = value;
        }

        public bool IsExpand => (CurExpandStatus == ExpandStatus.Expand);

        private bool IsCollapse => (CurExpandStatus == ExpandStatus.Collapse);

        public void DoExpandOrCollapse()
        {
            if (IsExpand)
            {
                Collapse();
            }
            else
            {
                Expand();
            }
        }

        private void Expand()
        {
            if (HasChildItem == false || CurExpandStatus == ExpandStatus.Expand)
            {
                CurExpandStatus = ExpandStatus.Expand;
                return;
            }
            
            needReposition = true;

            childTreeList.gameObject.SetActive(true);
            if (!IsExpand)
            {
                OnExpandBegin();
            }
            
            CurExpandStatus = ExpandStatus.Expand;
        }

        private void Collapse()
        {
            if (HasChildItem == false || CurExpandStatus == ExpandStatus.Collapse)
            {
                CurExpandStatus = ExpandStatus.Collapse;
                return;
            }

            needReposition = true;

            if (!IsCollapse)
            {
                OnCollapseBegin();
            }

            CurExpandStatus = ExpandStatus.Collapse;
            childTreeList.gameObject.SetActive(false);
        }

        public float TotalHeight => totalHeight;

        public bool NeedReposition
        {
            get
            {
                if (needReposition)
                {
                    return true;
                }
                if (childTreeList != null)
                {
                    return childTreeList.NeedReposition;
                }
                return false;
            }
        }

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
        
        public int ItemIndex
        {
            get => itemIndex;
            set => itemIndex = value;
        }

        public int ItemId
        {
            get => itemId;
            set => itemId = value;
        }
        public UITreeView RootTreeView
        {
            get => rootTreeView;
            set => rootTreeView = value;
        }
        public UITreeList ParentTreeList
        {
            get => parentTreeList;
            set => parentTreeList = value;
        }

        public UITreeList ChildTree
        {
            get
            {
                EnsureChildTreeListCreated();
                return childTreeList;
            }
        }

        private bool HasChildItem => (ChildItemCount > 0);

        private int ChildItemCount => childTreeList == null ? 0 : childTreeList.ItemCount;

        void OnExpandBegin()
        {
            RootTreeView.OnItemExpand?.Invoke(this);
        }

        void OnCollapseBegin()
        {
            RootTreeView.OnItemCollapse?.Invoke(this);
        }

        public void Init()
        {
            needReposition = true;
            totalHeight = 0f;
            CurExpandStatus = ExpandStatus.Expand;
            if(childTreeList != null)
            {
                childTreeList.gameObject.SetActive(true);
                childTreeList.Init();
            }
        }

        public void Clear()
        {
            if (childTreeList != null)
            {
                childTreeList.Clear();
            }
        }

        public void RaiseCustomEvent(CustomEvent customEvent, System.Object param)
        {
            if(RootTreeView.OnItemCustomEvent != null)
            {
                RootTreeView.OnItemCustomEvent(this, customEvent, param);
            }
        }

        void EnsureChildTreeListCreated()
        {
            if (childTreeList != null)
            {
                return;
            }
            GameObject go = new GameObject();
            go.name = "ChildTree";
            go.layer = CachedRectTransform.gameObject.layer;
            RectTransform tf = go.GetComponent<RectTransform>();
            if (tf == null)
            {
                tf = go.AddComponent<RectTransform>();
            }
            tf.SetParent(CachedRectTransform);
            tf.anchorMax = new Vector2(0f, 1f);
            tf.anchorMin = tf.anchorMax;
            tf.pivot = new Vector2(0.5f, 1);
            childTreeList = tf.gameObject.AddComponent<UITreeList>();
            childTreeList.RootTreeView = RootTreeView;
            childTreeList.ParentTreeItem = this;
            childTreeList.CachedRectTransform.localEulerAngles = Vector3.zero;
            childTreeList.CachedRectTransform.localScale = Vector3.one;
            needReposition = true;
            if(CurExpandStatus == ExpandStatus.Collapse)
            {
                childTreeList.gameObject.SetActive(false);
            }
        }
        
        public void OnUpdate()
        {
            if (RootTreeView.NeedRepositionAll)
            {
                needReposition = true;
            }
            if (HasChildItem == false)
            {
                return;
            }
            childTreeList.OnUpdate();
            if (childTreeList.NeedReposition)
            {
                needReposition = true;
            }
        }

        public void Reposition()
        {
            if (NeedReposition == false)
            {
                return;
            }
            DoReposition();
        }

        public void DoReposition()
        {
            needReposition = false;
            if (childTreeList == null)
            {
                totalHeight = cachedRectTransform.rect.height;
                return;
            }
            childTreeList.Reposition();
            if (CurExpandStatus == ExpandStatus.Expand)
            {
                float itemHeight = CachedRectTransform.rect.height;
                childTreeList.CachedRectTransform.anchoredPosition3D = new Vector3(ChildTreeIndent, -itemHeight, 0);
                float childTreeListHeight = childTreeList.ContentTotalHeight;
                totalHeight = cachedRectTransform.rect.height + childTreeListHeight;
                childTreeList.CachedRectTransform.localScale = Vector3.one;
                childTreeList.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, childTreeListHeight);
            }
            else if (CurExpandStatus == ExpandStatus.Collapse)
            {
                totalHeight = cachedRectTransform.rect.height;
                childTreeList.CachedRectTransform.localScale = Vector3.one;
            }
        }
    }
}