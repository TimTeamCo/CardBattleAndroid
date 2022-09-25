using System;

public abstract class Observed<T>
{
    /// If you want to copy all of the values, and only trigger OnChanged once.
    public abstract void CopyObserved(T oldObserved);

    public Action<T> onChanged { get; set; }
    public Action<T> onDestroyed { get; set; }

    /// Should be implemented into every public property of the observed 
    protected void OnChanged(T observed)
    {
        onChanged?.Invoke(observed);
    }

    protected void OnDestroyed(T observed)
    {
        onDestroyed?.Invoke(observed);
    }
}
