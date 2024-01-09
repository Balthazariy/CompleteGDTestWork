using UnityEngine;
using UnityEngine.UI;

namespace Test.UI.Base
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class View : MonoBehaviour
    {
        private GameObject _selfObject;
        private Canvas _canvas;

        public virtual void Initialize()
        {
            _selfObject = this.gameObject;

            if (!_selfObject.activeInHierarchy)
            {
                ForceActiveGameObject();
            }

            _canvas = GetComponent<Canvas>();
        }

        private void ForceActiveGameObject() => _selfObject.SetActive(true);

        private bool IsActive() => _canvas.enabled;

        private bool IsInactive() => !_canvas.enabled;

        private bool IsCanvasNull() => _canvas == null;

        public virtual void Show()
        {
            if (IsActive())
            {
                return;
            }

            if (IsCanvasNull())
            {
                return;
            }

            _canvas.enabled = true;
        }

        public virtual void Hide()
        {
            if (IsInactive())
            {
                return;
            }

            if (IsCanvasNull())
            {
                return;
            }

            _canvas.enabled = false;
        }

        public virtual void Dispose()
        {
            _canvas = null;
            _selfObject = null;
        }

        public virtual void Update()
        {
            if (IsInactive())
            {
                return;
            }
        }
    }
}