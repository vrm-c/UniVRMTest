using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using VRM;

namespace VRMViewer
{
    public class RokuroCamera : MonoBehaviour
    {
        [SerializeField]
        private GameObject _faceExpressionPanelGo;

        [SerializeField]
        private GameObject _motionControlPanel;

        [SerializeField]
        private GameObject _viewpointPanel;

        [SerializeField]
        private GameObject _predefinedExpression;

        [SerializeField]
        private GameObject _scrollBarVertical;

        [SerializeField]
        private GameObject _sliderFacialCamera;

        [SerializeField]
        private GameObject _sliderOrbitalRadius;

        [SerializeField]
        private GameObject _sliderVerticalPosition;

        [Range(0.1f, 5.0f)]
        private float _rotateSpeed = 0.7f;

        [Range(0.1f, 5.0f)]
        private float _grabSpeed = 0.7f;

        [Range(0.1f, 5.0f)]
        private float _dollySpeed = 1.0f;

        private List<GameObject> _objs;
        private int _validExpNum = 0;
        private string[] _sliderExpName;
        private static float _defaultDistance = 2.0f;
        private static float _deltaMovement = 0.08f;
        private bool _sliderAdjust;

        public List<GameObject> Objs { set { _objs = value; } }
        public int ValidExpNum { set { _validExpNum = value; } }

        struct PosRot
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }

        class _Rokuro
        {
            public float Yaw;
            public float Pitch;
            public float ShiftX;
            public float ShiftY;
            public float DeltaMovement = _deltaMovement;
            public float Distance = _defaultDistance;

            public void Rotate(float x, float y)
            {
                Yaw += x;
                Pitch -= y;
                Pitch = Mathf.Clamp(Pitch, -90, 90);
            }

            public void Grab(float x, float y)
            {
                ShiftX += x * Distance;
                ShiftY += y * Distance;
            }

            public void Dolly(float delta)
            {
                if (delta > 0)
                {
                    Distance *= 0.9f;
                }
                else if (delta < 0)
                {
                    Distance *= 1.1f;
                }
            }

            public PosRot Calc()
            {
                var r = Quaternion.Euler(Pitch, Yaw, 0);

                if (Input.GetKey(KeyCode.W)) // Move forward
                {
                    Distance = Distance - DeltaMovement;

                    return new PosRot
                    {
                        Position = r * new Vector3(-ShiftX, -ShiftY, -Distance),
                        Rotation = r,
                    };
                }
                else if (Input.GetKey(KeyCode.A))  // Move to the left
                {
                    ShiftX = ShiftX + DeltaMovement;

                    return new PosRot
                    {
                        Position = r * new Vector3(-ShiftX, -ShiftY, -Distance),
                        Rotation = r,
                    };
                }
                else if (Input.GetKey(KeyCode.D)) // Move to the right
                {
                    ShiftX = ShiftX - DeltaMovement;

                    return new PosRot
                    {
                        Position = r * new Vector3(-ShiftX, -ShiftY, -Distance),
                        Rotation = r,
                    };
                }
                else if (Input.GetKey(KeyCode.S)) // Move backward
                {
                    Distance = Distance + DeltaMovement;

                    return new PosRot
                    {
                        Position = r * new Vector3(-ShiftX, -ShiftY, -Distance),
                        Rotation = r,
                    };
                }
                else if (Input.GetKeyDown(KeyCode.O)) // Back to the origin
                {
                    ShiftX = 0;
                    ShiftY = 0;

                    Yaw = 0;
                    Pitch = 0;
                    Distance = _defaultDistance;

                    return new PosRot
                    {
                        Position = r * new Vector3(0, 0, 0),
                        Rotation = r,
                    };
                }
                else
                {
                    return new PosRot
                    {
                        Position = r * new Vector3(-ShiftX, -ShiftY, -Distance),
                        Rotation = r,
                    };
                }

            }
        }

        private _Rokuro m_currentCamera = new _Rokuro();
        private List<Coroutine> m_activeCoroutines = new List<Coroutine>();

        private void OnEnable()
        {
            // left mouse drag
            m_activeCoroutines.Add(StartCoroutine(MouseDragOperationCoroutine(0, diff =>
            {
                m_currentCamera.Rotate(diff.x * _rotateSpeed, diff.y * _rotateSpeed);
            })));
            // right mouse drag
            m_activeCoroutines.Add(StartCoroutine(MouseDragOperationCoroutine(1, diff =>
            {
                m_currentCamera.Rotate(diff.x * _rotateSpeed, diff.y * _rotateSpeed);
            })));
            // middle mouse drag
            m_activeCoroutines.Add(StartCoroutine(MouseDragOperationCoroutine(2, diff =>
            {
                m_currentCamera.Grab(
                    diff.x * _grabSpeed / Screen.height,
                    diff.y * _grabSpeed / Screen.height
                    );
            })));
            // mouse wheel
            m_activeCoroutines.Add(StartCoroutine(MouseScrollOperationCoroutine(diff =>
            {
                m_currentCamera.Dolly(diff.y * _dollySpeed);
            })));
        }

        private void OnDisable()
        {
            foreach (var coroutine in m_activeCoroutines)
            {
                StopCoroutine(coroutine);
            }
            m_activeCoroutines.Clear();
        }

        private void Update()
        {
            var posRot = m_currentCamera.Calc();

            transform.localRotation = posRot.Rotation;
            transform.localPosition = posRot.Position;
        }

        private IEnumerator MouseDragOperationCoroutine(int buttonIndex, Action<Vector2> dragOperation)
        {
            var scrollBarVertical = _scrollBarVertical.GetComponent<UISlider>();
            var sliderFacialCamera = _sliderFacialCamera.GetComponent<UISlider>();
            var sliderOrbitalRadius = _sliderOrbitalRadius.GetComponent<UISlider>();
            var sliderVerticalPosition = _sliderVerticalPosition.GetComponent<UISlider>();

            while (true)
            {
                while (!Input.GetMouseButtonDown(buttonIndex))
                {
                    yield return null;
                }
                var prevPos = Input.mousePosition;

                while (Input.GetMouseButton(buttonIndex))
                {
                    var currPos = Input.mousePosition;

                    _sliderAdjust = false;

                    // Check if any slider is being used
                    if (_faceExpressionPanelGo.activeSelf == true)
                    {
                        if (_predefinedExpression.activeSelf == true) {
                            foreach (var objs in _objs)
                            {
                                if (objs.GetComponent<UISlider>().IsBeingDragged() == true)
                                {
                                    _sliderAdjust = true;
                                }
                            }
                        }

                        if (scrollBarVertical.IsBeingDragged() == true)
                        {
                            _sliderAdjust = true;
                        }
                    }

                    // slider for face camera distance
                    if (_viewpointPanel.activeSelf == true)
                    {
                        if (_sliderFacialCamera.activeSelf == true && sliderFacialCamera.IsBeingDragged() == true)
                        {
                            _sliderAdjust = true;
                        }
                    }

                    // sliders for moving sphere property
                    if (_motionControlPanel.activeSelf == true)
                    {
                        if (_sliderOrbitalRadius.activeSelf == true && sliderOrbitalRadius.IsBeingDragged() == true)
                        {
                            _sliderAdjust = true;
                        }

                        if (_sliderVerticalPosition.activeSelf == true && sliderVerticalPosition.IsBeingDragged() == true)
                        {
                            _sliderAdjust = true;
                        }
                    }

                    if (EventSystem.current.IsPointerOverGameObject() == true)
                    {
                        // The mouse pointer is on the panel, do nothing for camera movement
                    }
                    else if (_sliderAdjust == true)
                    {
                        // The mouse pointer is adjusting an expression, do nothing for camera movement
                    }
                    else
                    {
                        var diff = currPos - prevPos;
                        dragOperation(diff);
                    }

                    prevPos = currPos;
                    yield return null;
                }
            }
        }

        private IEnumerator MouseScrollOperationCoroutine(Action<Vector2> scrollOperation)
        {
            while (true)
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                {
                    scrollOperation(Input.mouseScrollDelta);
                }
                
                yield return null;
            }
        }

    }
}
