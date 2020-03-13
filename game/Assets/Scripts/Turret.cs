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
    void Update()
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
            Vector3 mPos = Input.mousePosition;
            mPos.z = Camera.main.nearClipPlane;
            mPos = Camera.main.ScreenToWorldPoint(mPos);
            mPos = Camera.main.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                (mPos.z + 6.23f) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.transform.rotation.eulerAngles.x)));
            Vector3 pos = transform.position;
            Vector3 angle = new Vector3(0, 0, Mathf.Atan2(mPos.y - pos.y, mPos.x - pos.x) * Mathf.Rad2Deg - 90.0f);
            transform.rotation = Quaternion.Euler(angle);
            Debug.DrawLine(pos, mPos, Color.red, 1.0f/60.0f, false);
        }

        // Fire when ready
        if (canFire && (!mouseControlled || Input.GetButton("Fire")))
        {
            float angle = Mathf.Deg2Rad * (transform.rotation.eulerAngles.z + 90.0f);
            Vector3 dir = new Vector3(Mathf.Rad2Deg * Mathf.Cos(angle), Mathf.Rad2Deg * Mathf.Sin(angle), 0);
            GameController.self.bulletSystem.emit(tip.position, dir);
            canFire = false;
        }
    }
}
