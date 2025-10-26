using UnityEngine;
using UnityEditor;
using System.IO;

public class BoolGridEditor : EditorWindow
{
	private BoolGrid boolGrid;
	private int cellSize = 25;
	private string drawingName = "";
	private string savePath = "Assets/Scripts/ScriptableObjects";

	[MenuItem("Window/Bool Grid Editor")]
	public static void OpenWindow()
	{
		GetWindow<BoolGridEditor>("Bool Grid Editor");
	}

	private void OnGUI()
	{
		EditorGUILayout.Space();
		drawingName = EditorGUILayout.TextField("Grid Name:", drawingName);
		EditorGUILayout.Space();

		if (boolGrid == null)
		{
			if (GUILayout.Button("Create New BoolGrid"))
			{
				CreateNewGrid();
			}
			EditorGUILayout.HelpBox("Assign a BoolGrid asset to begin drawing.", MessageType.Info);
			return;
		}
			

		

		boolGrid.Initialize();

		

		EditorGUILayout.LabelField("Drawing:", EditorStyles.boldLabel);
		DrawGrid();
		EditorGUILayout.Space();

		if(GUILayout.Button("Save Changes"))
		{
			SaveAssetAsNew();
			CreateNewGrid();
		}
		if(GUILayout.Button("Clear Grid"))
		{
			CreateNewGrid();
		}
	}

	private void CreateNewGrid()
	{
		boolGrid = CreateInstance<BoolGrid>();
		boolGrid.width = 5;
		boolGrid.height = 5;
		boolGrid.Initialize();
	}

	private void SaveAssetAsNew()
	{
		if(!Directory.Exists(savePath))
			Directory.CreateDirectory(savePath);
		else
			Debug.LogWarning("Directory already exists.");

		BoolGrid newgrid = CreateInstance<BoolGrid>();
		newgrid.width = boolGrid.width;
		newgrid.height = boolGrid.height;
		newgrid.cells = (bool[])boolGrid.cells.Clone();

		string assetPath = $"{savePath}/BoolGrid_{drawingName}.asset";
		if (File.Exists(assetPath))
		{
			Debug.LogError($"An asset with the name 'BoolGrid_{drawingName}.asset' already exists at the specified path. Please choose a different name.");

			return;
		}
		AssetDatabase.CreateAsset(newgrid, assetPath);
		AssetDatabase.SaveAssets();
		Debug.Log($"BoolGrid saved as new asset at: {assetPath}");
	}

	private void DrawGrid()
	{
		Event e = Event.current;

		for(int y = 0; y < boolGrid.height; y++)
		{
			GUILayout.BeginHorizontal();
			for(int x = 0; x < boolGrid.width; x++)
			{
				bool currentValue = boolGrid.GetCell(x, y);
				Rect cellRect = GUILayoutUtility.GetRect(cellSize, cellSize);
				EditorGUI.DrawRect(cellRect, currentValue ? Color.mediumPurple : Color.gray);

				Handles.color = Color.black;
				Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMin), new Vector2(cellRect.xMax, cellRect.yMin));
				Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMin), new Vector2(cellRect.xMin, cellRect.yMax));
				Handles.DrawLine(new Vector2(cellRect.xMax, cellRect.yMin), new Vector2(cellRect.xMax, cellRect.yMax));
				Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMax), new Vector2(cellRect.xMax, cellRect.yMax));


				if (e.type == EventType.MouseDown && cellRect.Contains(e.mousePosition))
				{
					boolGrid.SetCell(x, y, !currentValue);
					Repaint();
				}
				//GUI.color = originalColor;
			}
			GUILayout.EndHorizontal();
		}
	}
}
