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
using System.IO;

[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(CSVParser))]
public class ScenarioCreator : MonoBehaviour
{

    public GameObject panelPrefab;
    TimelineAsset timeline;
    PlayableDirector playableDirector;
    public TrackAsset customActivationTrack; // Changed from activationTrack
    public List<CustomActivationTrack> customActivationTracks = new List<CustomActivationTrack>(); // Changed from activationTracks 
    private double startTime = 0.0;
    private double clipDuration = 5.0;
    string groupName;
    int oldStep = 0;

    private GameObject instance;
    private List<Step> myScenario;
    [SerializeField] string filePath;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
       
    }
    public void StepsCreation()
    {
        //Reset var 
        startTime = 0.0;
        clipDuration = 5.0;
        oldStep = 0;
        timeline = ScriptableObject.CreateInstance<TimelineAsset>();
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.initialTime = startTime;
        if(filePath == null || filePath == "")
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "scenario.xlsx");
        }
        //Parse file and get the list of steps
        CSVParser.Instance.Parse(filePath);
        myScenario = CSVParser.Instance.GetScenario();

        
        foreach (Step step in myScenario)
        {

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
                    SetTimeLine(step);
                }
            }
            

        }
        SaveTimeLine();
        
    }


    public void PanelSetUp(Step myStep)
    {

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
        Transform myBtn = instance.transform.Find("Button").GetComponent<Transform>();
        myBtn.gameObject.SetActive(true);
        TextMeshProUGUI textMeshPro = myBtn.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = string.IsNullOrEmpty(myStep.buttonName) ? "Default Button Name" : myStep.buttonName;

        instance.AddComponent<ConditionButton>();
    }

    public void SaveTimeLine()
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

    public void SetTimeLine(Step myStep)
    {
        //Check for markerTrack
        MarkerTrack markerTrack =null;
        foreach (TrackAsset track in timeline.GetRootTracks())
        {
            
            if (track is MarkerTrack marker)
            {
                markerTrack= marker;
                break;
            }
        }
        if(markerTrack == null)
        {
            markerTrack = timeline.CreateTrack<MarkerTrack>(null, "MarkerTrack");
        }
       

        if (myStep.nbstep != oldStep)
        {
            startTime += clipDuration + 5.0f;
            clipDuration += 10f;
            Debug.Log("start time : " + startTime);
            oldStep = myStep.nbstep;
            var annotationMarker = markerTrack.CreateMarker<AnnotationMarker>(clipDuration);

            annotationMarker.color = Color.green;
            annotationMarker.title = myStep.nbstep.ToString();
        }

        if (myStep.group != "" || myStep.group != null)
        {
            groupName = myStep.group;
            GroupTrack existingGroupTrack = null;
           
            //Check for group track
            foreach (TrackAsset track in timeline.GetRootTracks())
            {
                if (track is GroupTrack groupTrack && groupTrack.name == groupName)
                {
                    existingGroupTrack = groupTrack;
                    break;
                }
            }

            if (existingGroupTrack == null)
            {
               
                existingGroupTrack = timeline.CreateTrack<GroupTrack>(null, groupName);
            }
            customActivationTrack = timeline.CreateTrack<CustomActivationTrack>(existingGroupTrack, myStep.containerName);

        }
        else
        {
            customActivationTrack = timeline.CreateTrack<CustomActivationTrack>(null, myStep.containerName);
        }

        GameObject go = instance;
        TimelineClip timelineClip = customActivationTrack.CreateClip<CustomActivationAsset>();

        timelineClip.start = startTime;
        timelineClip.duration = clipDuration;

        CustomActivationAsset asset = (CustomActivationAsset)timelineClip.asset;
        asset.targetObject.exposedName = myStep.title;
        asset.targetObject.defaultValue = go;
        playableDirector.SetReferenceValue(asset.targetObject.exposedName, go);  
        playableDirector.playableAsset = timeline;
        playableDirector.initialTime = startTime;
        playableDirector.Evaluate();
        
    }

    public GameObject FindPanelWithSameID(int ID)
    {

        StepCharacteristics[] children = this.transform.GetComponentsInChildren<StepCharacteristics>(true);
       
        foreach (StepCharacteristics child in children)
        {
          
            if (child.GetID() == ID)
            {
               
                return child.gameObject;
            }
        }
       
        return null;
    }


}


#if UNITY_EDITOR
[CustomEditor(typeof(ScenarioCreator))]
public class ScenarioCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ScenarioCreator creator = (ScenarioCreator)target;

        EditorGUILayout.LabelField("Custom Panel & Scenario Generator :");
        if (GUILayout.Button("Generate Scenario"))
        {
            creator.StepsCreation(); 

        }
    }
}
#endif