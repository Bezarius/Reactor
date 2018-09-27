using UnityEngine;

public static class EntityHelper
{
    private const string EntityTag = "IEntity";

    public static bool IsEntity(this GameObject gameObject)
    {
        return gameObject.CompareTag(EntityTag);
    }

    public static void SetEntityTag(this GameObject gameObject)
    {
        gameObject.tag = EntityTag;
    }
}
