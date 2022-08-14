using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Drawingterminal : EditorWindow {
    [MenuItem("Tools/Draw Map")]
    public static void ShowWindow() {
        GetWindow(typeof(Drawingterminal));
    }

    Dictionary<Vector2Int, bool> clickedtiles = new();
    MapContainer source = null;

    public string data;
    string StringName = "New Map";

    int width = 100;
    int height = 100;

    void OnGUI() {
        GUILayout.Label("Create Map", EditorStyles.boldLabel);

        //source = EditorGUILayout.ObjectField("Drop to Load", source, typeof(MapContainer), true) as MapContainer;

        GUILayout.Space(20);
        
        if (source != null) {
            //LoadMap(source.GetComponent<MapContainer>().Map);
            source = null;
        }

        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);

        GUILayout.Space(20);

        StringName = EditorGUILayout.TextField("Map Name", StringName);
        if(GUILayout.Button("Export Map")) {
            ExportMap();
        }

        GUILayout.Space(20);

        if (width * height != clickedtiles.Count)
            Reset(width, height);

        CreateGridOnGUI(width, height);
    }

    public void LoadMap(List<Vector2Int> map) {
        Reset(width, height);
        foreach (var item in map) {
            clickedtiles[item] = true;
        }
    }

    public void ExportMap() {
        List<Vector2Int> Map = new();
        foreach (var item in clickedtiles) {
            if (item.Value) {
                Map.Add(item.Key);
            }
        }

        if (!Directory.Exists("Assets/Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        string localPath = "Assets/Prefabs/" + StringName + ".prefab";

        var gameobject = new GameObject();
        //gameobject.AddComponent<MapContainer>().Map = Map;

        var pf = PrefabUtility.SaveAsPrefabAssetAndConnect(gameobject, localPath, InteractionMode.UserAction, out bool prefabSuccess);
        if (prefabSuccess == true) 
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);

        DestroyImmediate(gameobject);
    }

    public void CreateGridOnGUI(int width, int height) {
        for (int y = 0; y < height; y++) {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < width; x++) {
                if (!clickedtiles[new Vector2Int(x, y)])
                    GUI.color = Color.black;
                else
                    GUI.color = Color.white;

                if (GUILayout.Button("", GUILayout.Width(20))) {
                    clickedtiles[new Vector2Int(x, y)] = !clickedtiles[new Vector2Int(x, y)];
                }
            }
            GUILayout.EndHorizontal();
        }
    }

    void Reset(int width, int height) {
        clickedtiles.Clear();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                clickedtiles.Add(new Vector2Int(x, y), false);
            }
        }
    }

    public void ChangeState() {
        if (GUI.color == Color.white)
            GUI.color = Color.black;
        else
            GUI.color = Color.white;
    }
}