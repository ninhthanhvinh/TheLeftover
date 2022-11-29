using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheLeftOver
{
    public class LayerAdjustment : MonoBehaviour
    {
        public string layer;
        public string sortingLayer;

        private void OnTriggerExit2D(Collider2D collision)
        {
            collision.gameObject.layer = LayerMask.NameToLayer(layer);
            collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
            SpriteRenderer[] srs = collision.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                sr.sortingLayerName = sortingLayer;
            }
        }
    }
}