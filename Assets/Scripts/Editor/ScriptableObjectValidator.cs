using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

[assembly: RegisterValidator(typeof(ScriptableObjectValidator<>))]

public class ScriptableObjectValidator<T> : ValueValidator<T> where T : ScriptableObject
{
    protected override void Validate(ValidationResult result)
    {
        if (ValueEntry.SmartValue == null)
        {
            result.ResultType = ValidationResultType.Error;
            result.Message = "This Scriptable Object is empty!";
        }
    }
}