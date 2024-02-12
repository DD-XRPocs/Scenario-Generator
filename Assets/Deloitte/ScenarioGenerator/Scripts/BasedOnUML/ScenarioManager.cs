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
       //pass to next marker in timeline
        
    }

    public void CheckStepAndGo(int nextStep)
    {
       //Check nb step and check if next exist call next one
    }

    public void GoToNextStep()
    {
        //ADD +1 and call set step
    }

}
