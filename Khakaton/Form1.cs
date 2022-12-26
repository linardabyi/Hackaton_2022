using Khakaton.DataHandler;
using Khakaton.DataHandler2Step;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace Khakaton
{
    public partial class Form1 : Form
    {
        Graphics g;
        DataDTO data;
        int scale = 11;
        ApiConnector apiConnector;
        DataProcesser dataProcesser;
        Data2Processer data2Processer;
        public Form1()
        {
            InitializeComponent();
            apiConnector = new ApiConnector();
            dataProcesser = new DataProcesser();
            data2Processer = new Data2Processer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                        
        }        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //string mapData = apiConnector.GetMapData();
            string map2Data = apiConnector.GetChildAndGiftsMap();
            Data2DTO data2 = data2Processer.GetData(map2Data);

            List<PresentingGiftDTO> order = data2Processer.PresentGifts();

            Result2DTO dto = new();
            dto.mapID = "a8e01288-28f8-45ee-9db4-f74fc4ff02c8";
            dto.presentingGifts = order.ToArray();

            var json = JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            int a = 5;

            /*data = dataProcesser.GetData(mapData);
            g = e.Graphics;
            dataProcesser.SetGraphic(g);
            PaintCircles();
            PaintDots();
            while (true)
            {
                var route = dataProcesser.GetRoute(data);
                if (route == null)
                {
                    break;
                }
                PaintRoute(route);
                //System.Threading.Thread.Sleep(5000);
            }
            string usefullInformation = string.Empty;
            int notHappyChildren = data.children.Count(child => !child.visited);
            int bagsCount = dataProcesser.Bags.Count();
            if (notHappyChildren == 0)
            {
                usefullInformation = $"All children get their presents. {bagsCount} bags";
            }
            else
            {
                usefullInformation = $"{notHappyChildren} childreb did NOT get their presents";
            }
            ShowInfo(usefullInformation);
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }*/
        }

        private void ShowInfo(string userfullInformation)
        {
            DialogResult dialogResult = MessageBox.Show($"{userfullInformation}\nSend result?", "Send confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var rawResult = apiConnector.SendResult(dataProcesser.Moves, dataProcesser.Bags);
                var result = dataProcesser.ReadResultResponse(rawResult);
            }
        }

        private void PaintCircles()
        {
            foreach (var snowArea in data.snowAreas)
            {
                Point centre = new Point(snowArea.x / scale, snowArea.y / scale);
                paintCircle(centre, snowArea.r / scale);
            }
        }

        private void PaintDots()
        {
            foreach (var child in data.children)
            {
                Point point = new Point(child.x / scale, child.y / scale);
                PaintDot(point);
            }
        }

        private void PaintRoute(List<Point> points)
        {
            paintLine(Point.Empty, new Point(points[0].X / scale, points[0].Y / scale));
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X < 0 || points[i].X > 10000)
                    throw new Exception("out of range");
                if (points[i].Y < 0 || points[i].Y > 10000)
                    throw new Exception("out of range");
                Point start = new Point(points[i-1].X / scale, points[i-1].Y / scale);
                Point end = new Point(points[i].X / scale, points[i].Y / scale);
                paintLine(start, end);
            }
        }

        private void paintLine(Point start, Point end)
        {
            Pen p = new Pen(Color.Green, 1);
            g.DrawLine(p, start, end);
        }

        private void paintCircle(Point centre, int radius)
        {
            Rectangle rectangle = Rectangle.FromLTRB(
                centre.X - radius,
                centre.Y + radius,
                centre.X + radius,
                centre.Y - radius);

            Brush brush = new SolidBrush(Color.LightBlue);
            g.FillEllipse(brush, rectangle);
        }

        private void PaintDot(Point point)
        {
            int width = 2;
            Rectangle rectangle = Rectangle.FromLTRB(
                point.X - width,
                point.Y + width,
                point.X + width,
                point.Y - width);

            Brush brush = new SolidBrush(Color.Red);
            g.FillEllipse(brush, rectangle);
        }
    }
}