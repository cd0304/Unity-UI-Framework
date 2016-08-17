using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[AddComponentMenu("UI/Effects/Gradient")]
public class Gradient : BaseMeshEffect
{
    [SerializeField]
    private Color32
        topColor = Color.white;
    [SerializeField]
    private Color32
        bottomColor = Color.black;

    public override void ModifyMesh(Mesh mesh)
    {
        if (!IsActive())
        {
            return;
        }

        Vector3[] vertexList = mesh.vertices;
        int count = mesh.vertexCount;
        if (count > 0)
        {
            float bottomY = vertexList[0].y;
            float topY = vertexList[0].y;

            for (int i = 1; i < count; i++)
            {
                float y = vertexList[i].y;
                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }
            }
            List<Color32> colors = new List<Color32>();
            float uiElementHeight = topY - bottomY;
            for (int i = 0; i < count; i++)
            {
                colors.Add(Color32.Lerp(bottomColor, topColor, (vertexList[i].y - bottomY) / uiElementHeight));
            }
            mesh.SetColors(colors);
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0)
            return;

        List<UIVertex> vertices = new List<UIVertex>();
        vh.GetUIVertexStream(vertices);

        float bottomY = vertices[0].position.y;
        float topY = vertices[0].position.y;

        for (int i = 1; i < vertices.Count; i++)
        {
            float y = vertices[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

        float uiElementHeight = topY - bottomY;

        UIVertex v = new UIVertex();

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref v, i);
            v.color = Color32.Lerp(bottomColor, topColor, (v.position.y - bottomY) / uiElementHeight);
            vh.SetUIVertex(v, i);
        }

        for (int i = 0; i < vertices.Count && vertices.Count - i >= 6;)
        {
            ChangeColor(ref vertices, i, topColor);
            ChangeColor(ref vertices, i + 1, topColor);
            ChangeColor(ref vertices, i + 2, bottomColor);
            ChangeColor(ref vertices, i + 3, bottomColor);
            ChangeColor(ref vertices, i + 4, bottomColor);
            ChangeColor(ref vertices, i + 5, topColor);
            i += 6;
        }
    }

    private void ChangeColor(ref List<UIVertex> verList, int index, Color color)
    {
        UIVertex temp = verList[index];
        temp.color = color;
        verList[index] = temp;
    }
}
