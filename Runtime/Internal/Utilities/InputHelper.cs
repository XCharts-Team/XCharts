#if INPUT_SYSTEM_ENABLED
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace XCharts.Runtime
{
    public class InputHelper
    {
        public static Vector2 mousePosition
        {
            get
            {
                var value = Vector2.zero;
                if (null != Mouse.current)
                {
                    value = Mouse.current.position.ReadValue();
                }
                else if (null != Touchscreen.current && Touchscreen.current.touches.Count > 0)
                {
                    value = Touchscreen.current.touches[0].position.ReadValue();
                }
                return value;
            }
        }
        public static int touchCount
        {
            get
            {
                var value = 0;
                if (null != Touchscreen.current)
                {
                    value = Touchscreen.current.touches.Count;
                }
                return value;
            }
        }

        public static Touch GetTouch(int v)
        {
            UnityEngine.TouchPhase PhaseConvert(TouchState state)
            {
                UnityEngine.TouchPhase temp = UnityEngine.TouchPhase.Began;
                switch (state.phase)
                {
                    case UnityEngine.InputSystem.TouchPhase.Began:
                        temp = UnityEngine.TouchPhase.Began;
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Moved:
                        temp = UnityEngine.TouchPhase.Moved;
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Canceled:
                        temp = UnityEngine.TouchPhase.Canceled;
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Stationary:
                        temp = UnityEngine.TouchPhase.Stationary;
                        break;
                    default:
                    case UnityEngine.InputSystem.TouchPhase.Ended:
                    case UnityEngine.InputSystem.TouchPhase.None:
                        temp = UnityEngine.TouchPhase.Ended;
                        break;
                }
                return temp;
            }
            var touch = Touchscreen.current.touches[v];
            var value = touch.ReadValue();
            //copy touchcontrol's touchstate data  into  touch
            return new Touch
            {
                deltaPosition = value.delta,
                fingerId = value.touchId,
                position = value.position,
                phase = PhaseConvert(value),
                pressure = value.pressure,
                radius = value.radius.magnitude,
                radiusVariance = value.radius.sqrMagnitude,
                type = value.isPrimaryTouch ? TouchType.Direct : TouchType.Indirect,
                tapCount = value.tapCount,
                deltaTime = Time.realtimeSinceStartup - (float)value.startTime,
                rawPosition = value.startPosition,
            };
        }

        public static bool GetKeyDown(KeyCode keyCode)
        {
            var value = false;
            if (null != Keyboard.current)
            {
                var key = Keyboard.current.spaceKey;
                switch (keyCode)
                {
                    case KeyCode.Space:
                        key = Keyboard.current.spaceKey;
                        break;
                    case KeyCode.L:
                        key = Keyboard.current.lKey;
                        break;
                    default:
                        Debug.LogError($"{nameof(InputHelper)}: not support {keyCode} yet , please add it yourself if needed");
                        break;
                }

                value = key.wasPressedThisFrame;
            }
            return value;
        }

    }
}
#endif
