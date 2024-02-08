using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static CSVParser;

public class ScenarioManager : MonoBehaviour
{
    public int currentStep;
    public static ScenarioManager scenarioInstance;
    int maxNbSteps;

   
    void Awake()
    {
        scenarioInstance = GetComponent<ScenarioManager>();

    }
  

    public void SetStep(int index)
    {
        currentStep = Mathf.Clamp(index, 0, myScenario.Count - 1);
        
        foreach (Transform stepContent in this.transform)
        {
            stepContent.gameObject.SetActive(false);
        }
        Transform thisStep = transform.Find("Step_" + myScenario[index - 1].nbstep).GetComponent<Transform>();
        thisStep.gameObject.SetActive(true);
        
    }

    public void CheckStepAndGo(int nextStep)
    {
        if (currentStep == nextStep - 1)
        {
            SetStep(nextStep);
            //if (validateStepSound != null) GetComponent<AudioSource>().PlayOneShot(validateStepSound);
        }
    }

    public void GoToNextStep()
    {
        int next = currentStep++;
        SetStep(next);
    }

}
