using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.BehaviourTrees
{

    [Name("AlwaysTrue")]
    [Category("Decorators")]
    [Description("Returns True in every case")]
    [ParadoxNotion.Design.Icon("Remap")]
    public class AlwaysTrue : BTDecorator
    {

        protected override Status OnExecute(Component agent, IBlackboard blackboard)
        {

            if (decoratedConnection == null)
                return Status.Optional;

            status = decoratedConnection.Execute(agent, blackboard);

            switch (status)
            {
                case Status.Success:
                    return Status.Success;
                case Status.Failure:
                    return Status.Success;
            }

            return status;
        }
    }
}