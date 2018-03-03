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
/// Main controller for the Steering Behaviour scenes.
/// </summary>
public class SBSceneController: MonoBehaviour
{
    /// <summary>
    /// Reference to a transform (empty) used as the hidden mouse target.
    /// </summary>
    public Transform mouseTarget;

    /// <summary>
    /// Reference to the agent being controlled.
    /// </summary>
    public SteeringManager2D agent;

    /// <summary>
    /// Indicates if the agent should teleport when it gets outside the screen edges.
    /// </summary>
    public bool teleportAtEdges = true;

    /// <summary>
    /// Stores the camera bounds for checking agent teleporting.
    /// </summary>
    private Rect cameraBounds;

    /// <summary>
    /// Interval in seconds to allow a teleport again. Used to prevent
    /// flickering at the viewport edges.
    /// </summary>
    private const float teleportLimit = 1;

    /// <summary>
    /// Countdown in seconds to allow a teleport again. Used to prevent
    /// flickering at the viewport edges.
    /// </summary>
    private float teleportCountdown = 0;

    /// <summary>
    /// Initialization of the script.
    /// Checks if references are set up and calculate the camera bounds
    /// for the edge teleport.
    /// </summary>
    private void Awake()
    {
        if(!mouseTarget)
            Debug.LogError("Property mouseTarget is required");

        if(!agent)
            Debug.LogError("Property agent is required");

        agent.Active = false;

        float aspectRatio = (float) Screen.width / Screen.height;
        float extentY = Camera.main.orthographicSize;
        float extentX = extentY * aspectRatio;
        cameraBounds = new Rect(-extentX, -extentY, 2 * extentX, 2 * extentY);
    }

    /// <summary>
    /// Updates on each frame of the game.
    /// Moves the target as the mouse position updates.
    /// </summary>
    private void Update()
    {        
        Vector3 mousePos = Input.mousePosition;
        mouseTarget.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        // Activate the steering behaviour only while the mouse button/touch is kept pressed
        // (discondering the pressing over UI objects)
        bool isDown = !HitTestUI() &&
                      ((Input.touchCount == 1 &&
                          (Input.GetTouch(0).phase == TouchPhase.Began ||
                           Input.GetTouch(0).phase == TouchPhase.Moved ||
                           Input.GetTouch(0).phase == TouchPhase.Stationary))
                      || Input.GetMouseButton(0));
        agent.Active = isDown;

        // Teleport the agent if needed
        if(teleportAtEdges)
            CheckTeleport();
    }

    /// <summary>
    /// Test if the current touch/mouse position hits any UI object.
    /// </summary>
    /// <returns>True if the position is hitting an UI object, false otherwise.</returns>
    private bool HitTestUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if(results.Count > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Teleports the agent when it gets away from the screen limits.
    /// </summary>
    private void CheckTeleport()
    {
        if(teleportCountdown == 0 && !cameraBounds.Contains(agent.transform.position))
        {
            teleportCountdown = teleportLimit;
            agent.transform.position *= -1;
        }

        if(teleportCountdown > 0)
        {
            teleportCountdown -= Time.deltaTime;
            if(teleportCountdown < 0)
                teleportCountdown = 0;
        }
    }
}