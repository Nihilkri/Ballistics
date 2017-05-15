using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;

namespace Ballistics {
	public partial class Form1 : Form {
		#region Variables
		#region Graphics
		// Boilerplate Graphics
		Graphics gb, gf; Bitmap gi;
		int fx, fy, fx2, fy2;
		#endregion Graphics
		#region Physics
		// Global Position, Global Gravity
		public Vector2 gP, gG;
		// Storage structure of each arc line
		public struct Arc {
			public int n, tfr;
			public Vector2 G0, P0, V0, A0, P, V, A;
			public float v, a, th, ph, X, Y, Z, T;
			public float x, y, z, g;
			public Pen p;
			public PointF[] c; public PointF cp;
			public float tanth, devi, x1, x2, pow;
			public double rad;
		}
		// The data list that holds them all
		public Arc[] arcs;
		// Timing and animation
		public Timer tim; public DateTime st, et;
		public MouseEventArgs m;
		public KeyEventArgs k;
		#endregion Physics
		public string err = "";
		#endregion Variable
		#region Events
		public Form1() {InitializeComponent();}
		private void Form1_Load(object sender, EventArgs e) {
			// Graphics and Timing Boilerplate
			fx = Width = Screen.GetBounds(this).Width; fx2 = fx / 2;
			fy = Height = Screen.GetBounds(this).Height; fy2 = fy / 2;
			gi = new Bitmap(fx, fy); gb = Graphics.FromImage(gi);
			gf = CreateGraphics(); gb.Clear(Color.Black);
			tim = new Timer() { Interval = (int)(1000.0 / 60.0) };
			tim.Tick += Tim_Tick;

			// Initialize World
			gP = new Vector2(0.0f, fy2); gG = new Vector2(0, -9.805f);

			// Initialize Mouse arc and Test arcs
			arcs = new Arc[] {
				new Arc() { n = 0, p = Pens.White }
				, new Arc() { n = 1, p = Pens.Red }
				, new Arc() { n = 2, p = Pens.Green }
				, new Arc() { n = 3, p = Pens.Blue }
				//, new Arc() { n = 4, p = Pens.SkyBlue }
			};
			arcXT(0, fx * 3.0f / 4.0f);
			arcXT(1, fx * 3.0f / 4.0f);
			arcXT(2, fx * 3.0f / 4.0f);
			arcXT(3, fx * 3.0f / 4.0f);
			//arcXT(1, fx, 45.0f, 0, 0);
			//arcXT(2, fx, 60.0f, 0, 0);
			//arcXT(3, fx2, 45.0f, 0, fy2);
			//arcVT(4, arcs[3].v, 45.0f, 0, fy2);
			//arcVT(0, arcs[3].v, 45.0f);

			m = new MouseEventArgs(MouseButtons.None, 0, fx2, fy2, 0);
			k = new KeyEventArgs(Keys.None);
			// Begin.
			tim.Start();
		} // Load

		private void Form1_Paint(object sender, PaintEventArgs e) {
			gf.DrawImage(gi, 0, 0);
		} // Paint

		private void Form1_KeyDown(object sender, KeyEventArgs e) {
			k = e;
			switch(e.KeyCode) {
				case Keys.Escape: Close(); break;
				case Keys.Left: gP.X = 0; break;
			} // Switch
		} // KeyDown

		private void Form1_MouseMove(object sender, MouseEventArgs e) {
			m = e;
			// Todo: Mouse move sets th,V
			// Todo: Ctrl + Mouse sets gP
			// Todo: Shift + LClick calcs X,T
			// Todo: Shift + RClick calcs P,T

		} // MouseMove

		private void Tim_Tick(object sender, EventArgs e) {
			// Stopwatch
			st = DateTime.Now;
			err = "Start\n";
			//arcVT(0, (float)(Math.Atan2(((fy - m.Y) - arcs[0].P0.Y), (m.X - arcs[0].P0.X)) * 180.0 / Math.PI), arcs[0].v);// (float)Math.Sqrt(Math.Pow(m.Y - arcs[0].P0.Y, 2.0) + Math.Pow(m.X - arcs[0].P0.X, 2.0)));
			//arcXT(0, (float)(Math.Atan2(((fy - m.Y) - arcs[0].P0.Y), (m.X - arcs[0].P0.X)) * 180.0 / Math.PI), fx * 3.0f / 4.0f);// (float)Math.Sqrt(Math.Pow(m.Y - arcs[0].P0.Y, 2.0) + Math.Pow(m.X - arcs[0].P0.X, 2.0)));
			arcPV(ref arcs[0], true, m.Location, arcs[0].v);
			arcPV(ref arcs[1], false, m.Location, arcs[1].v);
			//arcXT(1, (float)(Math.Atan2(((fy - m.Y) - arcs[1].P0.Y), (m.X - arcs[1].P0.X)) * 180.0 / Math.PI), fx * 3.0f / 4.0f, 0.0f, fy2);// (float)Math.Sqrt(Math.Pow(m.Y - arcs[0].P0.Y, 2.0) + Math.Pow(m.X - arcs[0].P0.X, 2.0)));
			//arcXT(2, 90.0f-(float)(Math.Atan2(((fy - m.Y) - arcs[2].P0.Y), (m.X - arcs[2].P0.X)) * 180.0 / Math.PI), fx * 3.0f / 4.0f, 0.0f, fy2);// (float)Math.Sqrt(Math.Pow(m.Y - arcs[0].P0.Y, 2.0) + Math.Pow(m.X - arcs[0].P0.X, 2.0)));
			//arcVT(3, (float)(Math.Atan2(((fy - m.Y) - arcs[3].P0.Y), (m.X - arcs[3].P0.X)) * 180.0 / Math.PI), arcs[1].v, 0.0f, 0.0f);// (float)Math.Sqrt(Math.Pow(m.Y - arcs[0].P0.Y, 2.0) + Math.Pow(m.X - arcs[0].P0.X, 2.0)));

			//gP.X += (3000.0f / 60.0f) * fx / 20000.0f;

			// CLS
			gb.Clear(Color.Black);
			gb.DrawLine(Pens.Violet, 0, fy2, fx, fy2);
			gb.DrawLine(Pens.Violet, 0, fy2/2, fx, fy2/2);
			gb.DrawLine(Pens.Violet, fx2, 0, fx2, fy);
			gb.DrawLine(Pens.Cyan, 0, fy-gP.Y, fx, fy-gP.Y);
			gb.DrawLine(Pens.Cyan, gP.X, 0, gP.X, fy);

			// Draw all arcs
			//Parallel.ForEach(arcs, a => drawArc(a));
			foreach (Arc a in arcs) { drawArc(a); }

			// Lap, Debug, Print
			err += "Finish: " + (DateTime.Now - st).TotalMilliseconds.ToString() + " / " + tim.Interval;
			gb.DrawString(err, Font, Brushes.White, 0, 0);
			gf.DrawImage(gi, 0, 0);
		} // Tim_Tick


		#endregion Events
		#region Arcs
		// Todo: For a given th, V, and g, calculate X, Y, and T
		// Todo: For a given th, P0, V, and g, calculate X, Y, and T
		public void arcVT(int n, float nv, float nth = -1.0f, float nx0 = -1.0f, float ny0 = -1.0f, float ngx = -1.0f, float ngy = -1.0f) {
			arcVT(ref arcs[n], nth, nv, nx0, ny0, ngx, ngy);
		}
		public void arcVT(ref Arc a, float nv, float nth = -1.0f, float nx0 = -1.0f, float ny0 = -1.0f, float ngx = -1.0f, float ngy = -1.0f) {
			// Optional custon cannon and gravity
			a.P0.X = (nx0 == -1.0f) ? gP.X : nx0;
			a.P0.Y = (ny0 == -1.0f) ? gP.Y : ny0;
			a.G0.X = (ngx == -1.0f) ? gG.X : ngx;
			a.G0.Y = (ngy == -1.0f) ? gG.Y : ngy;
			a.g = -a.G0.Y;

			// Max range
			if (nth == -1.0f) {
				a.rad = (2.0f * a.g * a.P0.Y + a.v * a.v) / (2.0f * a.g * a.P0.Y + 2.0f * a.v * a.v);
				a.th = nth = (float)Math.Acos(Math.Sqrt(Math.Abs(a.rad)));
			} else
			// Degrees to Radians
			a.th = nth = ((nth + 360.0f) % 360.0f) * (float)Math.PI / 180.0f; a.v = nv;

			// Break the vector
			a.V0.X = nv * (float)Math.Cos(nth);
			a.V0.Y = nv * (float)Math.Sin(nth);

			// Time To Land
			//a.T = -2.0f / a.G0.Y * a.V0.Y;
			a.rad = (a.V0.Y * a.V0.Y) + 2.0f * -a.G0.Y * a.P0.Y; a.rad = a.rad >= 0 ? a.rad : -a.rad;
			a.T = (a.V0.Y + (float)Math.Sqrt(a.rad)) / -a.G0.Y;
			// Range
			a.X = a.V0.X * a.T;
			// Height
			a.Y = a.V0.Y * a.T / 4.0f;
			// Relative range and height
			a.x = a.X - a.P0.X; a.y = a.Y - a.P0.Y;
			//a.c = Color.White;
			arc(ref a);
		}
		// Todo: For a given th, X, and g, calculate V, Y, and T
		// Todo: For a given th, P0, X, and g, calculate V, Y, and T
		public void arcXT(int n, float nX, float nth = -1.0f, float nx0 = -1.0f, float ny0 = -1.0f, float ngx = -1.0f, float ngy = -1.0f) {
			arcXT(ref arcs[n], nth, nX, nx0, ny0, ngx, ngy);
		}
		public void arcXT(ref Arc a, float nX, float nth = -1.0f, float nx0 = -1.0f, float ny0 = -1.0f, float ngx = -1.0f, float ngy = -1.0f) {
			// Optional custon cannon and gravity
			a.P0.X = (nx0 == -1.0f) ? gP.X : nx0;
			a.P0.Y = (ny0 == -1.0f) ? gP.Y : ny0;
			a.G0.X = (ngx == -1.0f) ? gG.X : ngx;
			a.G0.Y = (ngy == -1.0f) ? gG.Y : ngy;

			// Max range
			if(nth == -1.0f) {
				a.rad = (2.0f * a.g * a.P0.Y + a.v * a.v) / (2.0f * a.g * a.P0.Y + 2.0f * a.v * a.v);
				a.th = nth = (float)Math.Acos(Math.Sqrt(Math.Abs(a.rad)));
			} else
			// Degrees to Radians
			a.th = nth = ((nth + 360.0f) % 360.0f) * (float)Math.PI / 180.0f;

			// Base velocity on range
			a.X = nX -= a.P0.X;
			// Relative range
			a.x = a.X - a.P0.X;
			//a.v = (float)Math.Sqrt(Math.Abs(a.G0.Y) * (nX / (float)Math.Sin(2.0 * nth)));
			a.rad = -a.G0.Y / Math.Cos(nth) / (2.0 * nX * Math.Sin(nth) + 2.0 * a.P0.Y * Math.Cos(nth)); a.rad = a.rad >= 0 ? a.rad : -a.rad;
			a.v = nX * (float)Math.Sqrt(a.rad);

			// Break the vector
			a.V0.X = a.v * (float)Math.Cos(nth);
			a.V0.Y = a.v * (float)Math.Sin(nth);

			// Time To Land
			//a.T = -2.0f * a.v / a.G0.Y * (float)Math.Sin(nth);
			a.rad = (a.V0.Y * a.V0.Y) + 2.0f * -a.G0.Y * a.P0.Y; a.rad = a.rad >= 0 ? a.rad : -a.rad;
			a.T = (a.V0.Y + (float)Math.Sqrt(a.rad)) / -a.G0.Y;

			//if(Math.Abs(a.X - a.V0.X * a.T) < 0.01f) a.c = Color.Green; else a.c = Color.Red;
			// Height
			a.Y = a.V0.Y * a.T / 4.0f;
			// Relative height
			a.y = a.Y - a.P0.Y;
			arc(ref a);
		}

		// Todo: For a given P0, P, and v, calculate th, X, Y, and T
		public void arcPV(ref Arc a, bool ang, PointF nP, float nv, float nx0 = -1.0f, float ny0 = -1.0f, float ngx = -1.0f, float ngy = -1.0f) {
			// Optional custon cannon and gravity
			a.P0.X = (nx0 == -1.0f) ? gP.X : nx0;
			a.P0.Y = (ny0 == -1.0f) ? gP.Y : ny0;
			a.G0.X = (ngx == -1.0f) ? gG.X : ngx;
			a.G0.Y = (ngy == -1.0f) ? gG.Y : ngy;
			a.x = nP.X - a.P0.X; a.y = nP.Y - a.P0.Y;

			// Determine angle
			a.rad = -1.0f; while (a.rad < 0.0f) {
				a.rad = Math.Pow(a.v, 4) + a.G0.Y * (2.0f * a.y * a.v * a.v - a.G0.Y * Math.Pow(a.x, 2.0f));
				if (a.rad < 0) {

					a.rad = -a.rad;
				}
			}
			a.th = (float)(Math.Atan((a.v * a.v + (ang ? 1.0f : -1.0f) * Math.Sqrt(a.rad)) / (-a.G0.Y * a.x)) * 180.0f / Math.PI);

			arcVT(ref a, a.th, nv, nx0, ny0, ngx, ngy);
		}
		// Todo: For a given P, calculate all arcs that pass through
		// Todo: For a given X and T, calculate all arcs that land simultaneously
		// Todo: For a given P and T, calculate all arcs that coincide simultaneously

		public void arc(int n) { arc(ref arcs[n]); }
		public void arc(ref Arc a) {
			a.x1 = Math.Min(a.P0.X, a.P0.X);
			a.x2 = Math.Max(a.P0.X + a.X, a.P0.X + a.X);
			a.c = new PointF[4];
			a.tanth = (float)Math.Tan(a.th); a.devi = a.G0.Y / (2.0f * (float)Math.Pow(a.V0.X, 2.0));
			a.c = new PointF[] {
				new PointF(a.x1, arcY(ref a, a.x1)),
				new PointF(0,0),
				new PointF(0,0),
				new PointF(a.x2, arcY(ref a, a.x2))
			}; //(a.x1 - a.P0.X)
			a.cp = new PointF((a.x1 + a.x2) / 2.0f,
				a.c[0].Y + (a.x2 - a.x1) / 2.0f * (2.0f * a.devi * (a.x1 - a.P0.X) + a.tanth));
			a.c[1].X = a.x1 / 3.0f + a.cp.X * 2.0f / 3.0f;
			a.c[1].Y = fy - (a.c[0].Y / 3.0f + a.cp.Y * 2.0f / 3.0f);
			a.c[2].X = a.x2 / 3.0f + a.cp.X * 2.0f / 3.0f;
			a.c[2].Y = fy - (a.c[3].Y / 3.0f + a.cp.Y * 2.0f / 3.0f);
			a.c[0].Y = fy - a.c[0].Y; a.c[3].Y = fy - a.c[3].Y; a.cp.Y = fy - a.cp.Y;
			//a.rad = Math.Abs(a.G0.Y) * (fx / (float)Math.Sin(Math.PI / 2.0)); a.rad = a.rad >= 0 ? a.rad : -a.rad;
			//a.pow = (float)Math.Sqrt(a.rad);
			a.rad = (2.0f * a.g * a.P0.Y + a.v * a.v) / (2.0f * a.g * a.P0.Y + 2.0f * a.v * a.v);
			a.pow = (float)Math.Acos(Math.Sqrt(Math.Abs(a.rad)));

		} // arc(int n)

		public float arcY(int n, float x) {
			return (arcs[n].P0.Y + (x - arcs[n].P0.X) * (arcs[n].tanth + (x - arcs[n].P0.X) * arcs[n].devi));
		}
		public float arcY(ref Arc a, float x) {
			return (a.P0.Y + (x - a.P0.X) * (a.tanth + (x - a.P0.X) * a.devi));
		}
		// Todo: Draw the arc
		// Todo: Overhaul the drawing to be a bezier curve instead
		public void drawArc(int n) { drawArc(arcs[n]); }
		public void drawArc(Arc a) {
			gb.DrawLines(a.p, a.c);
			gb.DrawBezier(a.p, a.c[0], a.c[1], a.c[2], a.c[3]);
			gb.DrawEllipse(a.p, a.cp.X - 5, a.cp.Y - 5, 10, 10);
			gb.DrawLines(a.p, new PointF[] { a.c[0], a.cp, a.c[3] });
			//gb.DrawBezier(Pens.Green, a.c[0], a.c[1], a.c[1], a.c[2]);
			//gb.DrawBezier(Pens.Blue, a.c[1], a.c[2], a.c[2], a.c[3]);


			//float y, oy = a.P0.Y;
			//if (a.X >= 0.0f) {
			//	for (int x = (int)a.P0.X; x < a.X; x++) {
			//		y = fy - (a.P0.Y + x * (a.tanth + x * a.devi));
			//		gb.DrawLine(Pens.Green, x - 1, oy, x, y);
			//		oy = y;
			//	} // For
			//} // If
			//else {
			//	for (int x = (int)(a.P0.X + a.X); x < a.P0.X; x++) {
			//		y = fy - (a.P0.Y + x * (a.tanth + x * a.devi));
			//		gb.DrawLine(Pens.Red, x - 1, oy, x, y);
			//		oy = y;
			//	} // For
			//} // Else


			err += a.n + ":" +
					"\n\tG0 = " + a.G0 +
					"\n\tP0 = " + a.P0 +
					"\n\tV0 = " + a.V0 +
					"\n\tA0 = " + a.A0 +
					"\n\tP  = " + a.P +
					"\n\tV  = " + a.V +
					"\n\tA  = " + a.A +
					"\n\tv  = " + a.v +
					"\n\tpow= " + a.pow +
					"\n\ta  = " + a.a +
					"\n\tth = " + a.th * 180.0f / Math.PI +
					"\n\tph = " + a.ph * 180.0f / Math.PI +
					"\n\tX  = " + a.X +
					"\n\tvxt= " + a.V0.X * a.T +
					"\n\tY  = " + a.Y +
					"\n\tZ  = " + a.Z +
					"\n\tT  = " + a.T +
					"\n\tC0 = " + a.c[0] +
					"\n\tC1 = " + a.c[1] +
					"\n\tC2 = " + a.c[2] +
					"\n\tC3 = " + a.c[3] +



					"\n";
		} // DrawArc

		#endregion Arcs
	} // Form1
} // Ballistics