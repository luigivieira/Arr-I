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
/// Editor for the <c>FlowField2D</c> class.
/// </summary>
[ExecuteInEditMode]
[CustomEditor(typeof(FlowField2D))]
public class FlowField2DEditor: Editor
{
    /// <summary>
    /// Value used with the fill with value button.
    /// </summary>
    private Vector2 fillValue = Vector2.zero;

    /// <summary>
    /// Method called when there were changes in the editor's fields via inspector
    /// (i.e. a property has been changed, for instance).
    /// </summary>
    override public void OnInspectorGUI()
    {
        FlowField2D tgt = (FlowField2D)target;

        // Rows
        int rows = tgt.Rows;
        EditorGUI.BeginChangeCheck();
        rows = EditorGUILayout.DelayedIntField(new GUIContent("Rows", "Number of rows in the flow field"), rows);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(tgt, "FlowField2D - Rows");
            tgt.Rows = rows;
            EditorUtility.SetDirty(tgt);
        }

        // Columns
        int columns = tgt.Columns;
        EditorGUI.BeginChangeCheck();
        columns = EditorGUILayout.DelayedIntField(new GUIContent("Columns", "Number of columns in the flow field"), columns);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(tgt, "FlowField2D - Columns");
            tgt.Columns = columns;
            EditorUtility.SetDirty(tgt);
        }

        // Values
        tgt.showValues = EditorGUILayout.Foldout(tgt.showValues, "Values");
        if(tgt.showValues)
        {
            EditorGUI.indentLevel++;

            // Rows
            for(int r = 0; r < tgt.Rows; r++)
            {
                tgt.showRows[r] = EditorGUILayout.Foldout(tgt.showRows[r], string.Format("Row {0}", r));
                if(tgt.showRows[r])
                {
                    EditorGUI.indentLevel++;

                    // Columns on each row
                    for(int c = 0; c < tgt.Columns; c++)
                    {
                        Vector2 value = tgt[r, c];
                        EditorGUI.BeginChangeCheck();
                        value = EditorGUILayout.Vector2Field(new GUIContent(string.Format("Column {0}", c), "Vector of the desired velocity on this cell of the flow field"), value);
                        if(EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(tgt, string.Format("FlowField2D - Value at ({0},{0})", r, c));
                            tgt[r, c] = value;
                            EditorUtility.SetDirty(tgt);
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }

            EditorGUI.indentLevel--;
        }

        // Buttons
        if(GUILayout.Button("Clear all values"))
        {
            Undo.RecordObject(tgt, "FlowField2D - Clear");
            tgt.ClearAllValues();
            EditorUtility.SetDirty(tgt);
        }

        if(GUILayout.Button("Fill with random values"))
        {
            Undo.RecordObject(tgt, "FlowField2D - Random Values");
            tgt.FillWithRandomValues();
            EditorUtility.SetDirty(tgt);
        }

        if(GUILayout.Button("Fill with Perlin noise"))
        {
            Undo.RecordObject(tgt, "FlowField2D - Perlin Noise");
            tgt.FillWithPerlinNoise();
            EditorUtility.SetDirty(tgt);
        }
       
        EditorGUI.BeginChangeCheck();
        fillValue = EditorGUILayout.Vector2Field(new GUIContent("Fill value", "Value to be used with the fill with value button"), fillValue);
        if(GUILayout.Button("Fill with value"))
        {
            Undo.RecordObject(tgt, "FlowField2D - Fill with Value");
            tgt.FillWithValue(fillValue);
            EditorUtility.SetDirty(tgt);
        }

    }
}