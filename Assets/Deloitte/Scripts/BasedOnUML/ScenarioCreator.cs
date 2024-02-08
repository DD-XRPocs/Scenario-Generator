using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using static CSVParser;
using System.ComponentModel;
using Timeline.Samples;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Rendering.VirtualTexturing;

public class ScenarioCreator : MonoBehaviour
{

    public GameObject panelPrefab;
    TimelineAsset timeline;
    private PlayableDirector playableDirector;
    public TrackAsset customActivationTrack; // Changed from activationTrack
    public List<CustomActivationTrack> customActivationTracks = new List<CustomActivationTrack>(); // Changed from activationTracks 
    private double startTime = 0.0;
    private double clipDuration = 5.0;
    string groupName;
    int oldStep = 0;
    GameObject instance;

    //Require once director play

    public void StepCreation()
    {
        timeline = ScriptableObject.CreateInstance<TimelineAsset>();
        playableDirector = GetComponent<PlayableDirector>();
        foreach (Step step in myScenario)
        {
            //Debug.Log("in step creation : "+step.title);

            if (step.isPanel)
            {
                GameObject existingPanel = FindPanelWithSameID(step.id);

                if (existingPanel == null)
                {
                    PanelSetUp(step);
                    if (step.isButton)
                    {
                        ButtonSetUp(step);
                    }
                    TimeLineSetUp(step);
                }
            }
            

        }
        TimeLineSaving();
        
    }


    public void PanelSetUp(Step myStep)
    {
        
            //Debug.Log("in panel set up : " + myStep.title);
            instance = Instantiate(panelPrefab);
            instance.name = myStep.containerName;
            instance.transform.SetParent(transform);

            TextMeshProUGUI titleText = instance.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            titleText.text = string.IsNullOrEmpty(myStep.title) ? "Default Title" : myStep.title;

            TextMeshProUGUI descriptionText = instance.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            descriptionText.text = string.IsNullOrEmpty(myStep.description) ? "Default Description" : myStep.description;

            instance.AddComponent<StepCharacteristics>();
            instance.GetComponent<StepCharacteristics>().SetCharacteristics(myStep);
        
    }

    public void ButtonSetUp(Step myStep)
    {
        //Debug.Log("in button creation : " + myStep.title);
        //Transform myPanel = transform.Find("Step_" + myStep.nbstep).GetComponent<Transform>();
        Transform myBtn = instance.transform.Find("Button").GetComponent<Transform>();
        //Debug.Log("in button befor active btn : " + myStep.title);
        myBtn.gameObject.SetActive(true);
        //Debug.Log("in button after active btn : " + myStep.title);
        TextMeshProUGUI textMeshPro = myBtn.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = string.IsNullOrEmpty(myStep.buttonName) ? "Default Button Name" : myStep.buttonName;

        instance.AddComponent<ConditionButton>();
    }

    public void TimeLineSaving()
    {

        string assetPath = "Assets/Deloitte/Timeline/NewScenario.playable";
        TimelineAsset existingAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TimelineAsset>(assetPath);

        if (existingAsset != null)
        {
            UnityEditor.EditorUtility.CopySerialized(timeline, existingAsset);
            UnityEditor.EditorUtility.SetDirty(existingAsset); 
        }
        else
        {
            UnityEditor.AssetDatabase.CreateAsset(timeline, assetPath);
        }

        UnityEditor.AssetDatabase.SaveAssets();

    }

    public void TimeLineSetUp(Step myStep)
    {
        if (myStep.group != "" || myStep.group != null)
        {
            groupName = myStep.group;
            GroupTrack existingGroupTrack = null;

            // Check if the group track already exists
            foreach (TrackAsset track in timeline.GetRootTracks())
            {
                if (track is GroupTrack groupTrack && groupTrack.name == groupName)
                {
                    existingGroupTrack = groupTrack;
                    break;
                }
            }


            // If the group track doesn't exist, create a new one
            if (existingGroupTrack == null)
            {
                existingGroupTrack = timeline.CreateTrack<GroupTrack>(null, groupName);
            }

            customActivationTrack = timeline.CreateTrack<CustomActivationTrack>(existingGroupTrack, "CustomActivationTrack");
           
            var markerTrack = timeline.CreateTrack<MarkerTrack>(null, "MarkerTrack");
            var annotationMarker = markerTrack.CreateMarker<AnnotationMarker>(startTime + 5f);
            annotationMarker.color = Color.green;
            annotationMarker.title = myStep.group;
        }
        else
        {
            customActivationTrack = timeline.CreateTrack<CustomActivationTrack>(null, "CustomActivationTrack");
        }

        GameObject go = instance;
        TimelineClip timelineClip = customActivationTrack.CreateClip<CustomActivationAsset>(); // changed from activationTrack

        timelineClip.start = startTime;
        timelineClip.duration = clipDuration;

        CustomActivationAsset asset = (CustomActivationAsset)timelineClip.asset;
        asset.targetObject.exposedName = myStep.title;
        asset.targetObject.defaultValue = go;
        playableDirector.SetReferenceValue(asset.targetObject.exposedName, go);

        if (myStep.nbstep != oldStep)
        {
            startTime += clipDuration + 5.0;
            oldStep = myStep.nbstep;
        }

        
        playableDirector.playableAsset = timeline;
        playableDirector.initialTime = startTime;
        playableDirector.Evaluate();
        //TimeLineSaving();
    }

    public GameObject FindPanelWithSameID(int ID)
    {

        StepCharacteristics[] children = this.transform.GetComponentsInChildren<StepCharacteristics>(true);
        Debug.Log("number of panels " + children.Length);
        foreach (StepCharacteristics child in children)
        {
            Debug.Log("the ID of new step : " + ID + "  The ID of panel : " + child.GetID());
            if (child.GetID() == ID)
            {
                Debug.Log("there is a same");
                return child.gameObject;
            }
        }
        Debug.Log("there is NO same");
        return null;
    }


}
