using UnityEngine;
using UnityEngine.EventSystems;

namespace BCIT
{
    public class ModelController : MonoBehaviour, IEventListener
    {
        private readonly float rotateSpeeed = 3f;
        private readonly float moveSpeeed = 0.5f;
        private readonly float moveSingleSpeeed = 0.3f;

        private Camera mainCam;
        private ModelView modelView;
        private GameObject curHoverGameObject;
        private bool canHover = true;
        
        // Start is called before the first frame update
        void OnEnable()
        {
            Initialize();
        }
        
        private void OnDisable()
        {
            Release();
        }

        private bool IsMouseOverUI => EventSystem.current.IsPointerOverGameObject();
        
        // Update is called once per frame
        void Update()
        {
            if (!canHover) return;

            if (IsMouseOverUI) return;
            
            if (mainCam != null && modelView != null)
            {
                var ray = mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    curHoverGameObject = hitInfo.transform.gameObject;
                }
                else
                {
                    curHoverGameObject = null;
                }
                modelView.OnHover(curHoverGameObject);
                UIManager.Instance.ShowTip(curHoverGameObject);
            }
        }
        
        private void Select(float x, float y)
        {
            if (curHoverGameObject != null)
            {
                modelView.OnSelect(curHoverGameObject);
                UIManager.Instance.OnModelPartSelect(curHoverGameObject);
            }
        }
        
        private void Move(float x, float y)
        {
            canHover = false;
            modelView.GetRootTransform().Translate(new Vector3(x, y, 0) * moveSpeeed, Space.World);
        }
        
        private void MoveEnd(float x, float y)
        {
            canHover = true;
        }
        
        private void Rotate(float x, float y)
        {
            canHover = false;
            modelView.GetRootTransform().Rotate(new Vector3(0f, x, 0f) * rotateSpeeed);
        }
        
        private void RotateEnd(float x, float y)
        {
            canHover = true;
        }

        private void MoveSinglePart(float x, float y)
        {
            if (IsMouseOverUI) return;
            canHover = false;
            var t = modelView.GetSelectedTransform();
            if (t != null && curHoverGameObject != null && curHoverGameObject == t.gameObject)
            {
                t.Translate(new Vector3(x, y, 0) * moveSingleSpeeed, Space.World);
            }
        }
        
        private void MoveSinglePartEnd(float x, float y)
        {
            canHover = true;
        }

        private void Initialize()
        {
            AddListeners();
            mainCam = Camera.main;
        }

        private void Release()
        {
            RemoveListeners();
        }

        public void SetView(ModelView viewer)
        {
            this.modelView = viewer;
        }

        public void AddListeners()
        {
            InputManager.Instance.OnRightButton += Rotate;
            InputManager.Instance.OnMiddleButton += Move;
            InputManager.Instance.OnLeftButton += MoveSinglePart;
            InputManager.Instance.OnLeftButtonUp += MoveSinglePartEnd;
            InputManager.Instance.OnRightButtonUp += RotateEnd;
            InputManager.Instance.OnMiddleButtonUp += MoveEnd;
            InputManager.Instance.OnLeftButtonUp += MoveSinglePartEnd;
            InputManager.Instance.OnLeftButtonClick += Select;
        }

        public void RemoveListeners()
        {
            // singleton may be destroyed earlier
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnRightButton -= Rotate;
                InputManager.Instance.OnMiddleButton -= Move;
                InputManager.Instance.OnLeftButton -= MoveSinglePart;
                InputManager.Instance.OnLeftButtonUp -= MoveSinglePartEnd;
                InputManager.Instance.OnRightButtonUp -= RotateEnd;
                InputManager.Instance.OnMiddleButtonUp -= MoveEnd;
                InputManager.Instance.OnLeftButtonUp -= MoveSinglePartEnd;
                InputManager.Instance.OnLeftButtonClick -= Select;
            }
        }
    }
}