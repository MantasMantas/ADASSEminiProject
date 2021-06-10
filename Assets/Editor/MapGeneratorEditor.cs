using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (GenerateMap))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GenerateMap mapGen = (GenerateMap)target;

        if(DrawDefaultInspector())
        {
            if(mapGen.autoUpdate)
            {
                mapGen.generateMap();
            }
        }

        if(GUILayout.Button ("Generate"))
        {
            mapGen.generateMap();
        }
    }
}
