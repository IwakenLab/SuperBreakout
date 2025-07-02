using UnityEngine;

namespace SuperBreakout.Core
{
    public static class SpriteHelper
    {
        public static Sprite CreateSquareSprite()
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }
        
        public static Sprite CreateCircleSprite(int resolution = 32)
        {
            int size = resolution * 2;
            Texture2D texture = new Texture2D(size, size);
            Color[] colors = new Color[size * size];
            
            Vector2 center = new Vector2(resolution, resolution);
            
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    colors[y * size + x] = distance <= resolution ? Color.white : Color.clear;
                }
            }
            
            texture.SetPixels(colors);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), resolution * 2);
        }
    }
}