using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

public class RefreshMaterialsTool : EditorWindow
{
    private List<Material> _materials = new List<Material>();
    private bool _inspectMaterials;
    private int _index;
    
	[MenuItem("Tools/Refresh Materials Tool", false, 4)]	
	static void Init()
	{
        // Get existing open window or if none, make a new one:
        var window = (SetupMaterialsAndTextures)EditorWindow.GetWindow(typeof(SetupMaterialsAndTextures));
        window.title = "Setup Mat and Tex";
        window.minSize = new Vector2(550, 370);
        window.maxSize = new Vector2(550, 370);
		//window.position = new Rect(Screen.width/2, Screen.height/2, 515, 155);
        window.autoRepaintOnSceneChange = true;
        window.Show();
        window.Repaint();
	}

    private void OnInspectorUpdate()
    {
		Repaint();
	}

    private void OnGUI()
    {
        GUILayout.Space(5);
        
        GUILayout.Label("Select any child or parent object and press the button below.\n Supports Hierarchy, Project view and any kind of asset.", EditorStyles.boldLabel);
        
        GUILayout.Space(5);
        
        GUILayout.Label("Refresh selected objects materials:");
        
        if (GUILayout.Button("Refresh Materials"))
        {
            GetMaterials();
        }
        
        if(_inspectMaterials)
        {
            InspectMaterials();
        }
    }

    private void GetMaterials()
    {
        var sObjs = Selection.objects;

        if (sObjs == null)
        {
            Debug.Log("No action taken, nothing was selected");
            return;
        }

        var dObjs = EditorUtility.CollectDependencies(sObjs);
        
        _inspectMaterials = false;
        _index = 0;
        _materials.Clear();
        foreach (var o in dObjs)
        {
            if (!(o is Material mat)) continue;
            _materials.Add(mat);
            _inspectMaterials = true;
        }
    }

    private void InspectMaterials()
    {
        Selection.activeObject = _materials[_index];
        
        InternalEditorUtility.SetIsInspectorExpanded(_materials[_index], true);//works on any object, not just materials
        
        _index++;
        _index = Mathf.Clamp(_index, 0, _materials.Count - 1);
        
        if (_index != _materials.Count - 1) return;
        
        _inspectMaterials = false;
        Selection.activeObject = null;
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}