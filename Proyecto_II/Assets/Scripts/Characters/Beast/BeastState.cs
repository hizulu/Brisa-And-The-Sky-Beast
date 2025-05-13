
// Jone Sainz Egea
// 15/04/2025
// Clase abstracta que sirve de molde para los estados de la Bestia
public abstract class BeastState
{
    public virtual void OnEnter(Beast beast) { }

    public virtual void OnUpdate(Beast beast) { }

    public virtual void OnExit(Beast beast) { }
}
