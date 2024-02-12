using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

public class CSVParser : MonoBehaviour
{
    public static CSVParser Instance { get; private set; } = new CSVParser();
    [SerializeField] bool setNewScenario;
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

    public List<Step> myScenario = new List<Step>();
    [SerializeField]ScenarioCreator scenarioCreator ;

    public List<Step> GetScenario()
    {
        return myScenario;
    }

    public void Parse(string path)
    {
        FromExcelToList(path);
    }
   
    private void FromExcelToList(string filePath)
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

    }

}



