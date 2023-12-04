using UnityEngine;

public class Input_Controller : MonoBehaviour
{
    private Vector2 _lastMousePos = Vector2.zero;
    private bool _dragging;

    private Vector2 _getMousePosition()
    {
        return (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void _handleDrag()
    {
        var oldDragging = _dragging;
        _dragging = Input.GetMouseButton(0);
        if (oldDragging != _dragging)
        {
            _lastMousePos = Vector2.zero;
        }

        if (!_dragging) return;

        if (_lastMousePos == Vector2.zero)
        {
            _lastMousePos = _getMousePosition();
            return;
        }

        var currentMousPos = _getMousePosition();

        var direction = _lastMousePos - currentMousPos;
        transform.position += (Vector3) direction;
        _lastMousePos = _getMousePosition();
    }

    // Update is called once per frame
    void Update()
    {
        _handleDrag();
    }
}
