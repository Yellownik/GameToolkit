using Orbox.Utils;
using UI.Menus;

namespace UI
{
    public class ViewFactory
    {
        private IUIRoot UIRoot;
        private IResourceManager ResourceManager;

        public ViewFactory(IResourceManager resourceManager, IUIRoot uiRoot)
        {
            UIRoot = uiRoot;
            ResourceManager = resourceManager;
        }

        public MainMenuView CreateMainMenuView()
        {
            var view = ResourceManager.CreatePrefabInstance<EViews, MainMenuView>(EViews.MainMenuView, UIRoot.MenuCanvas);
            return view;
        }

        public PauseMenuView CreatePauseMenuView()
        {
            var view = ResourceManager.CreatePrefabInstance<EViews, PauseMenuView>(EViews.PauseMenuView, UIRoot.MenuCanvas);
            return view;
        }

        public TitresMenuView CreateTitresMenuView()
        {
            var view = ResourceManager.CreatePrefabInstance<EViews, TitresMenuView>(EViews.TitresMenuView, UIRoot.MenuCanvas);
            return view;
        }
    }
}
