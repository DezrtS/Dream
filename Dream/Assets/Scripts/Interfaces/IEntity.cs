using UnityEngine;

public enum EntityType
{
    None,
    Creature,
    Object,
    Item,
    Environment
}

public interface IEntity
{
    public EntityType EntityType { get; }
    public GameObject GameObject { get; }
}