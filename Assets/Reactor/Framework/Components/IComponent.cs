using System;
using System.Reflection;

namespace Reactor.Components
{
    public interface IComponent
    {
        int TypeId { get; }
        Type Type { get; }
        Type WrapperType { get; }

#if UNITY_EDITOR
        PropertyInfo[] PropertyInfos { get; }
        bool IsCollapsed { get; set; }
#endif
    }
}
