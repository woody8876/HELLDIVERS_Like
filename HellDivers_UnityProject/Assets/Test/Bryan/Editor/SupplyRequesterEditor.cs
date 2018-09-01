//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditor.SceneManagement;

//[CustomEditor(typeof(SupplyRequester))]
//public class SupplyRequesterEditor : Editor
//{
//    private SupplyRequester t;
//    private float minRadius;
//    private float maxRadius;
//    private bool editing;

//    private void OnEnable()
//    {
//        t = (SupplyRequester)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        editing = GUILayout.Toggle(editing, "Edit", "Button");

//        if (editing) SceneView.RepaintAll();

//        base.OnInspectorGUI();
//    }

//    private void OnSceneGUI()
//    {
//        minRadius = serializedObject.FindProperty("minRadius").floatValue;
//        maxRadius = serializedObject.FindProperty("maxRadius").floatValue;

//        if (editing)
//        {
//            minRadius = Handles.RadiusHandle(Quaternion.identity, t.transform.position, minRadius, true);
//            serializedObject.FindProperty("minRadius").floatValue = minRadius;

//            maxRadius = Handles.RadiusHandle(Quaternion.identity, t.transform.position, maxRadius, true);
//            serializedObject.FindProperty("maxRadius").floatValue = maxRadius;

//            serializedObject.ApplyModifiedProperties();

//            Handles.color = Color.green;
//            Handles.DrawWireDisc(t.transform.position, t.transform.up, maxRadius);

//            Handles.color = Color.red;
//            Handles.DrawWireDisc(t.transform.position, t.transform.up, minRadius);
//        }
//        else
//        {
//            Handles.color = Color.gray;
//            Handles.DrawWireDisc(t.transform.position, t.transform.up, maxRadius);
//            Handles.DrawWireDisc(t.transform.position, t.transform.up, minRadius);
//        }
//    }
//}