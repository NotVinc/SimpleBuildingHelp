using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlacementSystem))]
[CanEditMultipleObjects]
public class PlacementSystemEditor : Editor
{
    bool seeList = false;
    SerializedProperty prefabsProp;

    void OnEnable()
    {
        prefabsProp = serializedObject.FindProperty("prefabsToSwitchBetween");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject,
            "prefabsToSwitchBetween"
        );
        GUILayout.Space(3);

        if (GUILayout.Button(GetButtonLabel(0)))
        {
            foreach(var obj in targets)
            {
                PlacementSystem scriptInstance = (PlacementSystem)obj;
                scriptInstance.SwitchPrefab();
                EditorUtility.SetDirty(scriptInstance);
            }
        }

        GUILayout.Space(3);


        if (GUILayout.Button(GetButtonLabel(1)))
        {
            PlacementSystem scriptInstance = (PlacementSystem)target;
            seeList = !seeList;
        }


        EditorGUILayout.PropertyField(prefabsProp, seeList);

        serializedObject.ApplyModifiedProperties();

    }

    public string GetButtonLabel(int i)
    {
        PlacementSystem scriptInstance = (PlacementSystem)target;
        switch (i)
        {
            case 0:
                switch (scriptInstance.type)
                {
                    case PlacementSystem.switchBuildEditoType.Random:
                        return "Get Random";
                    case PlacementSystem.switchBuildEditoType.RandomWithRotation:
                        return "Get Random Object + Random X rotation";
                    default:
                        return "Switch Object";
                }

            case 1:
                switch (seeList)
                {
                    case true: return "Hide Prefab List";
                    case false: return "Edit Prefabs to Switch";
                }

            default: return "Button";
        }
    }
}
