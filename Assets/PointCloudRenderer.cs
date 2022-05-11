using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PointCloudRenderer : MonoBehaviour {
    Texture2D texColor;
    Texture2D texPosScale;
    VisualEffect vfx;
    uint resolution = 2048;

    public float particleSize = 0.1f;
    bool toUpdate = false;
    uint particleCount = 0;

    private void Start() {
        vfx = GetComponent<VisualEffect>();

        Vector3[] vector3 = new Vector3[1000];
        Color[] colors = new Color[1000];

        for (int i = 0; i < 1000; i++) {
            vector3[i] = new Vector3(Random.value * 10, Random.value * 10, Random.value * 10);
            colors[i] = new Color(Random.value, Random.value, Random.value);
        }

        SetParticles(vector3, colors);
    }

    private void Update() {
        if (toUpdate) {
            toUpdate = false;

            vfx.Reinit();
            vfx.SetUInt(Shader.PropertyToID("ParticleCount"), particleCount);
            vfx.SetTexture(Shader.PropertyToID("TexColor"), texColor);
            vfx.SetTexture(Shader.PropertyToID("TexPosScale"), texPosScale);
            vfx.SetUInt(Shader.PropertyToID("Resolution"), resolution);
        }
    }

    public void SetParticles(Vector3[] positions, Color[] colors) {
        texColor = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length, Mathf.Clamp(positions.Length / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        texPosScale = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length, Mathf.Clamp(positions.Length / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);
        int texWidth = texColor.width;
        int texHeight = texColor.height;

        for (int y = 0; y < texHeight; y++) {
            for (int x = 0; x < texWidth; x++) {
                int index = x + y * texWidth;
                texColor.SetPixel(x, y, colors[index]);
                var data = new Color(positions[index].x, positions[index].y, positions[index].z, particleSize);
                texPosScale.SetPixel(x, y, data);
            }
        }

        texColor.Apply();
        texPosScale.Apply();
        particleCount = (uint)positions.Length;
        toUpdate = true;
    }
}
