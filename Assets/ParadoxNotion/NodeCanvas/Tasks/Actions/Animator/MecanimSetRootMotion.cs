using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Name("Set Animator Root Motion")]
    [Category("Animator")]
    [Description("You can turn on the root motion by using this")]
    public class MecanimSetRootMotion : ActionTask<Animator>
    {

        // protected override string info
        // {
        //    get { return string.Format("Mec.SetTrigger {0}", string.IsNullOrEmpty(parameter.value) && !parameter.useBlackboard ? parameterHashID.ToString() : parameter.ToString()); }
        // }

        protected override void OnExecute()
        {
            agent.applyRootMotion = true;
            EndAction();
        }
    }
}
