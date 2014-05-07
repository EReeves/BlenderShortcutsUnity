using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Text;

public class WinAPIUnity
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    internal static extern short GetKeyState(int virtualKeyCode);

    [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern void SendInput(byte vk, byte scan, int flags, int extrainfo);

    [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
    public static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    static List<int> down = new List<int>();

    public static bool IsKeyDownOnce(int keycode)
    {
        if (down.Contains(keycode))
            return false;

        if (GetKeyState(keycode) == -127 || GetKeyState(keycode) == -128)
        {
            down.Add(keycode);
            return true;
        }
        else
            return false;
    }
    //Call this after this ^
    public static void UpReset(int[] keycode)
    {
        foreach (int i in keycode)
        {
            int result = GetKeyState(i);
            if (result == 0 || result == 1)
                down.Remove(i);
        }
    }

    public static void UpReset(int keycode)
    {
        int result = GetKeyState(keycode);
        if (result == 0 || result == 1)
            down.Remove(keycode);
    }

    public static bool IsKeyDown(int keycode)
    {
        int result = GetKeyState(keycode);
        if (result == -127 || result == -128 )
        {
            down.Add(keycode);
            return true;
        }
        else
            return false;
    }

    public static bool KeyUp(int keycode)
    {
        int result = GetKeyState(keycode);
        if (result == 0 || result == 1)
            return true;
        else
            return false;
    }

    public static bool UnityActive()
    {
        IntPtr ptr;
        try { ptr = GetActiveWindow(); }
        catch { return false; } //No active window.
        StringBuilder buffer = new StringBuilder(32);
        GetWindowText(ptr, buffer, 32);
        string result = buffer.ToString();
        if (result.StartsWith("UNITY", true, System.Globalization.CultureInfo.CurrentCulture))
            return true;
        else return false;
    }
}