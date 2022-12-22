namespace UP08
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Popup : UILayout
    {
        [SerializeField] protected Canvas canvas;

        public void Focus()
        {
            PopupController.Focus(this);
        }


        public void SetCanvasOrder(int order)
        {
            canvas.sortingOrder = order;
        }
    }
}