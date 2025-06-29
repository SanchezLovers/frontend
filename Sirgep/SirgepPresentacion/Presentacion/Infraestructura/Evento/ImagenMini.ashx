<%@ WebHandler Language="C#" Class="ImagenMini" %>

using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class ImagenMini : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string rutaRelativa = context.Request.QueryString["src"];
        if (string.IsNullOrEmpty(rutaRelativa))
        {
            context.Response.StatusCode = 400;
            return;
        }

        string pathFisico = context.Server.MapPath(rutaRelativa);
        if (!File.Exists(pathFisico))
        {
            context.Response.StatusCode = 404;
            return;
        }

        using (Bitmap original = new Bitmap(pathFisico))
        {
            int ancho = 200;
            int alto = (int)(original.Height * (ancho / (double)original.Width));

            using (Bitmap mini = new Bitmap(original, ancho, alto))
            {
                context.Response.ContentType = "image/jpeg";
                mini.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            }
        }
    }

    public bool IsReusable => false;
}
