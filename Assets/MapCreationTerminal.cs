using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MapCreationTerminal : MonoBehaviour
{
    [Header("References")]
    public GameObject ButtonPrefab;
    public Image ButtonParent;

    public TMP_InputField widthInput;
    public TMP_InputField heightInput;
    public TMP_InputField nameInput;
    public TMP_InputField fileLocationInput;

    public TMP_Text infoText;

    //Private Vars
    Dictionary<Vector2Int, bool> Clickedtiles = new();
    List<GameObject> Buttons = new();

    string fileLocation = "C:/";
    string fileName = "New Map";

    int width = 10;
    int height = 10;
    int boxSize = 0;

    void Start() {
        widthInput.text = width.ToString();
        heightInput.text = height.ToString();

        fileLocationInput.text = fileLocation;

        boxSize = Mathf.RoundToInt(ButtonParent.rectTransform.rect.width) - 80;
    }

    void Update() {
        if (int.Parse(widthInput.text).GetType() == typeof(int))
            width = int.Parse(widthInput.text); 
        
        if (int.Parse(heightInput.text).GetType() == typeof(int))
            height = int.Parse(heightInput.text);

        fileName = nameInput.text;
        fileLocation = fileLocationInput.text;
    }

    private void SpawnButtons(int width, int height) {
        int buttonSize = 0;
        if (width > height)
            buttonSize = boxSize / width;
        else
            buttonSize = boxSize / height;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                RectTransform tmp = Instantiate(ButtonPrefab).GetComponent<RectTransform>();
                tmp.transform.SetParent(ButtonParent.transform);

                tmp.sizeDelta = new Vector2(buttonSize, buttonSize);
                tmp.anchoredPosition = new Vector2((-boxSize / 2 + buttonSize / 2) + x * buttonSize, (boxSize / 2 - buttonSize / 2) - y * buttonSize);

                var button = tmp.GetComponent<Button>(); 
                button.image.color = Color.gray;
                button.onClick.AddListener(delegate { ChangeState(button); });

                button.GetComponent<ButtonInfoHolder>().gridPos = new Vector2Int(x, y);

                Buttons.Add(tmp.gameObject);
            }
        }
    }

    public void CreateGrid() {
        Clickedtiles.Clear();

        while (Buttons.Count > 0) {
            var button = Buttons[0].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            Destroy(Buttons[0]);
            Buttons.RemoveAt(0);
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Clickedtiles.Add(new Vector2Int(x, y), false);
            }
        }

        SpawnButtons(width, height);
    }

    public void ChangeState(Button button) {
        var gridpos = button.GetComponent<ButtonInfoHolder>().gridPos;

        if (button.image.color == Color.white) {
            button.image.color = Color.gray;
            Clickedtiles[gridpos] = false;
        }
        else {
            button.image.color = Color.white;
            Clickedtiles[gridpos] = true;
        }
    }

    public void ExportMap() {
        if(fileName == "") {
            infoText.text = "Please enter a Name";
            return;
        }

        List<Vector2Int> Map = new();
        foreach (var item in Clickedtiles) {
            if (item.Value) {
                Map.Add(item.Key);
            }
        }

        if(Map.Count < 1 || Clickedtiles.Count < 1) {
            infoText.text = "Grid Cannot be Empty";
            return;
        }

        MapContainer MapContainer = new();
        MapContainer.Map = Map;

        string json = JsonUtility.ToJson(MapContainer); 
        string filename = Path.Combine(fileLocation + fileName + ".json");
        if (File.Exists(filename)) {
            File.Delete(filename);
        }
        File.WriteAllText(filename, json);

        infoText.text = "Json File Successfully Created";
        Debug.Log("Json file succesfully Created");
    }
}
