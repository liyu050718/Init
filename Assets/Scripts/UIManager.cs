using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("此处拖入画布游戏物体")]
    public GameObject canvas;
    public Dictionary<string,GameObject> panels = new Dictionary<string,GameObject>();
    [Header("ui组件存放的地址")]
    public string UIAddress = "Prefabs/UI";
    private void Start()
    {
        GameObject[] ui = Resources.LoadAll<GameObject>(UIAddress);
        foreach (GameObject go in ui)
        {
            GameObject u = Instantiate(go);
            u.transform.SetParent(canvas.transform);
            panels[u.name] = u;
            u.SetActive(false);
        }
    }
    public void OpenPanelOnly(string name)
    {
        ClosePanelAll();
        OpenPanel(name);
    }
    public void OpenPanel(string name)
    {
        panels[name].SetActive(true);
    }
    public void ClosePanel(string name)
    {
        panels[name].SetActive(false);
    }
    public void ClosePanelAll()
    {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            ClosePanel(canvas.transform.GetChild(i).name);
        }
    }
    public void StopTime()
    {
        Time.timeScale = 0;
    }
}
