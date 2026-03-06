using UnityEngine;
using UnityEngine.EventSystems;
using deVoid.Utils;

namespace SpaceInvaders.Project.Input
{
    public class HorizontalTouchButtons : MonoBehaviour,
        IPointerDownHandler,
        IDragHandler,
        IPointerUpHandler,
        IEndDragHandler
    {
        private enum ActiveZone
        {
            None,
            Left,
            Right
        }

        private RectTransform rectTransform;
        private int activePointerId = int.MinValue;
        private bool isTracking;
        private ActiveZone currentZone = ActiveZone.None;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isTracking)
                return;

            isTracking = true;
            activePointerId = eventData.pointerId;

            UpdateZone(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isTracking || eventData.pointerId != activePointerId)
                return;

            UpdateZone(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isTracking || eventData.pointerId != activePointerId)
                return;

            ReleaseAll();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isTracking || eventData.pointerId != activePointerId)
                return;

            ReleaseAll();
        }

        private void OnDisable()
        {
            ReleaseAll();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                ReleaseAll();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                ReleaseAll();
        }

        private void UpdateZone(PointerEventData eventData)
        {
            ActiveZone newZone = GetZoneAtScreenPosition(eventData.position, eventData.pressEventCamera);

            if (newZone == currentZone)
                return;

            SetZone(newZone);
        }

        private ActiveZone GetZoneAtScreenPosition(Vector2 screenPosition, Camera eventCamera)
        {
            if (rectTransform == null)
                return ActiveZone.None;

            Vector2 localPoint;
            bool inside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                screenPosition,
                eventCamera,
                out localPoint);

            if (!inside)
                return ActiveZone.None;

            Rect rect = rectTransform.rect;

            if (!rect.Contains(localPoint))
                return ActiveZone.None;

            float midpointX = rect.xMin + rect.width * 0.5f;

            if (localPoint.x < midpointX)
                return ActiveZone.Left;
            else
                return ActiveZone.Right;
        }

        private void SetZone(ActiveZone newZone)
        {
            if (currentZone == newZone)
                return;

            switch (currentZone)
            {
                case ActiveZone.Left:
                    Signals.Get<OnLeftButtonSignal>().Dispatch(false);
                    break;

                case ActiveZone.Right:
                    Signals.Get<OnRightButtonSignal>().Dispatch(false);
                    break;
            }

            currentZone = newZone;

            switch (currentZone)
            {
                case ActiveZone.Left:
                    Signals.Get<OnLeftButtonSignal>().Dispatch(true);
                    break;

                case ActiveZone.Right:
                    Signals.Get<OnRightButtonSignal>().Dispatch(true);
                    break;
            }
        }

        private void ReleaseAll()
        {
            switch (currentZone)
            {
                case ActiveZone.Left:
                    Signals.Get<OnLeftButtonSignal>().Dispatch(false);
                    break;

                case ActiveZone.Right:
                    Signals.Get<OnRightButtonSignal>().Dispatch(false);
                    break;
            }

            currentZone = ActiveZone.None;
            isTracking = false;
            activePointerId = int.MinValue;
        }
    }
}