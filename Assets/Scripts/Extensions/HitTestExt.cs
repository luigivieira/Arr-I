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
 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Helper class with hit test utilities.
/// </summary>
public static class HitTestExt
{
    /// <summary>
    /// The last touch position, in world coordinates.
    /// </summary>
    private static Vector2 lastTouchPosition = Vector2.zero;

    /// <summary>
    /// Test if the given screen position hits any UI object.
    /// </summary>
    /// <param name="position">Vector2 with the position to test.</param>
    /// <returns>True if the position is hitting an UI object, false otherwise.</returns>
    public static bool HitTestUI(Vector2 position)
    {
        try
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = Camera.main.WorldToScreenPoint(position);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if(results.Count > 0)
                return true;
            else
                return false;
        }
        catch(System.Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Get the mouse/touch position in world coordinates. In the case of touch devices
    /// with multitouch abilities, the position returned is always the position of the
    /// finger closer to the target reference. If there is no touch, the last touch position
    /// is returned.
    /// </summary>
    /// <param name="target">Transform of the target for reference, so the touch
    /// closest to it can be considered (on mobile devices). In standalone builds
    /// this parameter is not used.</param>
    /// <returns>Vector2 with the mouse/touch position.</returns>
    public static Vector2 GetTouchPosition(Transform target)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        lastTouchPosition = Camera.main.ScreenToWorldPoint((Vector2) Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IOS
        float minDist = float.MaxValue;
        for(int i = 0; i < Input.touchCount; i++)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            float dist = Vector2.SqrMagnitude(pos - (Vector2) target.position);
            if(dist < minDist)
            {
                lastTouchPosition = pos;
                minDist = dist;
            }
        }
#endif
        return lastTouchPosition;
    }

    /// <summary>
    /// Check if the user is touching/clicking the device (i.e. keeping at least one
    /// finger or the the mouse button pressed) on the scene screen but not in a UI
    /// element.
    /// </summary>
    /// <param name="reference">Transform of reference, so the touch
    /// closest to it can be considered (on multitouch devices). In standalone builds
    /// this parameter is not used.</param>
    /// <returns>Returns true if the user is touching or clicking on the scene screen.</returns>
    public static bool IsTouching(Transform reference)
    {
        Vector2 position = GetTouchPosition(reference);
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButton(0) && !HitTestUI(position);
#elif UNITY_ANDROID || UNITY_IOS
        return Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Canceled && !HitTestUI(position);
#endif
    }

    /// <summary>
    /// Check if the (last known) touch position is hitting the given collider.
    /// </summary>
    /// <param name="target">The collider to be checked for hit.</param>
    /// <param name="useRaycastAll">Indication if a RaycastAll should be used
    /// instead of a simple raycast. The default is true.</param>
    /// <returns>Returns true if the user is touching or clicking on the collider,
    /// and false otherwise.</returns>
    public static bool IsHitting(Collider2D target, bool useRaycastAll = true)
    {
        Vector2 position = GetTouchPosition(target.transform);

        if(useRaycastAll)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, Camera.main.transform.forward);
            foreach(RaycastHit2D hit in hits)
                if(hit.collider != null && hit.collider.name == target.name)
                    return true;
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(position, Camera.main.transform.forward);
            if(hit.collider != null && hit.collider == target)
                return true;
        }

        return false;
    }
}
