/*---------------------------------------------------------------------------*/
/* Copyright(C)  2018  OMRON Corporation                                     */
/*                                                                           */
/* Licensed under the Apache License, Version 2.0 (the "License");           */
/* you may not use this file except in compliance with the License.          */
/* You may obtain a copy of the License at                                   */
/*                                                                           */
/*     http://www.apache.org/licenses/LICENSE-2.0                            */
/*                                                                           */
/* Unless required by applicable law or agreed to in writing, software       */
/* distributed under the License is distributed on an "AS IS" BASIS,         */
/* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  */
/* See the License for the specific language governing permissions and       */
/* limitations under the License.                                            */
/*---------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class GrayscaleImage
{
    public int width;
    public int height;
    public byte[] data;

    public GrayscaleImage()
    {
        this.width = 0;
        this.height = 0;
    }

    public void save(String fname)
    {
        // if no data, no save.
        if ((this.width == 0) || (this.height == 0))
        {
            return;
        }

        using (var img = new Bitmap(this.width, this.height, PixelFormat.Format8bppIndexed))
        {

            ColorPalette pal = img.Palette;
            for (var i = 0; i < 256; ++i)
            {
                pal.Entries[i] = Color.FromArgb(i, i, i);
            }
            img.Palette = pal;

            var bmpdata = img.LockBits(new Rectangle(0, 0, this.width, this.height),
                                              ImageLockMode.WriteOnly,
                                              PixelFormat.Format8bppIndexed);
            try
            {
                Marshal.Copy(this.data, 0, bmpdata.Scan0, this.data.Length);
            }
            finally
            {
                img.UnlockBits(bmpdata);
            }
            img.Save(fname, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}

