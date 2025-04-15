using System.Collections.Generic;

public interface IUseable
{
    public delegate void UseHandler(IUseable useable, bool isUsing, UseContext useContext);
    public event UseHandler OnUsed;
    public bool IsUsing { get; }
    public abstract bool CanUse();
    public abstract bool CanStopUsing();
    public abstract void Use(UseContext useContext);
    public abstract void StopUsing(UseContext useContext);
}

public struct UseContext
{
    public Entity SourceEntity;
    // Timestamp

    public Dictionary<string, object> Parameters;
}