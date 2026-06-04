using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PhaseShift.Core;

public static class TextureFactory
{
    public static Texture2D CreatePlayerSolid(GraphicsDevice gd)
    {
        int w = 32, h = 48;
        var data = new Color[w * h];
        for (int i = 0; i < data.Length; i++) data[i] = Color.Transparent;

        // Плащ 
        FillRect(data, w, 2, 20, 28, 28, new Color(20, 40, 120, 255));
        // Края плаща светлее
        FillRect(data, w, 2, 20, 2, 28, new Color(40, 80, 180, 255));
        FillRect(data, w, 28, 20, 2, 28, new Color(40, 80, 180, 255));
        
        for (int x = 2; x < 30; x++)
        {
            int jagY = 44 + (x % 3 == 0 ? 2 : x % 3 == 1 ? 0 : 1);
            if (jagY < h) data[jagY * w + x] = Color.Transparent;
            if (jagY - 1 < h && jagY - 1 >= 0)
                data[(jagY - 1) * w + x] = new Color(20, 40, 120, 200);
        }

        // Голова
        FillEllipse(data, w, 16, 9, 9, 8, new Color(200, 210, 220, 255));
        // Глазницы
        FillEllipse(data, w, 11, 8, 3, 3, new Color(5, 5, 20, 255));
        FillEllipse(data, w, 21, 8, 3, 3, new Color(5, 5, 20, 255));
        // Светящиеся зрачки
        data[8 * w + 11] = new Color(80, 150, 255, 255);
        data[8 * w + 21] = new Color(80, 150, 255, 255);
        // Нос
        data[11 * w + 16] = new Color(5, 5, 20, 255);
        // Рот и зубы
        for (int x = 13; x <= 19; x++)
            data[13 * w + x] = new Color(5, 5, 20, 255);
        for (int x = 13; x <= 19; x += 2)
            data[14 * w + x] = new Color(200, 210, 220, 255);

        // Свечение вокруг глаз
        data[7 * w + 10] = new Color(60, 120, 255, 100);
        data[7 * w + 22] = new Color(60, 120, 255, 100);
        data[9 * w + 10] = new Color(60, 120, 255, 100);
        data[9 * w + 22] = new Color(60, 120, 255, 100);

        var tex = new Texture2D(gd, w, h);
        tex.SetData(data);
        return tex;
    }

    // Персонаж Ghost 
    public static Texture2D CreatePlayerGhost(GraphicsDevice gd)
    {
        int w = 32, h = 48;
        var data = new Color[w * h];
        for (int i = 0; i < data.Length; i++) data[i] = Color.Transparent;

        // Тело полупрозрачное фиолетовое облако
        for (int y = 10; y < 44; y++)
        for (int x = 2; x < 30; x++)
        {
            float dx = (x - 16) / 14f;
            float dy = (y - 27) / 17f;
            float dist = dx * dx + dy * dy;
            if (dist < 1f)
            {
                byte alpha = (byte)(180 * (1f - dist));
                data[y * w + x] = new Color((byte)120, (byte)60, (byte)220, alpha);
            }
        }

        // Волнистый нижний край
        for (int x = 4; x < 28; x++)
        {
            int waveY = 40 + (int)(Math.Sin(x * 0.8f) * 3);
            for (int y = waveY; y < h; y++)
                data[y * w + x] = Color.Transparent;
        }

        // Голова размытый череп
        FillEllipse(data, w, 16, 9, 8, 7, new Color(180, 140, 255, 200));
        // Глазницы светящиеся фиолетовые
        FillEllipse(data, w, 11, 8, 3, 3, new Color(10, 0, 30, 230));
        FillEllipse(data, w, 21, 8, 3, 3, new Color(10, 0, 30, 230));
        data[8 * w + 11] = new Color(200, 100, 255, 255);
        data[8 * w + 21] = new Color(200, 100, 255, 255);
        // Свечение глаз
        data[7 * w + 11] = new Color(180, 80, 255, 120);
        data[9 * w + 11] = new Color(180, 80, 255, 120);
        data[7 * w + 21] = new Color(180, 80, 255, 120);
        data[9 * w + 21] = new Color(180, 80, 255, 120);
        // Рот кривая улыбка
        for (int x = 13; x <= 19; x++)
            data[13 * w + x] = new Color(10, 0, 30, 200);

        var tex = new Texture2D(gd, w, h);
        tex.SetData(data);
        return tex;
    }

    // Платформа Solid — каменная плита с рунами
    public static Texture2D CreatePlatformSolid(GraphicsDevice gd, int width, int height)
    {
        var data = new Color[width * height];

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            // Верхняя светящаяся полоска
            if (y == 0)
                data[y * width + x] = new Color(80, 140, 255, 255);
            else if (y == 1)
                data[y * width + x] = new Color(50, 100, 200, 200);
            // Трещины вертикальные
            else if (x % 20 == 0 && y > 2)
                data[y * width + x] = new Color(10, 15, 50, 255);
            // Горизонтальная трещина посередине
            else if (y == height / 2 && x % 5 < 3)
                data[y * width + x] = new Color(15, 25, 70, 255);
            // Основной камень — тёмно-синий с вариацией
            else
            {
                int var = (x * 3 + y * 7) % 20;
                data[y * width + x] = new Color(25 + var, 45 + var, 110 + var, 255);
            }
        }

        // Руны — светящиеся точки
        for (int i = 15; i < width - 10; i += 25)
        {
            if (i + 1 < width)
            {
                int ry = height / 2;
                data[ry * width + i]     = new Color(80, 140, 255, 200);
                data[(ry-1) * width + i] = new Color(60, 110, 200, 150);
            }
        }

        var tex = new Texture2D(gd, width, height);
        tex.SetData(data);
        return tex;
    }

    // Платформа Ghost — полупрозрачная с фиолетовым свечением
    public static Texture2D CreatePlatformGhost(GraphicsDevice gd, int width, int height)
    {
        var data = new Color[width * height];

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            if (y == 0)
                data[y * width + x] = new Color(180, 100, 255, 230);
            else if (y == 1)
                data[y * width + x] = new Color(130, 70, 220, 180);
            // Волнистый узор
            else if ((int)(Math.Sin(x * 0.3f) * 2) == y - height / 2)
                data[y * width + x] = new Color(100, 50, 200, 180);
            else
            {
                // Полупрозрачная фиолетовая с вариацией
                int var = (x * 5 + y * 3) % 15;
                data[y * width + x] = new Color(60 + var, 30 + var, 150 + var, 140);
            }
        }

        var tex = new Texture2D(gd, width, height);
        tex.SetData(data);
        return tex;
    }

    // Стена Solid — тёмные кирпичи с синим свечением по краям
    public static Texture2D CreateWallSolid(GraphicsDevice gd, int width, int height)
    {
        var data = new Color[width * height];

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            bool isEdge  = x == 0 || x == width - 1;
            bool isMortar = y % 12 == 0 || (y % 12 < 6 ? x == width/2 : x == width/2 + 3);

            if (isEdge)
                data[y * width + x] = new Color(60, 120, 255, 220);
            else if (isMortar)
                data[y * width + x] = new Color(8, 15, 50, 255);
            else
            {
                int var = (x + y * 3) % 15;
                data[y * width + x] = new Color(20 + var, 40 + var, 100 + var, 255);
            }
        }

        var tex = new Texture2D(gd, width, height);
        tex.SetData(data);
        return tex;
    }

    // Стена Ghost — фиолетовая с волнистыми краями
    public static Texture2D CreateWallGhost(GraphicsDevice gd, int width, int height)
    {
        var data = new Color[width * height];

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            bool isEdge  = x == 0 || x == width - 1;
            bool isMortar = y % 12 == 0 || (y % 12 < 6 ? x == width/2 : x == width/2 + 3);

            if (isEdge)
                data[y * width + x] = new Color(160, 80, 255, 200);
            else if (isMortar)
                data[y * width + x] = new Color(20, 8, 60, 200);
            else
            {
                int var = (x + y * 3) % 15;
                data[y * width + x] = new Color(55 + var, 25 + var, 140 + var, 160);
            }
        }

        var tex = new Texture2D(gd, width, height);
        tex.SetData(data);
        return tex;
    }

    // Шипы — готические тёмно-красные с костяными кончиками
    public static Texture2D CreateSpike(GraphicsDevice gd, int width, int height)
    {
        var data = new Color[width * height];
        for (int i = 0; i < data.Length; i++) data[i] = Color.Transparent;

        int spikeW = 10;
        int count  = width / spikeW;

        for (int s = 0; s < count; s++)
        for (int x = 0; x < spikeW; x++)
        for (int y = 0; y < height; y++)
        {
            int gx = s * spikeW + x;
            // Треугольник: чем выше — уже
            float progress = 1f - (float)y / height; // 0 внизу, 1 вверху
            float halfW    = progress * (spikeW / 2f);
            float center   = spikeW / 2f;

            if (Math.Abs(x - center) < halfW)
            {
                bool isTip  = y < 3 && Math.Abs(x - center) < halfW * 0.4f;
                bool isEdge = Math.Abs(Math.Abs(x - center) - halfW) < 1.2f;

                if (isTip)
                    data[y * width + gx] = new Color(220, 220, 210, 255); // костяной кончик
                else if (isEdge)
                    data[y * width + gx] = new Color(120, 10, 10, 255);   // тёмный край
                else
                {
                    // Градиент от тёмно-красного внизу к ярче вверху
                    byte r = (byte)(100 + (int)(progress * 80));
                    data[y * width + gx] = new Color(r, (byte)15, (byte)15, (byte)255);
                }
            }
        }

        var tex = new Texture2D(gd, width, height);
        tex.SetData(data);
        return tex;
    }

    // Выход портал с вращающимся свечением
    public static Texture2D CreateExit(GraphicsDevice gd, int width, int height)
    {
        var data = new Color[width * height];
        for (int i = 0; i < data.Length; i++) data[i] = Color.Transparent;

        float cx = width / 2f, cy = height / 2f;
        float rx = width * 0.42f, ry = height * 0.42f;

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            float dx   = (x - cx) / rx;
            float dy   = (y - cy) / ry;
            float dist = dx * dx + dy * dy;

            if (dist < 0.15f)
                // Яркий центр
                data[y * width + x] = new Color(180, 255, 220, 255);
            else if (dist < 0.5f)
                // Зелёное свечение
                data[y * width + x] = Color.Lerp(
                    new Color(20, 200, 120, 230),
                    new Color(10, 80, 60, 180),
                    (dist - 0.15f) / 0.35f);
            else if (dist < 0.85f)
            {
                // Тёмный край с рунической рамкой
                bool isRune = (int)(Math.Atan2(dy, dx) * 4 / Math.PI) % 2 == 0;
                data[y * width + x] = isRune
                    ? new Color(30, 120, 80, 150)
                    : new Color(5, 40, 30, 120);
            }
        }

        // Символ двери вертикальные полосы
        for (int y = height/4; y < height*3/4; y++)
        {
            int mx = (int)cx;
            data[y * width + mx]     = new Color(200, 255, 220, 180);
            data[y * width + mx - 1] = new Color(150, 220, 180, 120);
            data[y * width + mx + 1] = new Color(150, 220, 180, 120);
        }

        var tex = new Texture2D(gd, width, height);
        tex.SetData(data);
        return tex;
    }

    // Фон 
    public static Texture2D CreateBackground(GraphicsDevice gd, int width, int height)
    {
        var rng  = new Random(42);
        var data = new Color[width * height];

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            float t = y / (float)height;
            data[y * width + x] = Color.Lerp(new Color(5, 5, 20), new Color(15, 8, 35), t);
        }

        for (int i = 0; i < 200; i++)
        {
            int sx = rng.Next(width);
            int sy = rng.Next(height / 2);
            int b  = rng.Next(120, 255);
            data[sy * width + sx] = new Color(b, b, b + 20, 255);
        }

        for (int y = height / 2; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            float fog  = (float)(y - height / 2) / (height / 2) * 0.3f;
            float wave = (float)Math.Sin(x * 0.02f + y * 0.05f) * 0.1f;
            fog = Math.Clamp(fog + wave, 0f, 0.4f);
            data[y * width + x] = Color.Lerp(data[y * width + x],
                new Color(30, 10, 60, (int)(fog * 255)), fog);
        }

        var tex = new Texture2D(gd, width, height);
        tex.SetData(data);
        return tex;
    }

    // Земля тёмный камень
    public static Texture2D CreateGround(GraphicsDevice gd, int width, int height)
    {
        var data = new Color[width * height];

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            if (y < 4)
                data[y * width + x] = new Color(50, 50, 70, 255);
            else if (x % 24 == 0 && y > 4 || x % 24 == 12 && y > 12)
                data[y * width + x] = new Color(10, 10, 20, 255);
            else
                data[y * width + x] = new Color(25, 25, 40, 255);
        }

        var tex = new Texture2D(gd, width, height);
        tex.SetData(data);
        return tex;
    }

    // Вспомогательные методы
    private static void FillRect(Color[] data, int w, int x, int y, int rw, int rh, Color c)
    {
        for (int dy = 0; dy < rh; dy++)
        for (int dx = 0; dx < rw; dx++)
        {
            int px = x + dx, py = y + dy;
            if (px >= 0 && px < w && py >= 0 && py < data.Length / w)
                data[py * w + px] = c;
        }
    }

    private static void FillEllipse(Color[] data, int w, int cx, int cy, int rx, int ry, Color c)
    {
        for (int dy = -ry; dy <= ry; dy++)
        for (int dx = -rx; dx <= rx; dx++)
        {
            float ex = (float)dx / rx, ey = (float)dy / ry;
            if (ex * ex + ey * ey <= 1f)
            {
                int px = cx + dx, py = cy + dy;
                if (px >= 0 && px < w && py >= 0)
                    data[py * w + px] = c;
            }
        }
    }
}