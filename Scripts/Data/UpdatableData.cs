using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class UpdatableData : ScriptableObject
{
    public event System.Action OnValueUpdated; // saving the updating mesh map after changing terrain data values
    public bool autoUpdated;
    public bool alreadyUpdated = false;

    protected virtual void OnValidate()
    { // check the value of inspector when it changes
        if (autoUpdated)
        {
            Debug.Log("Update Values");
            UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
        }
    }

    public void NotifyOfUpdatedValues()
    {
        Debug.Log(alreadyUpdated);
        if (OnValueUpdated != null && alreadyUpdated==false)
        {
            OnValueUpdated();
            alreadyUpdated=true;

        }
        if (alreadyUpdated) {
            UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
            alreadyUpdated = false;
        }
       }
}

 