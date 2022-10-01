using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTextCanvas : MonoBehaviour
{
    public static DebugTextCanvas Instance;

    public GameObject dbTextGO;

    private Dictionary<string, TextMeshProUGUI> dbTextDict = new Dictionary<string, TextMeshProUGUI>();
    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;



            transform.SetParent(null);
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }

    public void _SetDbText(string entryName, string entryText)
    {
        if (dbTextDict.ContainsKey(entryName))
        {
            dbTextDict[entryName].SetText(entryText);
        }
        else
        {
            GameObject tempgo = Instantiate(dbTextGO, this.transform);
            TextMeshProUGUI tmpugui = tempgo.GetComponent<TextMeshProUGUI>();
            tmpugui.SetText(entryText);
            dbTextDict.Add(entryName, tmpugui);
            tempgo.transform.position = (Vector2)tempgo.transform.position + (Vector2.down * 50f * dbTextDict.Count);
        }
    }
    public static void SetDbText(string entryName, string entryText)
    {
        Instance._SetDbText(entryName, entryText);
    }
}
