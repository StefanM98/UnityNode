#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class TestSocketIO : MonoBehaviour
{
    private SocketIOComponent socket;
    public GameObject playerPrefab;

    public GameObject myPlayer;

    Dictionary<string, GameObject> players;

    public void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);

        players = new Dictionary<string, GameObject>();
    }


    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("Connected");
    }

    private void OnRequestPosition(SocketIOEvent e)
    {
        Debug.Log("Server is requesting position");

        socket.Emit("updatePosition", new JSONObject(VectorToJson(myPlayer.transform.position)));
    }

    private void OnUpdatePosition(SocketIOEvent e)
    {
        Debug.Log("Updating position: " + e.data);

        var position = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "y"));

        var player = players[e.data["id"].ToString()];

        player.transform.position = position;
    }

    private void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("player disconnected: " + e.data);

        var id = e.data["id"].ToString();
        var player = players[id];

        Destroy(player);
        players.Remove(id);
    }

    private void OnSpawned(SocketIOEvent e)
    {
        Debug.Log("spawned" + e.data);
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        if (e.data["x"])
        {
            var movePosition = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "y"));

            var navigatePos = player.GetComponent<NavigatePosition>();

            navigatePos.NavigateTo(movePosition);
        }

        players.Add(e.data["id"].ToString(), player);
        Debug.Log("count: " + players.Count);
    }

    private void OnMove(SocketIOEvent e)
    {
        Debug.Log("player is moving" + e.data);

        var position = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "y"));

        var player = players[e.data["id"].ToString()];

        var navigatePos = player.GetComponent<NavigatePosition>();

        navigatePos.NavigateTo(position);
    }

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }

    public static string VectorToJson(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"", ""y"":""{1}""}}", vector.x, vector.z);
    }
}