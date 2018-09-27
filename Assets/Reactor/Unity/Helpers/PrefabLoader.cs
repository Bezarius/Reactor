using System;
using System.Linq;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class PrefabLoader<TEnum> : IPrefabLoader where TEnum : struct, IComparable, IFormattable, IConvertible// i.e enum
    {
        public GameObject[] Prefabs
        {
            get
            {
                if (_prefabs == null)
                    Load();
                return _prefabs;
            }
            private set { _prefabs = value; }
        }

        private readonly string _path;
        private bool _isLoaded;
        private GameObject[] _prefabs;

        public PrefabLoader(string pathToPrefabs)
        {
            _path = pathToPrefabs;
        }

        public void Load()
        {
            if (_isLoaded)
                return;
            var type = typeof(TEnum);
            if (type.IsEnum)
            {
                var names = Enum.GetNames(typeof(TEnum)).ToList();
                Prefabs = new GameObject[names.Count];

                for (int i = 0; i < names.Count; i++)
                {
                    if (names[i] == "None") continue; // skip 'None'
                    var path = string.Format("{0}{1}", _path, names[i]);
                    var prefab = Resources.Load<GameObject>(path);
                    if (prefab != null)
                    {
                        Prefabs[i] = prefab;
                    }
                    else
                    {
                        Debug.LogErrorFormat("Prefab with path '{0}' not found!", path);
                    }
                }
                _isLoaded = true;
            }
            else
            {
                Debug.LogErrorFormat("{0} can't be used for prefab load", type);
            }
        }
    }
}