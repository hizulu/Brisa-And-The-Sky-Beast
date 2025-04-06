using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;

public class Smell : Node
{
    private Blackboard _blackboard;

    public Smell(Blackboard blackboard)
    {
        _blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        PointOfInterest target = _blackboard.GetValue<PointOfInterest>("target");

        if (target == null)
        {
            Debug.Log("No se ha encontrado target, saliendo de Smell...");
            state = NodeState.FAILURE;
        }
        else
        {
            bool hasArrived = _blackboard.GetValue<bool>("hasArrived");

            if (!hasArrived)
            {
                state = NodeState.RUNNING;
            }
            else
            {
                Debug.Log($"Interacted with {target.name}.");
                _blackboard.ClearKey("target");
                _blackboard.SetValue("hasArrived", false); // reset
                state = NodeState.SUCCESS;
            }
        }

        return state;
    }
}
