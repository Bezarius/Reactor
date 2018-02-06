namespace Reactor.Unity.Components
{
    public class ViewComponentWrapper : ComponentWrapper<ViewComponent>
    {
        protected override void Initialize()
        {
            (this.Component as ViewComponent).View = this.gameObject;
        }
    }
}