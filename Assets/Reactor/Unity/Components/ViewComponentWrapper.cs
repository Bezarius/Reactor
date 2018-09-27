namespace Reactor.Unity.Components
{
    public class ViewComponentWrapper : ComponentWrapper<ViewComponent>
    {
        protected override void Initialize()
        {
            var view = this.Component as ViewComponent;
            if (view != null && view.GameObject == null)
                view.GameObject = this.gameObject;
        }
    }
}