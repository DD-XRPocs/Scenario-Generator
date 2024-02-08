using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

public class CSVParser : MonoBehaviour
{
    [SerializeField] bool setNewScenario;
    
    string filePath = Path.Combine(Application.streamingAssetsPath, "scenario.xlsx");
    public struct Step
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
    }

    public static List<Step> myScenario = new List<Step>();
    [SerializeField]ScenarioCreator scenarioCreator ;

    
    // public method which passes in the variables
   
    public List<Step> FromExcelToList(variables)
    {
        //int i = 0;
        myScenario.Clear();
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {

                bool isFirstRow = true;
                while (reader.Read()) // Each ROW
                {
                    // If it's the first row, skip it and continue to the next row
                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }

                    Step step = new Step
                    {
                        id = int.TryParse(reader.GetValue(0)?.ToString(), out int id) ? id : 0,
                        nbstep = int.TryParse(reader.GetValue(1)?.ToString(), out int nbStep) ? nbStep : 0,
                        group = reader.GetValue(2)?.ToString() ?? "Default Group",
                        containerName = reader.GetValue(3)?.ToString() ?? "Default Panel",
                        isPanel = bool.TryParse(reader.GetValue(4)?.ToString(), out bool isPanel) ? isPanel : false,
                        title = reader.GetValue(5)?.ToString() ?? "Default Title",
                        description = reader.GetValue(6)?.ToString() ?? "Default Description",
                        isButton = bool.TryParse(reader.GetValue(7)?.ToString(), out bool isButton) ? isButton : false,
                        buttonName = reader.GetValue(8)?.ToString() ?? "Default Button Name",
                    };
                    myScenario.Add(step);
                }
            }
        }
        return myScenario;
        //scenarioCreator.StepCreation(); // create methode that return  the scenario list instead of calling it 
        // DebugCheck();
    }

    public void DebugCheck()
    {
    
        foreach (var step in myScenario)
        {
            Debug.Log("in step  " + step.title);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CSVParser))]
public class CSVParserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CSVParser parser = (CSVParser)target;

        EditorGUILayout.LabelField("Custom Panel & Scenario Generator :");
        if(GUILayout.Button("Generate Scenario"))
        {
            parser.FromExcelToList(); // switch to creator script

        }
    }
}


#endif