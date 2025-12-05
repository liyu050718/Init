using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject Boss;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Boss.GetComponent<Boss>().TakeDamage(Item.圆规);
            Boss.GetComponent<Boss>().TakeDamage(Item.消防斧);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PropManager.Instance.Pick(Item.日记);
        }
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            PropManager.Instance.Switch(0);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            PropManager.Instance.Switch(1);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            PropManager.Instance.Switch(2);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad4))
        {
            PropManager.Instance.Switch(3);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            PropManager.Instance.Switch(4);
        }
        else if(Input.GetKeyDown(KeyCode.Keypad6))
        {
            PropManager.Instance.Switch(5);
        }
    }

    
}
