using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PixelDrawer.Model;

namespace PixelDrawer
{
    static class FileSaveLoad
    {
        public static void SaveFile(string path, TestProject project)
        {
            File.WriteAllBytesAsync(path, ConvertPictureToBytes(project));
        }

        public static byte[] ConvertPictureToBytes(TestProject project) 
        {
            return new byte[0];
        }
    }
}
