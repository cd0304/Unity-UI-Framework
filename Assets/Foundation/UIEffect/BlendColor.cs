using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/*
 *	
 *  Blend Color Using BaseVertexEffect
 *
 *	by Xuanyi
 *
 */


namespace UiEffect
{
    [AddComponentMenu ("UI/Effects/Blend Color")]
    [RequireComponent (typeof (Graphic))]
    public class BlendColor : BaseMeshEffect
    {
        public enum BLEND_MODE
        {
            Multiply,
            Additive,
            Subtractive,
            Override,
        }

        public BLEND_MODE blendMode = BLEND_MODE.Multiply;
        public Color color = Color.grey;

        Graphic graphic;

        //public override void ModifyMesh(List<UIVertex> vList)
        //{
        //    if (IsActive() == false || vList == null || vList.Count == 0)
        //    {
        //        return;
        //    }

        //    UIVertex tempVertex = vList[0];
        //    for (int i = 0; i < vList.Count; i++)
        //    {
        //        tempVertex = vList[i];
        //        byte orgAlpha = tempVertex.color.a;
        //        switch (blendMode)
        //        {
        //            case BLEND_MODE.Multiply:
        //                tempVertex.color *= color;
        //                break;
        //            case BLEND_MODE.Additive:
        //                tempVertex.color += color;
        //                break;
        //            case BLEND_MODE.Subtractive:
        //                tempVertex.color -= color;
        //                break;
        //            case BLEND_MODE.Override:
        //                tempVertex.color = color;
        //                break;
        //        }
        //        tempVertex.color.a = orgAlpha;
        //        vList[i] = tempVertex;
        //    }
        //}

        /// <summary>
        /// Refresh Blend Color on playing.
        /// </summary>
        public void Refresh ()
        {
            if (graphic == null) {
                graphic = GetComponent<Graphic> ();
            }
            if (graphic != null) {
                graphic.SetVerticesDirty ();
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;
            UIVertex vert = new UIVertex();
            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vert, i);
                //vert.uv1.x = (i >> 1);
                //vert.uv1.y = ((i >> 1) ^ (i & 1));
                byte orgAlpha = vert.color.a;
                switch (blendMode)
                {
                    case BLEND_MODE.Multiply:
                        vert.color *= color;
                        break;
                    case BLEND_MODE.Additive:
                        vert.color += color;
                        break;
                    case BLEND_MODE.Subtractive:
                        vert.color -= color;
                        break;
                    case BLEND_MODE.Override:
                        vert.color = color;
                        break;
                }
                vert.color.a = orgAlpha;
                vh.SetUIVertex(vert, i);
            }
        }
    }
}
