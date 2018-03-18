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
/// Editor for the <c>PursuitBehaviour2D</c> class.
/// </summary>
[CanEditMultipleObjects]
[ExecuteInEditMode]
[CustomEditor(typeof(PursuitBehaviour2D), true)]
public class PursuitBehaviour2DEditor: BaseBehaviour2DEditor
{
    /// <summary>
    /// Method called when there were changes in the editor's fields via inspector
    /// (i.e. a property has been changed, for instance).
    /// </summary>
    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // MaxFutureSteps
        int maxFutureSteps = 0;
        bool multipleValues = false;
        for(int i = 0; i < targets.Length; i++)
        {
            if(i == 0)
                maxFutureSteps = ((PursuitBehaviour2D) targets[i]).MaxFutureSteps;
            else if(maxFutureSteps != ((PursuitBehaviour2D) targets[i]).MaxFutureSteps)
                multipleValues = true;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = multipleValues;
        maxFutureSteps = EditorGUILayout.IntField(new GUIContent("Max Future Steps", "Maximum number of time steps (in seconds) for the prediction of the target position (0 means no prediction, hence equals to the Seek behaviour)"), maxFutureSteps);
        if(EditorGUI.EndChangeCheck())
        {
            maxFutureSteps = Mathf.Max(0, maxFutureSteps);
            Undo.RecordObjects(targets, "PursuitBehaviour2D - MaxFutureSteps");
            for(int i = 0; i < targets.Length; i++)
            {
                ((PursuitBehaviour2D) targets[i]).MaxFutureSteps = maxFutureSteps;
                EditorUtility.SetDirty(targets[i]);
            }
        }
    }
}