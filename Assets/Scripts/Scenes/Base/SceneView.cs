using System.Collections.Generic;
using Test.Managers;
using Test.UI.Base;
using UnityEngine;
using Zenject;

namespace Test.Scenes.Base
{
    public class SceneView : MonoBehaviour
    {
        [Header("Root view that never been hiden by ViewStacks")]
        [SerializeField] private View _rootView;

        [Space(4)]
        [Header("List of all view in current scene exept root view")]
        [SerializeField] private List<View> _views = new List<View>();

        protected UIManager _uiSystem;

        [Inject]
        public void Construct(UIManager uiSystem)
        {
            _uiSystem = uiSystem;

            Initialize();
        }

        public virtual void Initialize()
        {
            if (_rootView != null)
            {
                _views = _uiSystem.TryToRemoveRootViewFromViewList(_views, _rootView);
            }

            _views = _uiSystem.TryToRemoveDuplicatesFromViewList(_views);

            _uiSystem.SetupViewsInCurrentScene(_views, _rootView, this);

            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Initialize();

                _views[i].Hide();
            }

            if (_rootView != null)
            {
                _rootView.Initialize();

                _rootView.Show();
            }
        }

        private void Update()
        {
            if (_views == null)
            {
                return;
            }

            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Update();
            }
        }

        public virtual void ShowView(View view)
        {
            _uiSystem.ShowView(view);
        }

        public virtual void HideView()
        {
            _uiSystem.HideCurrentView();
        }

        public virtual void Dispose()
        {
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].Dispose();
            }

            _views.Clear();

            _views = null;

            _rootView.Dispose();
            _rootView = null;

            _uiSystem = null;
        }
    }
}