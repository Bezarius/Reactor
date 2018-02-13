using UnityEngine;

public static class EntityHelper
{
    public static bool IsEntity(this GameObject gameObject)
    {
        return gameObject.CompareTag("IEntity");
    }
}
