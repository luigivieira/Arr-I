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
/// Editor for the <c>EschewBehaviour2D</c> class.
/// </summary>
[CanEditMultipleObjects]
[ExecuteInEditMode]
[CustomEditor(typeof(EschewBehaviour2D))]
public class EschewBehaviour2DEditor: BaseBehaviour2DEditor
{
    /// <summary>
    /// Method called when there were changes in the editor's fields via inspector
    /// (i.e. a property has been changed, for instance).
    /// </summary>
    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // SafeRadius
        float safeRadius = 0f;
        bool multipleValues = false;
        for(int i = 0; i < targets.Length; i++)
        {
            if(i == 0)
                safeRadius = ((EschewBehaviour2D) targets[i]).SafeRadius;
            else if(safeRadius != ((EschewBehaviour2D) targets[i]).SafeRadius)
                multipleValues = true;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = multipleValues;
        safeRadius = EditorGUILayout.FloatField(new GUIContent("Safe Radius", "Radius of a circle around the target in which the object will try to stay out"), safeRadius);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "EschewBehaviour2D - SafeRadius");
            for(int i = 0; i < targets.Length; i++)
            {
                ((EschewBehaviour2D) targets[i]).SafeRadius = safeRadius;
                EditorUtility.SetDirty(targets[i]);
            }
        }
    }
}