using UnityEngine;

namespace BCIT
{
    public class ModelController : MonoBehaviour, IEventListener
    {
        private readonly float rotateSpeeed = 5f;
        private readonly float moveSpeeed = 1f;

        private Camera mainCam;
        private ModelView modelView;
        private GameObject curSelectionGameObject;
        
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
            if (mainCam != null)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    modelView.OnHover(hitInfo.transform.gameObject, true);
                }
                else
                {
                    modelView.OnHover(null, false);
                }
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
            
        }
        
        private void Select(float x, float y)
        {
            
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