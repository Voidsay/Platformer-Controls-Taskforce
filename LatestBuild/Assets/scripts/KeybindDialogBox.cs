/*
 * This scipt was created for the Platformer Controls Taskforce Project.
 * It is used to setup a basic menu, fill it with all options and modify and save them
 * 
 *  * Project home: https://github.com/Voidsay/Platformer-Controls-Taskforce
 * 
 * Copyright: 
 * MIT
 * Copyright (C) 2012-2014  Martin "quill18" Glaude
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * Contributors:
 * - Martin Glaude (quill18) base code: quill18.com/unity_tutorials/Tutorial-KeyRebind.zip see also: https://www.youtube.com/watch?v=HkmP7raUYi0
 * - Voidsay
 * 
 * Last update: 
 * 05/14/2020
 * 
 * Features:
 * - setup controls menu
 * - updates after change
 * 
 * Flaws:
 * - always saves after change, may cause stutters on bad harddrives
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class KeybindDialogBox : MonoBehaviour
{
    public InputManager inputManager;
    public GameObject item;
    public GameObject list;

    string buttonToRebind = null;
    Dictionary<string, Text> buttonToLabel;

    // Start is called before the first frame update
    void Start()
    {
        string[] buttonNames = inputManager.GetButtonNames();
        buttonToLabel = new Dictionary<string, Text>();

        //foreach (string bn in buttonNames)
        for (int i = 0; i < buttonNames.Length; i++)
        {
            string bn;
            bn = buttonNames[i];
            GameObject thisItem;
            thisItem = (GameObject)Instantiate(item, list.transform);

            Text nameText = thisItem.transform.Find("Label").GetComponent<Text>();
            nameText.text = bn;

            Text buttonText = thisItem.transform.Find("Button/Text").GetComponent<Text>();
            buttonText.text = inputManager.GetKeyName(bn);
            buttonToLabel[bn] = buttonText;

            Button bindingButton = thisItem.transform.Find("Button").GetComponent<Button>();
            bindingButton.onClick.AddListener(() => StartRebindFor(bn));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonToRebind != null)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kc))
                    {
                        inputManager.SetKeyTo(buttonToRebind, kc);
                        buttonToLabel[buttonToRebind].text = kc.ToString();
                        PlayerPrefs.SetString(buttonToRebind, kc.ToString());
                        PlayerPrefs.Save();//if you fear crash or don't quit with Application.Quit();
                        buttonToRebind = null;
                        break;
                    }
                }

            }
        }
    }

    void StartRebindFor(string buttonNumber)
    {
        Debug.Log(buttonNumber);
        buttonToRebind = buttonNumber;
    }
}
