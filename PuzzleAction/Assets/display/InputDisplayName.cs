using UnityEngine;
using UnityEngine.InputSystem;

public class InputDisplayName
{
    public string GetInputName(InputActionReference actionReference)
    {
        return actionReference.action.GetBindingDisplayString();
    }
}
