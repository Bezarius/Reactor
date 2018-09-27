using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ModestTree;
using Reactor.Components;
using UniRx;
using UnityEngine;

public static class RxTypeHelper
{
    public static bool IsReactiveProperty(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>);
    }

    public static bool IsBaseTypeReactiveProperty(this Type type)
    {
        return type.BaseType != null && type.BaseType.IsGenericType &&
               type.BaseType.GetGenericTypeDefinition() == typeof(ReactiveProperty<>);
    }
}

public class BlueprintGenerator : IBlueprintGenerator
{
    private string _path;
    private readonly BlueprintSettings _settings;
    private HashSet<string> _propNames = new HashSet<string>();

    // todo: generate usings
    private readonly string _blueprint = @"using System.Collections.Generic;
using System;
using Assets.Game.Behaviours;
using Assets.Game.Components;
using Assets.Game.Enums;
using Reactor.Blueprints;
using Reactor.Components;
using Reactor.Entities;
using Reactor.Unity.Components;
using UnityEngine;

namespace Assets.Game.Blueprints
{{
    public class {0}Blueprint : IBlueprint
    {{
{1}
        public IEnumerable<IComponent> Build()
        {{
            return new IComponent[]
            {{
{2}
            }};
        }}
    }}
}}
";



    private static string GetStringValue(Type type, object value)
    {
        string stringValue;
        if (type.IsEnum)
            stringValue = string.Format(@"{0}.{1}", type.Name, value);
        else if (type == typeof(float))
            stringValue = string.Format(@"{0}f", value);
        else if (type == typeof(string))
            stringValue = string.Format(@"""{0}""", value);
        else if (type == typeof(bool))
            stringValue = value.ToString().ToLower();
        else if (type.IsReactiveProperty())
        {
            var rVal = GetStringValue(type.GetGenericArguments()[0], value);
            stringValue = rVal != "(null)" ? string.Format(@"{{ Value = {0} }}", rVal) : null;
        }
        else if (type.IsBaseTypeReactiveProperty())
        {
            var rVal = GetStringValue(type.BaseType.GetGenericArguments()[0], value);
            stringValue = rVal != "(null)" ? string.Format(@"{{ Value = {0} }}", rVal) : null;
        }
        else stringValue = value.ToString();
        return stringValue;
    }

    private string GetPropName(FieldInfo info, Type componentType)
    {
        var propName = info.Name;
        if (_propNames.Contains(propName))
        {
            propName = string.Format(@"{0}{1}", componentType.Name, info.Name).Replace("Component", "");
        }
        _propNames.Add(propName);
        return propName;
    }

    private string GetPropType(Type type)
    {
        if (type.IsReactiveProperty())
        {
            return type.GetGenericArguments()[0].Name;
        }
        else if (type.IsBaseTypeReactiveProperty())
        {
            return type.BaseType.GetGenericArguments()[0].Name;
        }
        return type.Name;
    }

    public BlueprintGenerator(string path, BlueprintSettings settings)
    {
        _path = path;
        _settings = settings;
    }

    public void Generate()
    {
        var sb = new StringBuilder();
        var propSb = new StringBuilder();
        _path = Path.Combine(Application.dataPath, _path);
        _path = Path.Combine(_path, string.Format(@"{0}Blueprint.cs", _settings.Name));

        foreach (IComponent component in _settings.Components)
        {
            var componentType = component.GetType();
            sb.AppendLine(string.Format(@"                new {0}", componentType.Name));
            sb.AppendLine("                {");
            foreach (FieldInfo fieldInfo in component.FieldInfos)
            {

                /*
                object value = fieldInfo.GetValue(component);
                var propType = GetPropType(fieldInfo.FieldType);
                var propName = GetPropName(fieldInfo, componentType);
                if (value != null)
                {
                    var strVal = GetStringValue(fieldInfo.FieldType, value);
                    propSb.AppendLine(string.Format(@"public {0} {1} = {2};", propType, propName, strVal));
                }
                else
                {
                    propSb.AppendLine(string.Format(@"public {0} {1};", propType, propName));
                }
                sb.AppendLine(string.Format(@"                    {0} = this.{0},", propName));*/

                object value = fieldInfo.GetValue(component);
                if (value != null && value != fieldInfo.FieldType.GetDefaultValue())
                {
                    var strVal = GetStringValue(fieldInfo.FieldType, value);
                    if (!string.IsNullOrEmpty(strVal))
                        sb.AppendLine(string.Format(@"                    {0} = {1},", fieldInfo.Name, strVal));
                }
            }
            sb.AppendLine("                },");
        }
        var result = string.Format(_blueprint, _settings.Name, propSb, sb);
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
        using (StreamWriter sw = File.CreateText(_path))
        {
            sw.WriteLine(result);
        }
    }
}