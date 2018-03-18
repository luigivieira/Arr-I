/*
 * This file is part of the Arr-I project. The complete source code is
 * available at https://github.com/luigivieira/Arr-I.
 *
 * Copyright (c) 2018, Luiz Carlos Vieira (http://www.luiz.vieira.nom.br)
 *
 * MIT License
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor for the <c>BaseBehaviour2D</c> class.
/// </summary>
[CanEditMultipleObjects]
[ExecuteInEditMode]
[CustomEditor(typeof(BaseBehaviour2D), true)]
public class BaseBehaviour2DEditor: Editor
{
    /// <summary>
    /// Indicates if the target configuration should be displayed in this editor.
    /// The default is true, but this can be set to false by subclasses if a 
    /// target is not needed for a specific behaviour.
    /// </summary>
    protected bool displayTarget = true;

    /// <summary>
    /// Method called when there were changes in the editor's fields via inspector
    /// (i.e. a property has been changed, for instance).
    /// </summary>
    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty field;

        // Debug
        field = serializedObject.FindProperty("debug");
        EditorGUILayout.PropertyField(field);

        // Active
        bool active = false;
        for(int i = 0; i < targets.Length; i++)
            active = active || ((BaseBehaviour2D) targets[i]).Active;

        EditorGUI.BeginChangeCheck();
        active = EditorGUILayout.Toggle(new GUIContent("Active", "Turns on/off the influence of this steering behaviour"), active);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "BaseBehaviour2D - Active");
            for(int i = 0; i < targets.Length; i++)
            {
                ((BaseBehaviour2D) targets[i]).Active = active;
                EditorUtility.SetDirty(targets[i]);
            }
        }

        // Target
        if(displayTarget)
        {
            field = serializedObject.FindProperty("target");
            EditorGUILayout.PropertyField(field);
        }

        // weight
        float weight = 0f;
        bool multipleValues = false;
        for(int i = 0; i < targets.Length; i++)
        {
            if(i == 0)
                weight = ((BaseBehaviour2D) targets[i]).Weight;
            else if(weight != ((BaseBehaviour2D) targets[i]).Weight)
                multipleValues = true;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = multipleValues;
        weight = EditorGUILayout.Slider(new GUIContent("Weight","Defines the relative weight (importance) of this behaviour in relation to other behaviours"), weight, 0f, 1f);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "BaseBehaviour2D - Weight");
            for(int i = 0; i < targets.Length; i++)
            {
                ((BaseBehaviour2D) targets[i]).Weight = weight;
                EditorUtility.SetDirty(targets[i]);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}