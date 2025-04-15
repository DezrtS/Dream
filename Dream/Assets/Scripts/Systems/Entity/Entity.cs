using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum Type
    {
        None,
        Creature,
        Object,
        Item,
        Environment
    }

    [SerializeField] private Type entityType;
    public Type EntityType => entityType;
}