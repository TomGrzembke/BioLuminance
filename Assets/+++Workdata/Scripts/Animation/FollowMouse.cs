using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    #region serialized fields
    [SerializeField] float rotationSpeed = 25;
    [SerializeField] bool rotatePlayer = true;

    #endregion

    #region private fields
    Vector2 dir;
    Quaternion rotation;
    Camera mainCam;
    float angle;
    float movespeed = 10;
    #endregion

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        dir = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        if (rotatePlayer)
        {
            rotation = Quaternion.AngleAxis(-angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        Vector2 cursorPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.MoveTowards(transform.position, cursorPos, movespeed * Time.deltaTime);
    }
}