using UnityEngine;
using UnityEditor;
using System.Collections;

public static class MyShortcuts {
    const string ShortcutKey = "&p";

    [MenuItem("My Commands/Select Parent " + ShortcutKey)]
    static void SelectParent() {
        Selection.activeGameObject = Selection.activeGameObject.transform.parent.gameObject;
    }
}