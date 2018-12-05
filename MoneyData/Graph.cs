using System;
using System.Drawing;

namespace dotNET_Currencies
{
    public class Graph
    {
        private Currencies database;

        private static readonly int IMG_WIDTH = 800;
        private static readonly int IMG_HEIGHT = 600;

        private static readonly int PAD_SM = 30;
        private static readonly int PAD_BG = 60;

        private static readonly int RATE_MAX = 60;
        private static readonly int RATE_STEP = 5;

        private static readonly int DATE_N = 10;

        private Graphics graphics;

        public Graph(Currencies currencies)
        {
            this.database = currencies;
        }

        public void Draw()
        {
            using (var bitmap = new Bitmap(IMG_WIDTH, IMG_HEIGHT))
            {
                using (graphics = Graphics.FromImage(bitmap))
                {
                    DrawBackground();
                    DrawRateAxe();
                }
                bitmap.Save("currency.bmp");
            }
        }

        private void DrawBackground()
        {
            graphics.FillRectangle(
                new SolidBrush(Color.Red),
                0, 0,
                IMG_WIDTH, IMG_HEIGHT
            );

        }

        private void DrawRateAxe()
        {
            var clr = Color.LightSlateGray;
            var pen = new Pen(clr, 2);

            // Axe + Grid
            int len = IMG_HEIGHT - PAD_SM - PAD_BG;
            int n_pt = RATE_MAX / RATE_STEP;
            int dif = len / n_pt;

            int x = IMG_WIDTH - PAD_BG;
            int pt_l = 6;
            int pt_x = x - pt_l;
            int pt_y_base = IMG_HEIGHT - PAD_BG;

            // Axe
            graphics.DrawLine(
                pen,
                x, PAD_SM,
                x, pt_y_base
            );

            for (int i = 1; i <= n_pt; i++)
            {
                int pt_y = pt_y_base - dif * i;

                // Grid
                graphics.DrawLine(
                    new Pen(Color.LightGray, 1)
                    {
                        DashPattern = new float[] { 5, 3 }
                    },
                    PAD_SM, pt_y,
                    IMG_WIDTH - PAD_BG, pt_y
                );

                // Axe points
                graphics.DrawLine(
                    pen,
                    pt_x, pt_y,
                    pt_x + pt_l, pt_y
                );

                // Values
                string rate_val = string.Format("{0} CZK", RATE_STEP * i);
                graphics.DrawString(
                    rate_val,
                    new Font(FontFamily.GenericSansSerif, 10),
                    new SolidBrush(clr),
                    pt_x + pt_l + 4, pt_y
                );
            }
        }

        private void DrawDateAxe()
        {
            var clr = Color.LightSlateGray;
            var pen = new Pen(clr, 2);

            // Axe + Grid
            int len = IMG_WIDTH - PAD_SM - PAD_BG;

            int pt_l = 6;
            int pt_y = IMG_HEIGHT - PAD_BG;
            int pt_x_base = IMG_WIDTH - PAD_BG;

            // Axe
            //graphics.dra
        }
    }
}