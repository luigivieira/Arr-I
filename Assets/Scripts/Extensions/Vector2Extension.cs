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
/// Extension class for the <c>Vector2</c> class.
/// </summary>
public static class Vector2Extension
{
    /// <summary>
    /// Generates a random instance of a <c>Vector2</c>.
    /// </summary>
    /// <param name="vector">The instance of a <c>Vector2</c> to be changed.</param>
    /// <returns>Instance of a <c>Vector2</c> with random values.</returns>
    public static Vector2 Random(this Vector2 vector)
    {
        vector.x = UnityEngine.Random.Range(-100, 100);
        vector.y = UnityEngine.Random.Range(-100, 100);
        return vector;
    }

    /// <summary>
    /// Rotates the given <c>Vector2</c> by the given number of degrees.
    /// </summary>
    /// <param name="vector">The <c>Vector2</c> to rotate.</param>
    /// <param name="degrees">The amoung of degrees by which to rotate the vector.</param>
    /// <returns>New <c>Vector2</c> rotated by the given angle.</returns>
    public static Vector2 Rotate(this Vector2 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float x = vector.x;
        float y = vector.y;

        vector.x = x * cos - y * sin;
        vector.y = x * sin + y * cos;

        return vector;
    }
}