using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform tip;
    private bool canFire = false;

    public int firingRate = 10;
    public bool mouseControlled;

    // Start is called before the first frame update
    void Start()
    {
        tip = transform.Find("Tip");
        Debug.Assert(tip, "Tip not found");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Determine if we can fire again
        int t = Time.frameCount;
        if (t % firingRate == 0)
        {
            canFire = true;
        }

        // Set rotation towards mouse position if mouse controlled
        if (mouseControlled)
        {
            // Get mouse point on camera plane
            Vector3 wPos = Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            // Get mouse point on world plane
            Vector3 mPos = Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                (-wPos.z) / Mathf.Cos(Mathf.Deg2Rad * -Camera.main.transform.rotation.eulerAngles.x) + transform.position.z));
            // Get angle between turret and world plane point
            Vector3 angle = new Vector3(0, 0, Mathf.Atan2(
                mPos.y - 0.2f - transform.position.y, 
                mPos.x - transform.position.x) * Mathf.Rad2Deg - 90.0f);
            transform.rotation = Quaternion.Euler(angle);
        }

        // Fire when ready
        if (canFire && (!mouseControlled || Input.GetButton("Fire")))
        {
            float angle = Mathf.Deg2Rad * (transform.rotation.eulerAngles.z + 90.0f);
            Vector3 dir = new Vector3(Mathf.Rad2Deg * Mathf.Cos(angle), Mathf.Rad2Deg * Mathf.Sin(angle), 0);
            GameController.self.playerBulletSystem.emit(tip.position, dir);
            canFire = false;
        }
    }
}
