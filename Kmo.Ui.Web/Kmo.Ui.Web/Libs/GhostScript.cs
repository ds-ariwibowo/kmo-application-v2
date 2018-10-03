using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ghostscript.NET;
using System.Drawing;
using Ghostscript.NET.Processor;
using Ghostscript.NET.Rasterizer;
using System.IO;
using System.Drawing.Imaging;

namespace Kmo
{
    public static class GhostScript
    {
        public static Image[] RasterizePdf(byte[] PdfBinary)
        {
            return RasterizePdf(new MemoryStream(PdfBinary));
        }

        public static Image[] RasterizePdf(MemoryStream PdfBinary)
        {
            
            string dll32Path = AppDomain.CurrentDomain.BaseDirectory + "gsdll32.dll";
            string dll64Path = AppDomain.CurrentDomain.BaseDirectory + "gsdll64.dll";

            int dpi_X = int.Parse(Application.AppSettingsValue("GhostScriptRasterizeDpi_X"));
            int dpi_Y = int.Parse(Application.AppSettingsValue("GhostScriptRasterizeDpi_Y"));

            int requestPlatform = int.Parse(Application.AppSettingsValue("GhostScriptPlatform"));
            byte[] ghLib;

            if (requestPlatform == 32)
            {
                ghLib = File.ReadAllBytes(dll32Path);
            }
            else
            {
                ghLib = File.ReadAllBytes(dll64Path);
            }
            
            using (var _rasterizer = new GhostscriptRasterizer())
            {
                _rasterizer.Open(PdfBinary, ghLib);
                int total = _rasterizer.PageCount;
                List<Image> images = new List<Image>(total);
                for (int i = 1; i <= total; i++)
                {
                    images.Add(_rasterizer.GetPage(dpi_X, dpi_Y, i));
                }

                return images.ToArray();
            }
        }
    }
}