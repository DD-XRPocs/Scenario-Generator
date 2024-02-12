using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CSVParser;

public class StepCharacteristics : MonoBehaviour
{
    public int id;
    public int nbstep;
    public string group;
    public string containerName;
    public bool isPanel;
    public string title;
    public string description;
    public bool isButton;
    public string buttonName;

    public void SetCharacteristics(Step step)
    {
        id = step.id;
        nbstep = step.nbstep;
        group = step.group;
            
        containerName = step.containerName;
        title = step.title;
        description = step.description;
        isButton = step.isButton;
        buttonName = step.buttonName;
    }

    public int GetID() {  return id; }
    public GameObject CheckID(Step step)
    {
        if (id == step.id)
        {
            return this.transform.gameObject;
        }
        return null;
    }
}
