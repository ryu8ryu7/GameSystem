using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookDown : Updater
{
    private bool _isEnable = false;
    public bool IsEnable { get { return _isEnable; } }

    private Camera _camera = null;
    public Camera Camera { get { return _camera; } set { _camera = value; } }

    /// <summary>
    /// カメラの見ている先
    /// </summary>
    private GameObject _targetDummy = null;

    private Transform _targetTransform = null;

    private CharacterBase _targetCharacter = null;

    private Vector3 _currentPos = Vector3.zero;

    private Vector3 _targetPos = Vector3.zero;

    private Vector3 _cameraPos = Vector3.zero;

    private Vector3 _direction = Vector3.zero;

    private float _moveSpeed = 0.1f;

    private float _cameraYaw = 180.0f;
    public float CameraYaw { get { return _cameraYaw; } }

    [SerializeField]
    private float _targetYaw = 180.0f;

    private float _cameraPitch = 45.0f;

    [SerializeField]
    private float _targetPitch = 45.0f;

    private float _cameraDistance = 10.0f;

    [SerializeField]
    private float _targetDistance = 18.0f;

    [SerializeField]
    private float _cameraDelay = 0.16f;

    public static CameraLookDown CreateCamera()
    {
        GameObject parentObj = new GameObject("CameraLookDown");
        GameObject obj = new GameObject("Camera");
        obj.transform.SetParent(parentObj.transform);
        Camera camera = obj.AddComponent<Camera>();
        CameraLookDown cameraLookDown = obj.AddComponent<CameraLookDown>();
        
        cameraLookDown.Initialize();

        return cameraLookDown;
    }

    public void Awake()
    {
        Initialize();
    }

    public void Start()
    {
        
    }

    public void Initialize()
    {
        if (_targetDummy != null)
            return;

        Camera camera = null;
        if (gameObject.TryGetComponent<Camera>(out camera) == false)
            return;

        _camera = camera;

        _targetDummy = new GameObject("target");
        _targetDummy.transform.SetParent(transform.parent);
        _updatePriority = (int)UpdaterManager.Priority.Camera;

        SetCharacter(null);

        UpdaterManager.Instance.AddUpdater(this);
    }

    public void SetCharacter( CharacterBase character )
    {
        if (character != null)
        {
            _targetCharacter = character;
            _targetTransform = character.transform;
            character.SetCameraLookDown(this);
        }
        else
        {
            if (_targetCharacter != null)
            {
                _targetCharacter.SetCameraLookDown(null);
            }

            _targetCharacter = null;
            _targetTransform = _targetDummy.transform;
        }
    }

    private void UpdateCamera()
    {
        _targetPos = _targetTransform.transform.localPosition;

        if(_targetCharacter != null)
        {
            _targetDummy.transform.localPosition = _targetPos;
        }


        _currentPos = (_targetPos - _currentPos) * _cameraDelay + _currentPos;
        _cameraYaw = (_targetYaw - _cameraYaw) * _cameraDelay + _cameraYaw;
        _cameraPitch = (_targetPitch - _cameraPitch) * _cameraDelay + _cameraPitch;
        _cameraDistance = (_targetDistance - _cameraDistance) * _cameraDelay + _cameraDistance;

        _cameraPos = _currentPos;

        _cameraPos.x += Utility.Sin(_cameraYaw) * _cameraDistance * Utility.Cos(_cameraPitch);
        _cameraPos.z += Utility.Cos(_cameraYaw) * _cameraDistance * Utility.Cos(_cameraPitch);
        _cameraPos.y += Utility.Sin(_cameraPitch) * _cameraDistance + 1.0f;

        _camera.transform.localPosition = _cameraPos;
        _camera.transform.localRotation = Quaternion.Euler(_cameraPitch, _cameraYaw + 180.0f, 0);
    }

    private void UpdateControl()
    {
        float trigger = Input.GetAxis("L_R_Trigger");
        if ( trigger > 0.05f )
        {
            _targetYaw += Utility.GetGameTime() * 96.0f;
        }
        else if( trigger < -0.05f )
        {
            _targetYaw -= Utility.GetGameTime() * 96.0f;
        }

        if(_targetCharacter == null )
        {
            //L Stick
            float lsh = Input.GetAxis("L_Stick_H");
            float lsv = Input.GetAxis("L_Stick_V");
            if ((lsh != 0) || (lsv != 0))
            {
                _direction.x = lsh;
                _direction.z = -lsv;
                float ang = Utility.GetAngle(_direction) + _cameraYaw + 180.0f;
                float magnitude = _direction.magnitude;
                _direction.x = Utility.Sin(ang) * _moveSpeed;
                _direction.z = Utility.Cos(ang) * _moveSpeed;

                Vector3 pos = _targetDummy.transform.localPosition;
                pos.x += _direction.x;
                pos.z += _direction.z;

                RaycastHit hit;
                if( Physics.Raycast( pos + Vector3.up, Vector3.down, out hit, 2.0f, LayerMask.GetMask("3DGround") ) )
                {
                    pos.y = hit.point.y;
                }

                _targetDummy.transform.localPosition = pos;
            }
        }
    }

    public override void PreAlterUpdate()
    {
        UpdateControl();
    }

    public override void AlterUpdate()
    {
        UpdateCamera();
    }

}
