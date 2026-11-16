using UnityEngine;
using UnityEditor;

/// <summary>
/// Haxsen: Move objects to center of a referenced pivot.
/// </summary>

public class MyPivotToCenter : EditorWindow
{
    GameObject[] objs;
	Vector3 selectedObjectPivot;
    Mesh myMesh;
    public GameObject referenceObject;

    [MenuItem("GameObject/CentersToPivotPosition")]
    public static void ShowWindow(){
        GetWindow<MyPivotToCenter>(false, "CentersToPivotPosition", true);
    }

    void OnGUI(){
		GUILayout.Label("Make sure all objects pivots are same! \n1. Set the reference \n2. Select the objects in hierarchy\n3. Press the button to move them.",
                         EditorStyles.boldLabel);
		ScriptableObject myObj = this;
		SerializedObject myObjS = new SerializedObject(myObj);
		SerializedProperty property = myObjS.FindProperty("referenceObject");
		EditorGUILayout.PropertyField(property, new GUIContent("Reference Object:", "Object to align centers with."), true);
		myObjS.ApplyModifiedProperties();
        if (referenceObject) {
            
			if (GUILayout.Button(new GUIContent("Move", "Work the magic :D")))
			{
				MoveObjects();
			}
        }
    }

    void MoveObjects(){
        if(referenceObject){
            objs = Selection.gameObjects;
            myMesh = objs[1].GetComponent<MeshFilter>().sharedMesh;
            foreach (GameObject o in objs){
                Vector3 offset = o.transform.position - o.transform.TransformPoint(myMesh.bounds.center);
                o.transform.position = referenceObject.transform.position + offset;
            }
        }
    }
}
