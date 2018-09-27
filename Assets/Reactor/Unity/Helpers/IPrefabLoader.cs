using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public interface IPrefabLoader
    {
        GameObject[] Prefabs { get; }

        void Load();
    }
}