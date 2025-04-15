using BehaviorTree;

// Jone Sainz Egea
// 06/04/2025
// Nodo que se encarga de ver si la acción que debe hacer es olfatear
// Por ahora, en todos los tipos de objeto de PointOfInterest su acción es olfatear
public class CheckSmellable : Node
{
    private Blackboard _blackboard;

    public CheckSmellable(Blackboard blackboard)
    {
        _blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        PointOfInterest target = _blackboard.GetValue<PointOfInterest>("target");
        state = target != null ? NodeState.SUCCESS : NodeState.FAILURE;
        return state;
    }
}
