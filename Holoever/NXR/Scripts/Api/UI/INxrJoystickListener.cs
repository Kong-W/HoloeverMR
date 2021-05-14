


/// This script provides an interface for AR Joystick
///  
namespace Nxr.Internal
{
    public interface INxrJoystickListener
    {
        void OnPressL1();

        void OnPressL2();

        void OnPressR1();

        void OnPressR2();

        void OnPressX();

        void OnPressY();

        void OnPressA();

        void OnPressB();

        void OnPressSelect();

        void OnPressStart();

        void OnPressDpadUp();

        void OnPressDpadDown();

        void OnPressDpadLeft();

        void OnPressDpadRight();

        // 左摇杆
        void OnLeftStickX(float axisValue);

        void OnLeftStickY(float axisValue);

        // 右摇杆
        void OnRightStickX(float axisValue);

        void OnRightStickY(float axisValue);

        void OnLeftStickDown();

        void OnRightStickDown();
    }
}