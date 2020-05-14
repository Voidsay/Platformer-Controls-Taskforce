/*
 * This scipt was created for the Platformer Controls Taskforce Project.
 * It is used to manage player input.
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
 * - controls for two players
 * - dose almost the same things as Input
 * - loads preferences from pref file or uses default
 * 
 * Flaws:
 * - no warning for double assigned keys
 * - no alternative keys supported
 * - controller support limited
 */
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class InputManager : MonoBehaviour
{
    Dictionary<string, KeyCode> buttonKeys;


    void OnEnable()
    {
        buttonKeys = new Dictionary<string, KeyCode>();

        //P1
        buttonKeys["P1Left"] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Left", KeyCode.A.ToString()));
        buttonKeys["P1Right"] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Right", KeyCode.D.ToString()));
        buttonKeys["P1Jump"] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Jump", KeyCode.W.ToString()));
        buttonKeys["P1Crouch"] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Crouch", KeyCode.S.ToString()));

        //P2
        buttonKeys["P2Left"] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Left", KeyCode.LeftArrow.ToString()));
        buttonKeys["P2Right"] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Right", KeyCode.RightArrow.ToString()));
        buttonKeys["P2Jump"] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Jump", KeyCode.UpArrow.ToString()));
        buttonKeys["P2Crouch"] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Crouch", KeyCode.DownArrow.ToString()));

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetButtonDown(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.LogError("No button named: " + buttonName);
            return false;
        }
        return Input.GetKeyDown(buttonKeys[buttonName]);
    }

   /* public float GetAxisRaw(string buttonName) // there is no good solution for this, avoid if possible
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.LogError("No button named: " + buttonName);
            return 0f;
        }
        return Input.GetAxisRaw(buttonKeys[buttonName]);
    }*/

    public bool GetButton(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.LogError("No button named: " + buttonName);
            return false;
        }
        return Input.GetKey(buttonKeys[buttonName]);
    }
    public bool GetButtonUp(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.LogError("No button named: " + buttonName);
            return false;
        }
        return Input.GetKeyUp(buttonKeys[buttonName]);
    }

    public string[] GetButtonNames()
    {
        return buttonKeys.Keys.ToArray();
    }

    public string GetKeyName(string buttonName)
    {
        if (buttonKeys.ContainsKey(buttonName) == false)
        {
            Debug.LogError("No button named: " + buttonName);
            return "N/A";
        }
        return buttonKeys[buttonName].ToString();
    }

    public void SetKeyTo(string buttonName, KeyCode keyCode)
    {
        buttonKeys[buttonName] = keyCode;
    }
}
