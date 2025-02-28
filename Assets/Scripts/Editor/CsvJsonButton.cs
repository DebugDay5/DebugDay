
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConverterTestManager))]
public class CsvJsonButton : Editor
{
    public string fileName;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ConverterTestManager mana = (ConverterTestManager)target;

        if (GUILayout.Button("Cvs to Json"))
        {
            mana.CsvConverByName();
        }
    }

}
