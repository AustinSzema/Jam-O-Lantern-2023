using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Vector3Variable _playerPos;

    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 10f;

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private MeshRenderer _meshRenderer;

    private Camera _camera;

    private bool _isMoving = true;

    private void Start()
    {
        _camera = Camera.main;
    }

    private Vector3 _cameraPosition;

    private void FixedUpdate()
    {
        _cameraPosition = _camera.transform.position;

        // Calculate the direction from the camera to the object
        Vector3 direction = transform.position - _cameraPosition;

        RaycastHit hit;
        float maxDistance = direction.magnitude;



        if (Physics.Raycast(_cameraPosition, direction, out hit, maxDistance, _layerMask))
        {
            if (hit.collider.gameObject != gameObject || _meshRenderer.isVisible == false)
            {
                // The GameObject is behind a wall or obstacle.
                Debug.Log("GameObject is behind a wall or is not visible to the camera.");
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
                Debug.Log("GameObject is visible to the camera");

            }
        }

        if (_isMoving)
            MoveBaby();



    }

    private void MoveBaby()
    {

        Vector3 direction = _playerPos.Value - transform.position;

        // Project the direction vector onto the XZ plane to get the direction on the Y-axis.
        direction.y = 0;

        // Calculate the rotation to face the target on the Y-axis.
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);


        // Apply the rotation to the rotating object only on the Y-axis.
        _rigidbody.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        // Move towards the target
        _rigidbody.position += transform.forward * _moveSpeed * Time.fixedDeltaTime;


    }


}
