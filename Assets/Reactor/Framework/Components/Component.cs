using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Reactor.Entities;
using UnityEditor;
using UnityEngine;

namespace Reactor.Components
{
    public abstract class EntityComponent<T> : IComponent where T : class, IComponent, new()
    {
        private static int _typeId;

        public int TypeId
        {
            get
            {
                if (_typeId == 0)
                {
#if DEBUG
                    //Debug.Log("init: " + typeof(T));
#endif
                    _typeId = TypeCache<T>.TypeId;
                }
                return _typeId;
            }
        }

        private static readonly Type _type = typeof(T);

        public Type Type
        {
            get { return _type; }
        }

        private static readonly Type _wrapperType =
            typeof(T).Assembly
            .GetTypes()
            .FirstOrDefault(field => field.BaseType != null && field.IsSubclassOf(typeof(ComponentWrapper<T>)));

        public Type WrapperType { get { return _wrapperType; } }

        #region Editor Stuff

#if UNITY_EDITOR
        private static readonly PropertyInfo[] _propertyInfos = _type
            .GetProperties()
            .Where(prop => !Attribute.IsDefined(prop, typeof(HideInInspector)))
            .ToArray();

        [JsonIgnore]
        public PropertyInfo[] PropertyInfos
        {
            get { return _propertyInfos; }
        }

        [HideInInspector]
        public bool IsCollapsed { get; set; }
#endif
        #endregion
    }
}