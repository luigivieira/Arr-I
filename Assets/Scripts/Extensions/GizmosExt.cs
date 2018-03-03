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
/// Extension-like class for the <c>Gizmos</c> class.
/// Obs.: This is not really an extension class because extensions are not allowed on static classes
/// (and <c>Gizmos</c> is a static class!).
/// </summary>
public static class GizmosExt
{
    /// <summary>
    /// Draws a two-dimensional rectangle gizmo.
    /// </summary>
    /// <param name="bounds"><c>Rect</c> instance with the data of the rectangle to draw.</param>
    public static void DrawRectangle(Rect bounds)
    {
        Gizmos.DrawLine(new Vector2(bounds.xMin, bounds.yMin), new Vector2(bounds.xMin, bounds.yMax));
        Gizmos.DrawLine(new Vector2(bounds.xMin, bounds.yMin), new Vector2(bounds.xMax, bounds.yMin));
        Gizmos.DrawLine(new Vector2(bounds.xMax, bounds.yMax), new Vector2(bounds.xMin, bounds.yMax));
        Gizmos.DrawLine(new Vector2(bounds.xMax, bounds.yMax), new Vector2(bounds.xMax, bounds.yMin));
    }

    /// <summary>
    /// Draws a two-dimensional arrow gizmo.
    /// </summary>
    /// <param name="origin">Origin (beginning) of the arrow.</param>
    /// <param name="destination">Destination (end) of the arrow.</param>
    /// <param name="arrowHeadLength">Lenght of the arrow head (default is 0.2f) in world unities.</param>
    /// <param name="arrowHeadAngle">Angle of the arrow head (default is 20f) in degrees.</param>
    public static void DrawArrow(Vector2 origin, Vector2 destination, float arrowHeadLength = 0.2f, float arrowHeadAngle = 20f)
    {
        Gizmos.DrawLine(origin, destination);

        Vector2 direction = destination - origin;

        Vector2 right = (direction.normalized * -arrowHeadLength).Rotate(arrowHeadAngle);
        Gizmos.DrawRay(destination, right);

        Vector2 left = (direction.normalized * -arrowHeadLength).Rotate(-arrowHeadAngle);
        Gizmos.DrawRay(destination, left);
    }
}
