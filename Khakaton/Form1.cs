using Khakaton.DataHandler;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace Khakaton
{
    public partial class Form1 : Form
    {
        Graphics g;
        DataDTO data;
        int scale = 11;
        ApiConnector apiConnector;
        DataProcesser dataProcesser;
        public Form1()
        {
            InitializeComponent();
            apiConnector = new ApiConnector();
            dataProcesser = new DataProcesser();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                        
        }        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            string mapData = apiConnector.GetMapData();
            data = dataProcesser.GetData(mapData);
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
            }
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