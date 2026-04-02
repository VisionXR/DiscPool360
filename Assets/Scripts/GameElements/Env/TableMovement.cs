using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class TableMovement : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public TableDataSO tableData;

    [Header("Follower Objects")]
    public GameObject TableHighLight;
    public Transform tableTransform;
    public List<GameObject> followerObjects;

    // local variables
    public bool canIMove = false;
    private Vector3 lastPosition;

    public void OnHovered()
    {
       
        TableHighLight.SetActive(true);
    }

    public void OnUnhovered()
    {
       
        TableHighLight.SetActive(false);
    }

    public void OnSelected()
    {
        lastPosition = tableTransform.position;
        canIMove = true;
        tableData.TableMovementStarted();
    }

    public void OnUnselected()
    {
        canIMove = false;
        tableData.TableMovementEnded();
    }

    private void Update()
    {
        if (canIMove)
        {
            // Compute delta Y since last frame while selected
            Vector3 delta = tableTransform.position - lastPosition;
            if (Mathf.Abs(delta.y) > Mathf.Epsilon)
            {
                Vector3 yDelta = new Vector3(delta.x, delta.y, delta.z);
                for (int i = 0; i < followerObjects.Count; i++)
                {
                    GameObject follower = followerObjects[i];
                    if (follower == null)
                    {
                        continue;
                    }

                    follower.transform.position += yDelta;
                }
            }

            // Update last position for next frame comparison
            lastPosition = tableTransform.position;
        }
    }
}
