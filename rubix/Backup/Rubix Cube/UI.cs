using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
namespace Rubix
{
    public class UI : System.Windows.Forms.Form
    {
        public static int scale = 1;
        public static Microsoft.DirectX.Direct3D.Device d3ddevice;
        private System.ComponentModel.Container components = null;
        private static Microsoft.DirectX.DirectInput.Device mouse;
        private static Microsoft.DirectX.DirectInput.Device keyboard;
        private Vector3 angle;
        private Vector3 cameraPos;
        private Vector3 mousepos;
        private float rho;
        private float theta;
        private float phi;
        private Color rotateFace;
        private bool arrow;

        private bool face;

        private bool middle;

        private static UI singleton;
        Solver solver;
        Cube rubixCube;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;

        float spacing = 1f;

        public static UI getInstance()
        {
            if (singleton == null)
            {
                singleton = new UI();
            }
            return singleton;
        }

        private UI()
        {
            InitializeComponent();
            InitializeKeyboard();
            InitializeMouse();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            rubixCube = new Cube();
            solver = new Solver(rubixCube);
            rho = 10;
            theta = Geometry.DegreeToRadian(0);
            phi = Geometry.DegreeToRadian(45);
            middle = false;
            rotateFace = Color.Hidden;
        }

        private void ReadMouse()
        {
            MouseState state = mouse.CurrentMouseState;
            mousepos.X = state.X;
            mousepos.Y = state.Y;
            mousepos.Z = state.Z;

            byte[] buttons = state.GetMouseButtons();
            if (buttons[1] != 0) //right button
            {

                float mx = mouse.CurrentMouseState.X;
                float my = mouse.CurrentMouseState.Y;
                //for the pen input, it goes insainly fast
                if (Math.Abs(mousepos.X) > 10)
                {
                    //mousepos.X = mousepos.X%10;
                }
                if (Math.Abs(mousepos.Y) > 10)
                {
                    //mousepos.Y = mousepos.Y%10;
                }
                theta += Geometry.DegreeToRadian(mousepos.X);
                phi += Geometry.DegreeToRadian(mousepos.Y);
            }
            if (mousepos.Z > 0) //scroll wheel
            {
                if (keyboard.GetCurrentKeyboardState()[Key.LeftControl])
                {
                    spacing -= .1f;
                }
                else
                {
                    rho -= 1;
                }
            }
            else if (mousepos.Z < 0)
            {
                if (keyboard.GetCurrentKeyboardState()[Key.LeftControl])
                {
                    spacing += .1f;
                }
                else
                {
                    rho += 1;
                }
            }
            if (buttons[2] != 0) //middle button
            {
                if (!middle)
                {

                    rubixCube.Scramble();
                }
            }
            middle = buttons[2] == 0;
            //mouse.
        }

        void MouseMoveCamera(object e, MouseEventArgs m)
        {
            /*
            if (m.Button == MouseButtons.Right)
            {
                if (mousepos.X > m.X)
                {
                    theta -= Geometry.DegreeToRadian(5);
                }
                else if (mousepos.X < m.X)
                {
                    theta += Geometry.DegreeToRadian(5);
                }
                
                if (mousepos.Y > m.Y)
                {
                    phi -= Geometry.DegreeToRadian(5);
                }
                else if (mousepos.Y < m.Y)
                {
                    phi += Geometry.DegreeToRadian(5);
                }
                
                mousepos.X = m.X;
                mousepos.Y = m.Y;
            }

            if (m.Button == MouseButtons.Middle)
            {
                rubixCube.Scramble();
            }
            */
        }

        private void InitializeMouse()
        {
            mouse = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Mouse);
            mouse.SetCooperativeLevel(this, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            mouse.Acquire();
        }

        private void InitializeKeyboard()
        {
            keyboard = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Keyboard);
            keyboard.SetCooperativeLevel(this, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            keyboard.Acquire();
        }

        private void ReadKeyboard()
        {
            KeyboardState keys = keyboard.GetCurrentKeyboardState();

            face = (keys[Key.R] || keys[Key.Y] || keys[Key.W] || keys[Key.B] || keys[Key.G] || keys[Key.O] || keys[Key.S]);

            if (face)
            {
                if (keys[Key.S] & keys[Key.LeftControl])
                {
                    if (keys[Key.RightArrow])
                    {
                        theta += .1f;
                    }
                    if (keys[Key.LeftArrow])
                    {
                        theta -= .1f;
                    }
                }
                if (!arrow)
                {
                    if (keys[Key.S])
                    {
                        if (keys[Key.D1])
                        {
                            solver.SolveWhiteCross();
                            //solver.SolveWhiteCorners();
                            //solver.SolveMiddleLayer();
                        }
                        if (keys[Key.UpArrow])
                        {
                            if (keys[Key.LeftControl])
                            {
                                spacing += .1f;
                            }
                            else
                            {
                                rho += 1;
                            }
                        }
                        if (keys[Key.DownArrow])
                        {
                            if (keys[Key.LeftControl])
                            {
                                spacing -= .1f;
                            }
                            else
                            {
                                rho -= 1;
                            }
                        }

                    }

                    if (keys[Key.LeftArrow])
                    {

                        if (keys[Key.R])
                        {
                            rubixCube.Rotate(Color.Red, Direction.CounterClockwise);
                        }
                        if (keys[Key.W])
                        {
                            rubixCube.Rotate(Color.White, Direction.CounterClockwise);
                        }
                        if (keys[Key.B])
                        {
                            rubixCube.Rotate(Color.Blue, Direction.CounterClockwise);
                        }
                        if (keys[Key.G])
                        {
                            rubixCube.Rotate(Color.Green, Direction.CounterClockwise);
                        }
                        if (keys[Key.Y])
                        {
                            rubixCube.Rotate(Color.Yellow, Direction.CounterClockwise);
                        }
                        if (keys[Key.O])
                        {
                            rubixCube.Rotate(Color.Orange, Direction.CounterClockwise);
                        }
                    }
                    else if (keys[Key.RightArrow])
                    {
                        if (keys[Key.R])
                        {
                            rubixCube.Rotate(Color.Red, Direction.Clockwise);
                        }
                        if (keys[Key.W])
                        {
                            rubixCube.Rotate(Color.White, Direction.Clockwise);
                        }
                        if (keys[Key.B])
                        {
                            rubixCube.Rotate(Color.Blue, Direction.Clockwise);
                        }
                        if (keys[Key.G])
                        {
                            rubixCube.Rotate(Color.Green, Direction.Clockwise);
                        }
                        if (keys[Key.Y])
                        {
                            rubixCube.Rotate(Color.Yellow, Direction.Clockwise);
                        }
                        if (keys[Key.O])
                        {
                            rubixCube.Rotate(Color.Orange, Direction.Clockwise);
                        }
                    }
                }
            }

            arrow = (keys[Key.LeftArrow] || keys[Key.RightArrow]);
        }

        private void PositionCamera()
        {
            cameraPos.Y = -(float)(rho * Math.Cos(phi));

            cameraPos.X = (float)(rho * Math.Sin(phi) * Math.Sin(theta));
            cameraPos.Z = (float)(rho * Math.Sin(phi) * Math.Cos(theta));

            d3ddevice.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1, 100);
            d3ddevice.Transform.View = Matrix.LookAtLH(cameraPos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(33, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Red";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(33, 62);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "White";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(33, 92);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(63, 24);
            this.button3.TabIndex = 2;
            this.button3.Text = "Green";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(33, 122);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(63, 24);
            this.button4.TabIndex = 3;
            this.button4.Text = "Yellow";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(34, 152);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(63, 24);
            this.button5.TabIndex = 4;
            this.button5.Text = "Orange";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(33, 182);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(63, 24);
            this.button6.TabIndex = 5;
            this.button6.Text = "Blue";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(417, 32);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(63, 24);
            this.button7.TabIndex = 6;
            this.button7.Text = "Clockwise";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(362, 62);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(118, 24);
            this.button8.TabIndex = 7;
            this.button8.Text = "Counter Clockwise";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // UI
            // 
            this.ClientSize = new System.Drawing.Size(492, 466);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "UI";
            this.Text = "Rubix Cube";
            this.ResumeLayout(false);

        }

        public void InitializeDevice()
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.EnableAutoDepthStencil = true;
            presentParams.AutoDepthStencilFormat = DepthFormat.D16;
            d3ddevice = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);
            d3ddevice.RenderState.Lighting = false;
            d3ddevice.RenderState.CullMode = Cull.None;
            d3ddevice.RenderState.ZBufferEnable = true;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {

            ReadMouse();
            ReadKeyboard();
            d3ddevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Cyan, 1.0f, 0);
            //theta += Geometry.DegreeToRadian(1);

            d3ddevice.BeginScene();
            d3ddevice.Transform.World = Matrix.Translation(-spacing - .5f, -spacing - .5f, -spacing - .5f);
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        rubixCube.GetBlock(x, y, z).draw(x * spacing, y * spacing, z * spacing);
                    }
                }
            }

            d3ddevice.EndScene();

            PositionCamera();
            d3ddevice.Present();
            this.Invalidate();
        }


        static void Main()
        {
            UI ui = UI.getInstance();
            ui.InitializeDevice();
            ui.PositionCamera();
            Application.Run(ui);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Red;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rotateFace = Color.White;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Green;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Yellow;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Orange;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Blue;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (rotateFace != Color.Hidden)
            {
                rubixCube.Rotate(rotateFace, Direction.Clockwise);
            }
            rotateFace = Color.Hidden;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (rotateFace != Color.Hidden)
            {
                rubixCube.Rotate(rotateFace, Direction.CounterClockwise);
            }
            rotateFace = Color.Hidden;
        }


    }
}
