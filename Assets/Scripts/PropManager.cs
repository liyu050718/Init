using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PropManager : MonoSingleton<PropManager>
{
    public string address = "Textures/PropUi";
    public GameObject parent;
    public Item currentItem;
    public int currentId = -1;
    public List<Item> items = new List<Item>();
    public Dictionary<string, Texture2D> ui = new Dictionary<string, Texture2D>();
    public void Start()
    {
        parent = GameObject.Find("Content");
        Texture2D[] textures= Resources.LoadAll<Texture2D>(address);
        foreach (Texture2D tex in textures)
        {
            Debug.Log(tex.name);
            ui[tex.name] = tex;
        }
    }
    public void Pick(Item item)
    {
        Debug.Log("ºÒµΩ¡À" + item);
        //AutoClosePickup.Instance.GetComponent<AutoClosePickup>().PickupItem();
        items.Add(item);
        UpdateUI();
    }
    public void Discard(Item item)
    {
        currentId = 0;
        currentItem = items[0];
        items.Remove(item);
        UpdateUI();
    }

    public void UpdateUI()
    {
        for(int i = 0;i<6;i++)
        {
            parent.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = null;
              
        }
        for(int i = 0;i<items.Count;i++)
        {
            Texture2D tex = ui[items[i].ToString()];
            parent.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(
                tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        }
    }
    public void Switch(int id)
    {

        if(id == 0)
        {
            if(currentId != -1)
                parent.transform.GetChild(currentId).transform.localScale /= 1.2f;
            Debug.Log("hahah");
            parent.transform.GetChild(0).transform.localScale *= 1.2f;
            currentId = 0;
            currentItem = items[0];
            //parent.transform.parent.transform.GetChild(id).transform.localScale *= 1.2f;
        }
        else if (id > items.Count)
        {
            if (currentId != -1)
                parent.transform.GetChild(currentId).transform.localScale /= 1.2f;
            parent.transform.GetChild(id).transform.localScale *= 1.2f;
            currentId = id;
        }
        else
        {
            currentItem = items[id];
            parent.transform.GetChild(currentId).transform.localScale /= 1.2f;
            currentId = id;
            parent.transform.GetChild(id).transform.localScale *= 1.2f;
        }
    }
}
