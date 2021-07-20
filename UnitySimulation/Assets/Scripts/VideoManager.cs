using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VideoManager : MonoBehaviour
{
    private const int WINDOW_OFFSET = 10;
    private const int RESIZE_BUTTON_SIZE = 20;
    private const int WINDOW_ID = 0;

    private readonly Vector2 MIN_WINDOW_SIZE = new Vector2(300, 150);
    private readonly Rect DRAG_RECT_DIMS = new Rect(0, 0, 10000, 10000);

    private Rect windowRect = new Rect(20, 20, 640, 480);
    private bool isResizing = false;


    //Called when rendering Gui
    private void OnGUI()
    {
        windowRect = GUI.Window(WINDOW_ID, windowRect, CreateWindowContent, GetImageFromStream());
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            isResizing = false;

        if (isResizing)
        {
            windowRect.width = Mathf.Max(Input.mousePosition.x - windowRect.x + WINDOW_OFFSET, MIN_WINDOW_SIZE.x);
            windowRect.height = Mathf.Max(((Screen.height - Input.mousePosition.y) - windowRect.y + WINDOW_OFFSET), MIN_WINDOW_SIZE.y);
        }
    }

    private void CreateWindowContent(int windowID)
    {
        //window Resizable
        if (GUI.RepeatButton(new Rect(windowRect.width - RESIZE_BUTTON_SIZE, windowRect.height - RESIZE_BUTTON_SIZE, RESIZE_BUTTON_SIZE, RESIZE_BUTTON_SIZE), ""))
            isResizing = true;

        //Window draggable
        GUI.DragWindow(DRAG_RECT_DIMS);
    }

    private Texture GetImageFromStream()
    {
        Texture2D texture = new Texture2D((int)windowRect.width + 50, (int)windowRect.height + 50);
        try
        {
            ImageConversion.LoadImage(texture, UDPManager.Instance.RecievedData);
        }
        catch { }
        return texture;
    }
}
