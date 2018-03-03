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

/// <summary>
/// Implements a two-dimensional flow field to be used by the FlowField steering behaviour.
/// </summary>
public class FlowField2D: MonoBehaviour
{
    /// <summary>
    /// Indicates if the values foldout is opened in the inspector.
    /// </summary>
    public bool showValues = false;

    /// <summary>
    /// Indicates if the rows foldouts are opened in the inspector.
    /// </summary>
    public bool[] showRows = new bool[] { false };

    /// <summary>
    /// Defines the number of rows in the grid field.
    /// </summary>
    [SerializeField]
    private int rows = 1;

    /// <summary>
    /// Public property to control the access to the <c>rows</c> class attribute.
    /// </summary>
    public int Rows
    {
        get { return rows; }
        set
        {
            value = Mathf.Max(1, value);
            if(value != rows)
            {
                rows = value;
                UpdateInternalData();
            }
        }
    }

    /// <summary>
    /// Defines the number of columns in the grid field.
    /// </summary>
    [SerializeField]
    private int columns = 1;

    /// <summary>
    /// Public property to control the access to the <c>columns</c> class attribute.
    /// </summary>
    public int Columns
    {
        get { return columns; }
        set
        {
            value = Mathf.Max(1, value);
            if(value != columns)
            {
                columns = value;
                UpdateInternalData();
            }
        }
    }

    /// <summary>
    /// Stores the vector values of each cell in the flow field.
    /// </summary>
    [SerializeField]
    private Vector2[] field = new Vector2[2];

    /// <summary>
    /// Alocates (or reallocates, if needed) the data structures used to represent
    /// the flow field.
    /// </summary>
    private void UpdateInternalData()
    {
        // Do nothing if the field is already updated
        if(field.Length == rows * columns)
            return;

        // Save the existing field so the data can be copied
        Vector2[] saved = field;

        bool[] savedShowRows = showRows;

        // Allocate the field based on the configured number of rows and columns
        int len = rows * columns;
        field = new Vector2[len];

        showRows = new bool[rows];

        // Copy the saved data into the newly allocated field
        if(saved != null)
        {
            for(int i = 0; i < len; i++)
            {
                if(i < saved.Length)
                    field[i] = saved[i];
                else
                    field[i] = Vector2.zero;
            }

            for(int i = 0; i < rows; i++)
            {
                if(i < savedShowRows.Length)
                    showRows[i] = savedShowRows[i];
                else
                    showRows[i] = false;
            }
        }
    }

    /// <summary>
    /// Get the rectangular bounds of the flow field.
    /// </summary>
    /// <returns><c>Rect</c> instance with the bounds of the flow field.</returns>
    public Rect GetBounds()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float extentX = columns * transform.localScale.x;
        float extentY = rows * transform.localScale.y;
        return new Rect(x + -extentX / 2f, y + -extentY / 2f, extentX, extentY);
    }

    /// <summary>
    /// Operator overload to allow accesing the values in a cell of the flow field via 
    /// the operator [].
    /// This operator raises <c>InvalidCellException</c> if either the row
    /// or column values used are invalid.
    /// </summary>
    /// <param name="row">The index of the row to access.</param>
    /// <param name="column">The index of the column to access.</param>
    /// <returns>The value of the <c>Vector2</c> contained in the given cell.</returns>
    public Vector2 this[int row, int column]
    {
        get { return GetValue(row, column); }
        set { SetValue(row, column, value); }
    }

    /// <summary>
    /// Gets the value in the given cell of the flow field.
    /// This method raises <c>InvalidCellException</c> if either the row
    /// or column values used are invalid.
    /// </summary>
    /// <param name="row">The index of the row to access.</param>
    /// <param name="column">The index of the column to access.</param>
    /// <returns>The value of the <c>Vector2</c> contained in the given cell.</returns>
    public Vector2 GetValue(int row, int column)
    {
        int len = rows * columns;
        int index = row * columns + column;
        if(index < 0 || index >= len)
            throw new InvalidCellException(row, column);

        return field[index];
    }

    /// <summary>
    /// Gets the value of the cell under the given world position.
    /// </summary>
    /// <param name="position">Vector2 with the coordinates in the world position.</param>
    /// <returns>The value of the <c>Vector2</c> contained in the cell under the given
    /// world position or <c>Vector2.zero</c> if no cell is under that position.</returns>
    public Vector2 GetValueAtPosition(Vector2 position)
    {
        Vector2 coord = WorldPositionToCellCoordinates(position);
        if(coord.x == -1 && coord.y == -1)
            return Vector2.zero;
        else
            return GetValue((int) coord.x, (int) coord.y);
    }

    /// <summary>
    /// Sets the value in the given cell of the flow field.
    /// This method raises <c>InvalidCellException</c> if either the row
    /// or column values used are invalid.
    /// </summary>
    /// <param name="row">The index of the row to access.</param>
    /// <param name="column">The index of the column to access.</param>
    /// <param name="value">The <c>Vector2</c> value to set in the cell.</param>
    public void SetValue(int row, int column, Vector2 value)
    {
        int len = rows * columns;
        int index = row * columns + column;
        if(index < 0 || index >= len)
            throw new InvalidCellException(row, column);

        field[index] = value;
    }

    /// <summary>
    /// Sets the value of the cell under the given world position. If there is no cell
    /// under the given position, nothing is done.
    /// </summary>
    /// <param name="position">Vector2 with the coordinates in the world position.</param>
    /// <param name="value">The value of the <c>Vector2</c> to be updated in the 
    /// cell under the given world position.</param>
    public void SetValueAtPosition(Vector2 position, Vector2 value)
    {
        Vector2 coord = WorldPositionToCellCoordinates(position);
        if(coord.x != -1 && coord.y != -1)
            SetValue((int) coord.x, (int) coord.y, value);
    }

    /// <summary>
    /// Clears all vectors in the flow field.
    /// </summary>
    public void ClearAllValues()
    {
        for(int r = 0; r < rows; r++)
            for(int c = 0; c < columns; c++)
                SetValue(r, c, Vector2.zero);
    }

    /// <summary>
    /// Fills the flow field with the given vector value.
    /// </summary>
    public void FillWithValue(Vector2 value)
    {
        for(int r = 0; r < rows; r++)
            for(int c = 0; c < columns; c++)
                SetValue(r, c, value);
    }

    /// <summary>
    /// Fills the flow field with random vectors.
    /// </summary>
    public void FillWithRandomValues()
    {
        for(int r = 0; r < rows; r++)
        {
            for(int c = 0; c < columns; c++)
            {
                Vector2 value = Vector2.zero.Random();
                SetValue(r, c, value);
            }
        }
    }

    /// <summary>
    /// Fills the flow field with values following a Perlin noise.
    /// </summary>
    public void FillWithPerlinNoise()
    {
        for(int r = 0; r < rows; r++)
        {
            for(int c = 0; c < columns; c++)
            {
                float angle = 360 * Mathf.PerlinNoise((float) r / rows, (float) c / columns);
                Vector2 value = Vector2.right.Rotate(angle);
                SetValue(r, c, value);
            }
        }
    }

    /// <summary>
    /// Converts the given <c>Vector2</c> with a world position to another
    /// <c>Vector2</c> with the coordinates (row and column) of the cell
    /// in the flow field where the point is located. If the point is located
    /// outside the bounds of the flow field, the return is always (-1, -1).
    /// </summary>
    /// <param name="position">World position to convert.</param>
    /// <returns>Row and column of the cell under the given world point.</returns>
    public Vector2 WorldPositionToCellCoordinates(Vector2 position)
    {
        Rect bounds = GetBounds();

        if(!bounds.Contains(position))
            return Vector2.one * -1;

        float cellWidth = bounds.width / columns;
        float cellHeight = bounds.height / rows;

        Vector2 localPos = position - bounds.min;

        int row = (int) (localPos.y / cellHeight);
        int column = (int) (localPos.x / cellWidth);

        return new Vector2(row, column);
    }

    /// <summary>
    /// Draws debug and control information in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        // Draw a rectangle around the bounds
        Rect bounds = GetBounds();
        Rect area = new Rect(bounds.x, bounds.y, bounds.width, bounds.height);
        GizmosExt.DrawRectangle(area);

        // Draw each cell
        Vector2 value, start, end;
        Rect cell = new Rect(0, 0, bounds.width / columns, bounds.height / rows);
        for(int r = 0; r < rows; r++)
        {
            cell.y = bounds.yMin + r * cell.height;
            for(int c = 0; c < columns; c++)
            {
                // Draw the cell rectangle
                cell.x = bounds.xMin + c * cell.width;
                GizmosExt.DrawRectangle(cell);

                // Draw the cell contents
                value = field[r * columns + c];
                if(value != Vector2.zero)
                {
                    value = value.normalized;
                    value = new Vector2(value.x * transform.localScale.x, value.y * transform.localScale.y);
                    start = cell.center - value / 3.5f;
                    end = cell.center + value / 3.5f;
                    GizmosExt.DrawArrow(start, end);
                }
            }
        }
    }
}

/// <summary>
/// Custom exception to indicate an attempt to access an invalid cell in the flow field.
/// </summary>
public class InvalidCellException: System.Exception
{
    /// <summary>
    /// The row used in the invalid attempted access.
    /// </summary>
    public int row;

    /// <summary>
    /// The column used in the invalid attempted access.
    /// </summary>
    public int column;

    /// <summary>
    /// Class constructor.
    /// </summary>
    /// <param name="row">The row used in the invalid attempted access.</param>
    /// <param name="column">The column used in the invalid attempted access.</param>
    public InvalidCellException(int row, int column)
        : base(string.Format("There is no cell given by row {0} and column {1}", row, column))
    {
        this.row = row;
        this.column = column;
    }
}