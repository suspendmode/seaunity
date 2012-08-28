
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Test for menu;
/// </summary>
public class MyGUITest
{
    private static MyGUIBehavior myGuiBehavior;
    private static void AttachMyGUIBehavior()
    {
        GameObject gameObject = GameObject.Find("_Config");
        myGuiBehavior = gameObject.GetComponent<MyGUIBehavior>();
        if (myGuiBehavior == null)
            myGuiBehavior = gameObject.AddComponent<MyGUIBehavior>();
    }

    public delegate void OnGUICallback();

    private static int layerID = 0;

    [MenuItem("Debug/GUITest")]
    static void GUITest()
    {
        AttachMyGUIBehavior();
        GUITest myGUI = new GUITest();
        myGUI.AddGUICall(DrawWindow2, MyGUI.GUICallPriority.top);
        myGUI.AddGUICall(DrawWindow3, MyGUI.GUICallPriority.top);
        myGUI.AddGUICall(DrawWindow4, MyGUI.GUICallPriority.top);
        myGUI.AddGUICall(myGUI.DrawWindow, MyGUI.GUICallPriority.background);

        MyGUIBehavior.Instance.AddMyGUI(layerID++, myGUI);
    }
    
    [MenuItem("Debug/RemoveGUITest")]
    static void RemoveGUITest()
    {
        AttachMyGUIBehavior();

        for(int i = 0; i < layerID; i++)
        {
            MyGUI myGUI = MyGUIBehavior.Instance.GetMyGUI(i);
            if (myGUI == null)
                continue;

            if(myGUI is GUITest)
            {
                GUITest guiTest = (GUITest) myGUI;
                myGUI.RemoveGUICall(guiTest.DrawWindow, MyGUI.GUICallPriority.background);
            }
        }
    }

    [MenuItem("Debug/RemoveDrawWindow2")]
    static void RemoveDrawWindow2()
    {
        AttachMyGUIBehavior();

        for (int i = 0; i < layerID; i++)
        {
            MyGUI myGUI = MyGUIBehavior.Instance.GetMyGUI(i);
            if(myGUI == null)
                continue;
            
            myGUI.RemoveGUICall(DrawWindow2, MyGUI.GUICallPriority.top);
            myGUI.RemoveGUICall(DrawWindow3, MyGUI.GUICallPriority.top);
            myGUI.RemoveGUICall(DrawWindow4, MyGUI.GUICallPriority.top);
        }
    }

    public static void DrawWindow2()
    {
        UI.DrawWindow(new Rect(Screen.width * 0.5f - 100f, Screen.height * 0.5f - 120f, 200f, 240f), "Hao DrawWindow2");
    }

    public static void DrawWindow3()
    {
        UI.DrawWindow(new Rect(Screen.width * 0.5f + 100f, Screen.height * 0.5f + 120f, 150f, 150f), "Hao DrawWindow3");
    }

    public static void DrawWindow4()
    {
        UI.DrawWindow(new Rect(10f, 10f, 190f, 190f), "Hao DrawWindow4");
    }
}

/// <summary>
/// Test for MyGUI;
/// </summary>
public class GUITest : MyGUI
{
    public void DrawWindow()
    {
        Debug.Log("Hao Test  --------> DrawWindow()");
        Rect rect = UI.DrawWindow(new Rect(Screen.width * 0.5f - 200f, Screen.height * 0.5f - 220f, 400f, 440f), "Hao TTTTT");
    }
}