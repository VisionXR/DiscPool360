using UnityEngine;

public class UserDirection : MonoBehaviour
{
    public GameObject userHead;

    private void LateUpdate()
    {
        transform.position = userHead.transform.position;
        transform.eulerAngles = new Vector3(0, userHead.transform.eulerAngles.y, 0);
    }
}
