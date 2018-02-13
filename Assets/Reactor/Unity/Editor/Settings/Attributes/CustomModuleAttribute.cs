using System;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CustomModuleAttribute : Attribute
{
    private readonly string menuItem;

    public string MenuItem
    {
        get
        {
            return this.menuItem;
        }
    }

    private readonly int order;

    public int Order
    {
        get
        {
            return this.order;
        }
    }

    public CustomModuleAttribute(string menuItem)
    {
        this.menuItem = menuItem;
    }


    public CustomModuleAttribute(string menuItem, int order)
    {
        this.menuItem = menuItem;
        this.order = order;
    }
}
