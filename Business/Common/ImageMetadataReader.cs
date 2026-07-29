namespace Plantilla_Base.Business.Common
{
    // Utilidad comun para leer bytes y dimensiones de imagenes sin cargar librerias externas.
    public static class ImageMetadataReader
    {
        public static async Task<byte[]> ObtenerBytesImagen(IFormFile imagen)
        {
            using var memoryStream = new MemoryStream();
            await imagen.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static (int Width, int Height)? ObtenerDimensionesImagen(byte[] bytes, string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                ".png" => ObtenerDimensionesPng(bytes),
                ".jpg" => ObtenerDimensionesJpeg(bytes),
                ".jpeg" => ObtenerDimensionesJpeg(bytes),
                ".gif" => ObtenerDimensionesGif(bytes),
                ".webp" => ObtenerDimensionesWebp(bytes),
                ".ico" => ObtenerDimensionesIco(bytes),
                _ => null
            };
        }

        private static (int Width, int Height)? ObtenerDimensionesPng(byte[] bytes)
        {
            if (bytes.Length < 24
                || bytes[0] != 0x89
                || bytes[1] != 0x50
                || bytes[2] != 0x4E
                || bytes[3] != 0x47)
            {
                return null;
            }

            return (LeerInt32BigEndian(bytes, 16), LeerInt32BigEndian(bytes, 20));
        }

        private static (int Width, int Height)? ObtenerDimensionesGif(byte[] bytes)
        {
            if (bytes.Length < 10
                || bytes[0] != 0x47
                || bytes[1] != 0x49
                || bytes[2] != 0x46)
            {
                return null;
            }

            return (LeerUInt16LittleEndian(bytes, 6), LeerUInt16LittleEndian(bytes, 8));
        }

        private static (int Width, int Height)? ObtenerDimensionesIco(byte[] bytes)
        {
            if (bytes.Length < 8
                || bytes[0] != 0x00
                || bytes[1] != 0x00
                || bytes[2] != 0x01
                || bytes[3] != 0x00)
            {
                return null;
            }

            var ancho = bytes[6] == 0 ? 256 : bytes[6];
            var alto = bytes[7] == 0 ? 256 : bytes[7];
            return (ancho, alto);
        }

        private static (int Width, int Height)? ObtenerDimensionesJpeg(byte[] bytes)
        {
            if (bytes.Length < 4 || bytes[0] != 0xFF || bytes[1] != 0xD8)
            {
                return null;
            }

            var posicion = 2;
            while (posicion + 9 < bytes.Length)
            {
                if (bytes[posicion] != 0xFF)
                {
                    posicion++;
                    continue;
                }

                while (posicion < bytes.Length && bytes[posicion] == 0xFF)
                {
                    posicion++;
                }

                if (posicion >= bytes.Length)
                {
                    return null;
                }

                var marcador = bytes[posicion++];
                if (marcador is 0xD8 or 0xD9 or 0x01)
                {
                    continue;
                }

                if (posicion + 1 >= bytes.Length)
                {
                    return null;
                }

                var longitud = LeerUInt16BigEndian(bytes, posicion);
                if (longitud < 2 || posicion + longitud > bytes.Length)
                {
                    return null;
                }

                if (EsMarcadorSofJpeg(marcador))
                {
                    if (posicion + 7 >= bytes.Length)
                    {
                        return null;
                    }

                    var alto = LeerUInt16BigEndian(bytes, posicion + 3);
                    var ancho = LeerUInt16BigEndian(bytes, posicion + 5);
                    return (ancho, alto);
                }

                posicion += longitud;
            }

            return null;
        }

        private static (int Width, int Height)? ObtenerDimensionesWebp(byte[] bytes)
        {
            if (bytes.Length < 30
                || bytes[0] != 0x52
                || bytes[1] != 0x49
                || bytes[2] != 0x46
                || bytes[3] != 0x46
                || bytes[8] != 0x57
                || bytes[9] != 0x45
                || bytes[10] != 0x42
                || bytes[11] != 0x50)
            {
                return null;
            }

            var chunk = System.Text.Encoding.ASCII.GetString(bytes, 12, 4);
            if (chunk == "VP8 " && bytes.Length >= 30)
            {
                return (LeerUInt16LittleEndian(bytes, 26) & 0x3FFF, LeerUInt16LittleEndian(bytes, 28) & 0x3FFF);
            }

            if (chunk == "VP8L" && bytes.Length >= 25)
            {
                var b0 = bytes[21];
                var b1 = bytes[22];
                var b2 = bytes[23];
                var b3 = bytes[24];
                var ancho = 1 + (((b1 & 0x3F) << 8) | b0);
                var alto = 1 + (((b3 & 0x0F) << 10) | (b2 << 2) | ((b1 & 0xC0) >> 6));
                return (ancho, alto);
            }

            if (chunk == "VP8X" && bytes.Length >= 30)
            {
                var ancho = 1 + LeerInt24LittleEndian(bytes, 24);
                var alto = 1 + LeerInt24LittleEndian(bytes, 27);
                return (ancho, alto);
            }

            return null;
        }

        private static bool EsMarcadorSofJpeg(byte marcador)
        {
            return marcador is 0xC0 or 0xC1 or 0xC2 or 0xC3 or 0xC5 or 0xC6 or 0xC7 or 0xC9 or 0xCA or 0xCB or 0xCD or 0xCE or 0xCF;
        }

        private static int LeerInt32BigEndian(byte[] bytes, int offset)
        {
            return (bytes[offset] << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3];
        }

        private static int LeerUInt16BigEndian(byte[] bytes, int offset)
        {
            return (bytes[offset] << 8) | bytes[offset + 1];
        }

        private static int LeerUInt16LittleEndian(byte[] bytes, int offset)
        {
            return bytes[offset] | (bytes[offset + 1] << 8);
        }

        private static int LeerInt24LittleEndian(byte[] bytes, int offset)
        {
            return bytes[offset] | (bytes[offset + 1] << 8) | (bytes[offset + 2] << 16);
        }
    }
}
