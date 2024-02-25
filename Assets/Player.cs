using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Gun data")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;


    [Header("Movement data")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private float verticalInput;
    private float horizontalInput;

    [Header("Tower data")]
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float towerRotationSpeed;



    [Header("Aim data")]
    [SerializeField] private LayerMask whatIsAimMask;
    [SerializeField] private Transform aimTransform;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateAim();
        CheckInputs();

    }

    private void CheckInputs()
    {

        if (Input.GetButtonDown("Fire1"))
            Shoot();


        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput < 0)
            horizontalInput = -Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyBodyRotation();
        ApplyTowerRotation();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);

        bullet.GetComponent<Rigidbody>().velocity = gunPoint.forward * bulletSpeed;

        Destroy(bullet, 7);
    }

    private void ApplyTowerRotation()
    {
        Vector3 direction = aimTransform.position - towerTransform.position;
        direction.y = 0;


        Quaternion targetRotation = Quaternion.LookRotation(direction);

        towerTransform.rotation = Quaternion.RotateTowards(towerTransform.rotation, targetRotation, towerRotationSpeed);
    }

    private void ApplyBodyRotation()
    {
        transform.Rotate(0, horizontalInput * rotationSpeed, 0);
    }

    private void ApplyMovement()
    {
        Vector3 movement = transform.forward * moveSpeed * verticalInput;
        rb.velocity = movement;
    }

    private void UpdateAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsAimMask)) {

            float fixedY = aimTransform.position.y;
            aimTransform.position = new Vector3(hit.point.x, fixedY, hit.point.z);
        }
    }
}
