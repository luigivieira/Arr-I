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
/// Editor for the <c>SteeringManager2D</c> class.
/// </summary>
[CanEditMultipleObjects]
[ExecuteInEditMode]
[CustomEditor(typeof(SteeringManager2D))]
public class SteeringManager2DEditor: Editor
{
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
            active = active || ((SteeringManager2D) targets[i]).Active;

        EditorGUI.BeginChangeCheck();
        active = EditorGUILayout.Toggle("Active", active);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "SteeringManager2D - Active");
            for(int i = 0; i < targets.Length; i++)
            {
                ((SteeringManager2D) targets[i]).Active = active;
                EditorUtility.SetDirty(targets[i]);
            }
        }

        // UpdateMode
        field = serializedObject.FindProperty("updateMode");
        EditorGUILayout.PropertyField(field);

        // MaxSpeed
        float maxSpeed = 0f;
        bool multipleValues = false;
        for(int i = 0; i < targets.Length; i++)
        {
            if(i == 0)
                maxSpeed = ((SteeringManager2D) targets[i]).MaxSpeed;
            else if(maxSpeed != ((SteeringManager2D) targets[i]).MaxSpeed)
                multipleValues = true;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = multipleValues;
        maxSpeed = EditorGUILayout.FloatField("Max Speed", maxSpeed);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "SteeringManager2D - MaxSpeed");
            for(int i = 0; i < targets.Length; i++)
            {
                ((SteeringManager2D) targets[i]).MaxSpeed = maxSpeed;
                EditorUtility.SetDirty(targets[i]);
            }
        }

        // Mass
        float mass = 0f;
        multipleValues = false;
        for(int i = 0; i < targets.Length; i++)
        {
            if(i == 0)
                mass = ((SteeringManager2D) targets[i]).Mass;
            else if(mass != ((SteeringManager2D) targets[i]).Mass)
                multipleValues = true;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = multipleValues;
        mass = EditorGUILayout.FloatField("Mass", mass);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "SteeringManager2D - Mass");
            for(int i = 0; i < targets.Length; i++)
            {
                ((SteeringManager2D) targets[i]).Mass = mass;
                EditorUtility.SetDirty(targets[i]);
            }
        }

        // Rotate
        field = serializedObject.FindProperty("rotate");
        EditorGUILayout.PropertyField(field);

        // Timescale independent
        field = serializedObject.FindProperty("timescaleIndependent");
        EditorGUILayout.PropertyField(field);

        serializedObject.ApplyModifiedProperties();
    }
}