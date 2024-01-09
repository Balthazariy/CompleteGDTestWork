using System.Collections.Generic;
using System.Linq;
using Test.Scenes;
using Test.Scenes.Base;
using Test.UI.Base;
using UnityEngine;

namespace Test.Managers
{
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Root view in current scene that never hide by ViewStacks
        /// </summary>
        private View _rootView;

        /// <summary>
        /// All views in current scene
        /// </summary>
        private List<View> _views;

        private View _previousView;

        public View CurrentView { get; private set; }
        public SceneView CurrentSceneView { get; private set; }

        public void SetupViewsInCurrentScene(List<View> views, View rootView, SceneView sceneView)
        {
            CurrentSceneView = sceneView;

            _rootView = rootView;

            _views = views;
        }

        public List<View> TryToRemoveRootViewFromViewList(List<View> targetListOfViews, View rootView)
        {
            for (int i = 0; i < targetListOfViews.Count; i++)
            {
                if (targetListOfViews[i] == rootView)
                {
                    targetListOfViews.RemoveAt(i);
                }
            }

            return targetListOfViews;
        }

        public List<View> TryToRemoveDuplicatesFromViewList(List<View> targetListOfViews)
        {
            List<View> distinationList = targetListOfViews.Distinct().ToList();

            return distinationList;
        }

        public void ShowView<T>() where T : View
        {
            if (CurrentView != null)
            {
                _previousView = CurrentView;
                CurrentView.Hide();
            }

            for (int i = 0; i < _views.Count; i++)
            {
                if (_views[i] is T)
                {
                    CurrentView = _views[i];
                    break;
                }
            }

            CurrentView.Show();
        }

        public void ShowView(View viewToShow)
        {
            if (CurrentView != null)
            {
                _previousView = CurrentView;
                CurrentView.Hide();
            }

            CurrentView = viewToShow;
            CurrentView.Show();
        }

        public void HideCurrentView()
        {
            if (CurrentView != null)
            {
                _previousView = CurrentView;
                CurrentView.Hide();
            }

            CurrentView = _rootView;
        }

        public T GetView<T>() where T : View
        {
            View view = null;

            for (int i = 0; i < _views.Count; i++)
            {
                if (_views[i] is T)
                {
                    view = _views[i];
                    return (T)view;
                }
            }

            return (T)view;
        }
    }
}