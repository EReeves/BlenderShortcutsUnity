using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

/* Blender Shortcuts for Unity.
 * Only works on Windows because Unity sucks.
 * Author: Evan Reeves
 * 08-05-2014
 */

[InitializeOnLoad]
public class BlenderShortcuts : EditorWindow {

    private static bool panning  = false;

	static BlenderShortcuts()
    {
        SetPreferences();
        EditorApplication.update += EditorUpdate;
    }

    [MenuItem("Window/Blender Shortcuts/Enable")]
    private static void Enable()
    {
        EditorApplication.update -= EditorUpdate;
        EditorApplication.update += EditorUpdate; 
    }

    [MenuItem("Window/Blender Shortcuts/Disable")]
    private static void Disable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    private static void SetPreferences()
    {
        if (EditorPrefs.GetString("Tools/Rotate") != "Tools/Rotate;R")
            Debug.LogWarning("You need to restart Unity for the Blender Rot/Scale/Move binds to work.");

        EditorPrefs.SetString("Tools/Rotate","Tools/Rotate;R");
        EditorPrefs.SetString("Tools/Scale","Tools/Scale;S");
        EditorPrefs.SetString("Tools/Move","Tools/Move;G");
        EditorPrefs.SetString("Tools/View", "Tools/View;#Q");
        //Get this out of the way, no one uses it anyway.
        EditorPrefs.SetString("Tools/Pivot Rotation", "Tools/Pivot Rotation;^X");
    }

    private static void EditorUpdate()
    {
        if (!WinAPIUnity.UnityActive())
            return;

        //Shift - pan
        if (!panning && WinAPIUnity.IsKeyDown(0x10))
        {
            WinAPIUnity.SendInput(0x51, 0x10, 0x0001 | 0, 0);
            panning = true;
        }
        else if(panning && WinAPIUnity.KeyUp(0x10))
        {
            //Undo
            WinAPIUnity.SendInput(0x47, 0x71, 0x0001 | 0, 0);
            panning = false;
        }
        WinAPIUnity.UpReset(0x10);
            
        //Shift - Modifier
        if(WinAPIUnity.IsKeyDown(0x10))
            ShiftCombinations();

        //X - Delete.
        if (WinAPIUnity.IsKeyDownOnce(88) || WinAPIUnity.IsKeyDownOnce(120))
        {
            //Send delete.
            WinAPIUnity.SendInput(0x2E, 0x53, 0x0001 | 0, 0);
        }
        WinAPIUnity.UpReset(new int[2] {88, 120});
    }

    private static void ShiftCombinations()
    {
        //D - Duplicate.
        if (WinAPIUnity.IsKeyDownOnce(68) || WinAPIUnity.IsKeyDownOnce(100))
        {
            //Fuck finding the codes for two keys. Duplicate manually.
            foreach (var obj in Selection.gameObjects)
            {
                var inst = Instantiate(obj);
                inst.name = obj.name;
            }
        }
        WinAPIUnity.UpReset(new int[2]{68,100});
    }
}
