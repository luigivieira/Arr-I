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
/// Editor for the <c>AvoidBehaviour2D</c> class.
/// </summary>
[CanEditMultipleObjects]
[ExecuteInEditMode]
[CustomEditor(typeof(AvoidBehaviour2D))]
public class AvoidBehaviour2DEditor: BaseBehaviour2DEditor
{
    /// <summary>
    /// Method called when there were changes in the editor's fields via inspector
    /// (i.e. a property has been changed, for instance).
    /// </summary>
    override public void OnInspectorGUI()
    {
        displayTarget = false;
        base.OnInspectorGUI();
        SerializedProperty field;

        // ObstacleLayerMask
        field = serializedObject.FindProperty("obstacleLayerMask");
        EditorGUILayout.PropertyField(field);

        // SeeAheadDistance
        float seeAheadDistance = 0f;
        bool multipleValues = false;
        for(int i = 0; i < targets.Length; i++)
        {
            if(i == 0)
                seeAheadDistance = ((AvoidBehaviour2D) targets[i]).SeeAheadDistance;
            else if(seeAheadDistance != ((AvoidBehaviour2D) targets[i]).SeeAheadDistance)
                multipleValues = true;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = multipleValues;
        seeAheadDistance = EditorGUILayout.FloatField(new GUIContent("See Ahead Distance", "Distance for the object to look ahead in its direction and check for collisions"), seeAheadDistance);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "AvoidBehaviour2D - SeeAheadDistance");
            for(int i = 0; i < targets.Length; i++)
            {
                ((AvoidBehaviour2D) targets[i]).SeeAheadDistance = seeAheadDistance;
                EditorUtility.SetDirty(targets[i]);
            }
        }

        // RaycastRadius
        float raycastRadius = 0f;
        multipleValues = false;
        for(int i = 0; i < targets.Length; i++)
        {
            if(i == 0)
                raycastRadius = ((AvoidBehaviour2D) targets[i]).RaycastRadius;
            else if(raycastRadius != ((AvoidBehaviour2D) targets[i]).RaycastRadius)
                multipleValues = true;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = multipleValues;
        raycastRadius = EditorGUILayout.FloatField(new GUIContent("Raycast Radius", "Radius of the circle raycast to be used for detecting obstacles"), raycastRadius);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "AvoidBehaviour2D - RaycastRadius");
            for(int i = 0; i < targets.Length; i++)
            {
                ((AvoidBehaviour2D) targets[i]).RaycastRadius = raycastRadius;
                EditorUtility.SetDirty(targets[i]);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}