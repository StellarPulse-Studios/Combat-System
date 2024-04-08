using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("Blackboard")]
[Description("Set a blackboard boolean variable and VERS variable increment by 1")]
public class SetBoolAndIncrementVERS : ActionTask
{
    // Start is called before the first frame update
    public enum BoolSetModes
    {
        False = 0,
        True = 1,
        Toggle = 2
    }

    [RequiredField]
    [BlackboardOnly]
    public BBParameter<bool> boolVariable;
    public BoolSetModes setTo = BoolSetModes.True;
    public BBParameter<GameObject> enemy;

    protected override string info
    {
        get
        {
            if (setTo == BoolSetModes.Toggle)
                return "Toggle " + boolVariable.ToString() + " and + VERS by 1";

            return "Set " + boolVariable.ToString() + " to " + setTo.ToString() + " and + VERS by 1";
        }
    }

    protected override void OnExecute()
    {

        if (setTo == BoolSetModes.Toggle)
        {

            boolVariable.value = !boolVariable.value;

        }
        else
        {
            var checkBool = ((int)setTo == 1);
            boolVariable.value = checkBool;
        }
        enemy.value.GetComponent<OrcVRES>().incrementByOne();
        EndAction();
    }
}
