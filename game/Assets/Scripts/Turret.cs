using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform tip;
    private bool canFire = false;

    public enum Control
    {
        TargetTrain, TargetEnemy, Mouse
    }

    public Control control;
    public int firingPeriod = 10;
    public float rotateSpeed = 1.0f;
    public float targetingRadius = 6.0f;

    private BulletSystem bulletSystem;

    // Start is called before the first frame update
    void Start()
    {
        if (control == Control.TargetTrain)
            bulletSystem = GameController.self.enemyBulletSystem;
        else
            bulletSystem = GameController.self.playerBulletSystem;
        tip = transform.Find("Tip");
        Debug.Assert(tip, "Tip not found");
        Debug.Assert(bulletSystem);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Determine if we can fire again
        int t = Time.frameCount;
        if (t % firingPeriod == 0)
        {
            canFire = true;
        }

        // Set rotation towards mouse position if mouse controlled
        Vector3 target = Vector3.zero;
        switch (control)
        {
            case Control.Mouse:
                // Get mouse point on camera plane
                Vector3 wPos = Camera.main.ScreenToWorldPoint(new Vector3(
                    Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                // Get mouse point on world plane
                target = Camera.main.ScreenToWorldPoint(new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    (-wPos.z) / Mathf.Cos(Mathf.Deg2Rad
                        * -Camera.main.transform.rotation.eulerAngles.x)
                        + transform.position.z));
                break;
            case Control.TargetEnemy:
                LayerMask mask = LayerMask.GetMask("Hittable");
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, targetingRadius, Vector2.zero, 0.0f, mask);
                if (hit)
                {
                    target = hit.point;
                }
                else
                {
                    canFire = false;
                    target = transform.position + Vector3.up;
                }
                break;
            case Control.TargetTrain:
                TrainCar nearestCar = GameController.self.train.getClosestCar(transform.position);
                target = nearestCar.transform.position;
                if (Vector2.Distance(transform.position, target) > targetingRadius)
                {
                    canFire = false;
                    target = transform.position + Vector3.up;
                }
                break;
        }

        // Get angle between turret and world plane point
        float rotationAngle = Mathf.LerpAngle(
            transform.rotation.eulerAngles.z,
            Mathf.Atan2(
                target.y - 0.2f - transform.position.y,
                target.x - transform.position.x) * Mathf.Rad2Deg - 90.0f,
            rotateSpeed
            );
        Vector3 rotation = new Vector3(0, 0, rotationAngle);
        transform.rotation = Quaternion.Euler(rotation);

        // Fire when ready
        if (canFire && (control != Control.Mouse || Input.GetButton("Fire")))
        {
            float angle = Mathf.Deg2Rad * (transform.rotation.eulerAngles.z + 90.0f);
            Vector3 dir = new Vector3(Mathf.Rad2Deg * Mathf.Cos(angle), Mathf.Rad2Deg * Mathf.Sin(angle), 0);
            bulletSystem.emit(tip.position, dir);
            canFire = false;
        }
    }
}
