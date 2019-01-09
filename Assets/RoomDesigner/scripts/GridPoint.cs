using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GridPoint : MonoBehaviour
{
    public GameObject wall;
    public GameObject startPoint;
    public Text text;
    public Transform parent;
    private bool firstWallCoordinate = true;
    private float firstWallStartXPos;
    private float firstWallStartZPos;
    private float wallStartXPos;
    private float wallStartZPos;
    private float wallEndXPos;
    private float wallEndZPos;
    private bool roomFinished = false;
    private bool startPointChosen = false;

    List<string> roomCommands = new List<string>();
    
    void Update()
    {
        if (roomFinished == false)
        {
            WallDraw();
        }
        else if (startPointChosen == false)
        {
            ChooseStartPoint();
        }
    }

    private void WallDraw()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = -Vector3.one;

            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 0));

            int xPos = Mathf.RoundToInt(clickPosition.x);
            int zPos = Mathf.RoundToInt(clickPosition.z);
            
            if (xPos < 0 || xPos > 19 || zPos < 0 || zPos > 9)
            {
                Debug.Log("Out of range");
            }
            else
            {
                if (firstWallCoordinate == true)
                {
                    wallStartXPos = xPos;
                    wallStartZPos = zPos;
                    firstWallStartXPos = xPos;
                    firstWallStartZPos = zPos;

                    var instance = Instantiate(wall, new Vector3(wallStartXPos, 1, wallStartZPos), Quaternion.identity);

                    instance.transform.localScale = new Vector3(0.04f, 1, 0.04f);

                    firstWallCoordinate = false;
                }
                else
                {
                    wallEndXPos = xPos;
                    wallEndZPos = zPos;

                    float wallCenterXPos = (wallStartXPos + wallEndXPos) / 2;
                    float wallCenterZPos = (wallStartZPos + wallEndZPos) / 2;
                    float wallLength = GetWallLength(wallStartXPos, wallStartZPos, wallEndXPos, wallEndZPos);
                    float wallRotation = GetWallRotation(wallStartXPos, wallStartZPos, wallEndXPos, wallEndZPos);

                    PlaceWall(wallCenterXPos, wallCenterZPos, wallLength, wallRotation);

                    wallStartXPos = wallEndXPos;
                    wallStartZPos = wallEndZPos;
                }

                if (wallEndXPos == firstWallStartXPos && wallEndZPos == firstWallStartZPos)
                {
                    roomFinished = true;
                    text.text = "Kies je spawn punt";
                }
            }
        }
    }

    public float GetWallRotation(float wallStartXPos, float wallStartZPos, float wallEndXPos, float wallEndZPos)
    {
        Vector2 vec1 = new Vector2(wallStartXPos, wallStartZPos);
        Vector2 vec2 = new Vector2(wallEndXPos, wallEndZPos);

        Vector2 diference = vec1 - vec2;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    public float GetWallLength(float wallStartXPos, float wallStartZPos, float wallEndXPos, float wallEndZpos)
    {
        float a = (wallStartXPos - wallEndXPos) * -1;
        float b = (wallStartZPos - wallEndZPos) * -1;

        return Mathf.Sqrt((a * a) + (b * b));
    }

    public void PlaceWall(float wallCenterXPos, float wallCenterZPos, float wallLength, float wallRotation)
    {
        var instance = Instantiate(wall, new Vector3(wallCenterXPos, -0.5f, wallCenterZPos), Quaternion.identity);

        instance.transform.localScale = new Vector3((wallLength / 10), 1, 0.02f);
        instance.transform.Rotate(0, wallRotation, 0);

        roomCommands.Add("W," + wallCenterXPos + "," + wallCenterZPos + "," + wallLength + "," + wallRotation);
    }

    public void ChooseStartPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = -Vector3.one;

            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 0));

            int xPos = Mathf.RoundToInt(clickPosition.x);
            int zPos = Mathf.RoundToInt(clickPosition.z);

            Instantiate(startPoint, new Vector3(xPos, 0, zPos), Quaternion.identity);

            roomCommands.Add("s," + xPos + "," + zPos);

            startPointChosen = true;

            text.enabled = false;

            Naar3DOmgeving();
        }
    }

    public void Naar3DOmgeving()
    {
        string filename = System.DateTime.Now.ToString("yyyyMMdd-hhmmss");

        using (StreamWriter writetext = File.AppendText("C:\\Users\\niek_vdv\\Desktop\\" + filename + ".txt"))
        {
            foreach (string command in roomCommands)
            {
                writetext.WriteLine(command);
            }

            writetext.Close();
        }
    }
}