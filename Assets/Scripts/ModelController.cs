using UnityEngine;
using UnityEngine.EventSystems;

namespace BCIT
{
    public class ModelController : MonoBehaviour, IEventListener
    {
        private readonly float rotateSpeeed = 5f;
        private readonly float moveSpeeed = 1f;
        private readonly float moveSingleSpeeed = 0.5f;

        private Camera mainCam;
        private ModelView modelView;
        private GameObject curHoverGameObject;
        
        // Start is called before the first frame update
        void OnEnable()
        {
            Initialize();
        }
        
        private void OnDisable()
        {
            Release();
        }
        
        // Update is called once per frame
        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

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
            modelView.GetRootTransform().Translate(new Vector3(x, y, 0) * moveSpeeed, Space.World);
        }
        
        private void Rotate(float x, float y)
        {
            modelView.GetRootTransform().Rotate(new Vector3(y, x, 0f) * rotateSpeeed);
        }

        private void MoveSinglePart(float x, float y)
        {
            var t = modelView.GetSelectedTransform();
            if (t != null)
            {
                t.Translate(new Vector3(x, y, 0) * moveSingleSpeeed, Space.World);
            }
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
                InputManager.Instance.OnLeftButtonClick -= Select;
            }
        }
    }
}