using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomListener : MonoBehaviour
{
    void OnEnable()
    {
        CustomEvent.onNextStep += HandleNextStep;
    }

    void OnDisable()
    {
        CustomEvent.onNextStep -= HandleNextStep;
    }

    void HandleNextStep()
    {
        Debug.Log("NextStep event triggered!");
        ScenarioManager.scenarioInstance.GoToNextStep();
        
    }
}
