using UnityEngine;
using System.Collections;

namespace Orbox.Utils
{
    public static class RenderHelper
    {

        public static void Copy(Texture source, RenderTexture result)
        {

            var active = RenderTexture.active;
            RenderTexture.active = result;

            //Saves both projection and modelview matrices to the matrix stack.
            GL.PushMatrix();

            //Setup a matrix for pixel-correct rendering.
            GL.LoadPixelMatrix(0, source.width, source.height, 0);

            GL.Clear(true, true, Color.black);

            //Draws a full-screen quad with background texture
            Graphics.Blit(source, result);

            //Restores both projection and modelview matrices off the top of the matrix stack.
            GL.PopMatrix();

            //De-activate my RenderTexture.
            //RenderTexture.active = null;

            //Restore active
            RenderTexture.active = active;


        }

        public static void Stamp(Texture background, Texture stamp, Material material, RenderTexture result, int x, int y)
        {
            //Set my RenderTexture active so DrawTexture will draw to it.
            var active = RenderTexture.active;
            RenderTexture.active = result;

            //Saves both projection and modelview matrices to the matrix stack.
            GL.PushMatrix();

            //Setup a matrix for pixel-correct rendering.
            GL.LoadPixelMatrix(0, background.width, background.height, 0);

            GL.Clear(true, true, Color.black);

            //Draws a full-screen quad with background texture
            Graphics.Blit(background, result);

            //Draw stamp texture on RenderTexture positioned by x and y in pixels coordinate
            var rect = new Rect(x, y, stamp.width, stamp.height);
            Graphics.DrawTexture(rect, stamp, material);

            //Restores both projection and modelview matrices off the top of the matrix stack.
            GL.PopMatrix();

            //De-activate my RenderTexture.
            //RenderTexture.active = null;

            //Restore active
            RenderTexture.active = active;
        }
    }
}