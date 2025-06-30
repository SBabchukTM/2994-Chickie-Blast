using System;
using UnityEngine;

namespace Runtime.Game.UserAccountSystem
{
    public class ImageProcessorHelper
    {
        /// <summary>
        /// Converts a Sprite to a Base64 PNG string, resizing it if needed.
        /// </summary>
        public string ConvertToBase64(Sprite sprite, int maxSize)
        {
            Texture2D resizedTexture = ResizeTexture(sprite.texture, maxSize);
            byte[] pngData = resizedTexture.EncodeToPNG();
            return Convert.ToBase64String(pngData);
        }

        /// <summary>
        /// Creates a Sprite from a Base64 PNG string.
        /// </summary>
        public Sprite CreateSpriteFromBase64(string base64)
        {
            byte[] imageData = Convert.FromBase64String(base64);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// Resizes a texture to fit within maxSize, preserving aspect ratio.
        /// </summary>
        private Texture2D ResizeTexture(Texture2D source, int maxSize)
        {
            int width = source.width;
            int height = source.height;

            if (maxSize > 0)
            {
                float scale = Mathf.Min((float)maxSize / width, (float)maxSize / height);
                width = Mathf.RoundToInt(width * scale);
                height = Mathf.RoundToInt(height * scale);
            }

            RenderTexture rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
            Graphics.Blit(source, rt);

            RenderTexture previousRT = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            result.Apply();

            RenderTexture.active = previousRT;
            RenderTexture.ReleaseTemporary(rt);

            return result;
        }
    }
}