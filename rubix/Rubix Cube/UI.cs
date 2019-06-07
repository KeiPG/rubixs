using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System.Threading;
namespace Rubix
{
    /// <summary>
    /// This class declears all of the camera, mouse and keyboard variables. as well as making the form to be used(the buttons).
    /// also sets up spaceing and components for later use.
    /// </summary>
    public class UI : System.Windows.Forms.Form
    {
        
        public static int scale = 1;//this is the scale of the cube
        public static Microsoft.DirectX.Direct3D.Device d3ddevice;//this is a directX device
        private System.ComponentModel.Container components = null;
        private static Microsoft.DirectX.DirectInput.Device mouse;//this is how i access the mouse
        private static Microsoft.DirectX.DirectInput.Device keyboard;//this is how i access the keyboard
        //private Vector3 angle;
        private Vector3 cameraPos;//this is a vector 3d for camera position
        private Vector3 mousepos;//this is a vector 3d for the mouse position
        private double rho;//this is the variable used in camera
        private float theta;//this is a variable used in camera x/y/z
        private float phi;// this is a variable used in camera x/z
        private Color rotateFace;//this is the variable used in rotation
        private bool arrow;//this is a bool for the arrow
        private float OldZ;//this is a float for z postion for mouse
        private bool face;//this is a bool for face

        private bool middle;//this is a bool for middle

        private static UI singleton;//this is a how i singleton my program
        Solver solver;//this is a solver for instantly solving the cube
        Cube rubixCube;//this is the cube
        private System.Windows.Forms.Button button1;//this is a button
        private System.Windows.Forms.Button button2;//this is a button
        private System.Windows.Forms.Button button3;//this is a button
        private System.Windows.Forms.Button button4;//this is a button
        private System.Windows.Forms.Button button5;//this is a button
        private System.Windows.Forms.Button button6;//this is a button
        private System.Windows.Forms.Button button7;//this is a button
        private System.Windows.Forms.Button button8;//this is a button
        private System.Windows.Forms.Button button9;//this is a button

        float spacing = 1f;

        /// <summary>
        /// this function is a singleton which means i will only ever have a single form. 
        /// it also turns off the minimize and maximize buttons so the user cant break my program and so it cant be
        /// expanded or constranted.
        /// </summary>
        /// <returns>the form</returns>
        public static UI getInstance()
        {
            if (singleton == null)
            {
                singleton = new UI();
                singleton.FormBorderStyle = FormBorderStyle.FixedSingle;
                singleton.MinimizeBox = false;
                singleton.MaximizeBox = false;
            }
            return singleton;
        }
        /// <summary>
        /// this functions calls of the initializations as well as puts in some start values for how the cube looks
        /// </summary>
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
            spacing = 1.1f;
            
        }
        /// <summary>
        /// this function gives values to the mouse position for use later.
        /// this also makes sure the user cant break the code by making the mouse 
        /// go places i dont want them to.
        /// this also does the zooming functions on the scroll wheel and when pressed down will scramble.
        /// </summary>
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
                    mousepos.X = mousepos.X%10;
                }
                if (Math.Abs(mousepos.Y) > 10)
                {
                    mousepos.Y = mousepos.Y%10;
                }
                theta += Geometry.DegreeToRadian(mousepos.X);
                phi += Geometry.DegreeToRadian(mousepos.Y);
            }
            float Dif = mousepos.Z - OldZ;
            OldZ = mousepos.Z;
            if (Dif != 0) //scroll wheel
            {
                if (keyboard.GetCurrentKeyboardState()[Key.LeftControl])
                {
                    spacing -= 0.0005f * Dif;
                    Console.WriteLine("rho = " + rho + " phi = " + phi + " theta = " + theta + " spacing = " + spacing);
                }
                else
                {
                    rho += 0.005 * Dif;
                    
                    
                    
                   
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
        /// <summary>
        /// this function does the rotation of the cube on click(fully commented)
        /// </summary>
        /// <param name="e">this parameter is a object that does nothing</param>
        /// <param name="m">this parameter is an event</param>
        void MouseMoveCamera(object e, MouseEventArgs m)
        {
            
            if (m.Button == MouseButtons.Right)//checks if right click is down
            {
                if (mousepos.X > m.X)//if it is clicked and moved left
                {
                    theta -= Geometry.DegreeToRadian(5);//change the cube depending on click distance moved
                }
                else if (mousepos.X < m.X)//if it is clicked and moved right
                {
                    theta += Geometry.DegreeToRadian(5);//change the cube depending on click distance moved
                }

                if (mousepos.Y > m.Y)//if it is clicked and moved up
                {
                    phi -= Geometry.DegreeToRadian(5);//change the cube depending on click distance moved
                }
                else if (mousepos.Y < m.Y)//if it is clicked and moved down
                {
                    phi += Geometry.DegreeToRadian(5);//change the cube depending on click distance moved
                }
                
                mousepos.X = m.X;//makes the new mouse position the saved one
                mousepos.Y = m.Y;//makes the new mouse position the saved one
            }

            if (m.Button == MouseButtons.Middle)
            {
                rubixCube.Scramble();
            }
            
        }
        /// <summary>
        /// this function initialize the mouse using directX functions
        /// </summary>
        private void InitializeMouse()
        {
            mouse = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Mouse);
            mouse.SetCooperativeLevel(this, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            mouse.Acquire();
            
        }
        /// <summary>
        /// this function initializes the keyboard using directX functions
        /// </summary>
        private void InitializeKeyboard()
        {
            keyboard = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Keyboard);
            keyboard.SetCooperativeLevel(this, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            keyboard.Acquire();
        }
        /// <summary>
        /// this function waits for the user to press a button then checks
        /// if there is an event to that key.
        /// if one of the event keys are pressed then does that function.
        /// there is face rotation, cube rotation and zooming function
        /// </summary>
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
                        if (keys[Key.Q])
                        {
                            solver.SolveWhiteCross();
                            solver.SolveWhiteCorners();
                            solver.SolveMiddleLayer(Color.Blue);
                            solver.SolveMiddleLayer(Color.Green);
                            solver.SolveMiddleLayer(Color.Red);
                            solver.SolveMiddleLayer(Color.Purple);
                            solver.SolveBottomCross(singleton);
                            solver.SolveBottomEdges(singleton);
                            solver.SolveBottomCorners(singleton);
                            
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
                            rotateFace = Color.Hidden;
                            
                        }
                        else if (keys[Key.W])
                        {
                            rubixCube.Rotate(Color.White, Direction.CounterClockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.B])
                        {
                            rubixCube.Rotate(Color.Blue, Direction.CounterClockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.G])
                        {
                            rubixCube.Rotate(Color.Green, Direction.CounterClockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.Y])
                        {
                            rubixCube.Rotate(Color.Yellow, Direction.CounterClockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.O])
                        {
                            rubixCube.Rotate(Color.Purple, Direction.CounterClockwise);
                            rotateFace = Color.Hidden;
                        }
                    }
                    else if (keys[Key.RightArrow])
                    {
                        if (keys[Key.R])
                        {
                            rubixCube.Rotate(Color.Red, Direction.Clockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.W])
                        {
                            rubixCube.Rotate(Color.White, Direction.Clockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.B])
                        {
                            rubixCube.Rotate(Color.Blue, Direction.Clockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.G])
                        {
                            rubixCube.Rotate(Color.Green, Direction.Clockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.Y])
                        {
                            rubixCube.Rotate(Color.Yellow, Direction.Clockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.O])
                        {
                            rubixCube.Rotate(Color.Purple, Direction.Clockwise);
                            rotateFace = Color.Hidden;
                        }
                        else if (keys[Key.M])
                        {
                            Random r = new Random();
                            for (int i = 0; i < 50; i++)
                            {


                                Int32 color = r.Next(1, 7);
                                if (color == 1)
                                {
                                    rubixCube.Rotate(Color.Red, Direction.Clockwise);
                                }
                                else if (color == 2)
                                {
                                    rubixCube.Rotate(Color.Purple, Direction.Clockwise);
                                }
                                else if (color == 3)
                                {
                                    rubixCube.Rotate(Color.Blue, Direction.Clockwise);
                                }
                                else if (color == 4)
                                {
                                    rubixCube.Rotate(Color.Green, Direction.Clockwise);
                                }
                                else if (color == 5)
                                {
                                    rubixCube.Rotate(Color.Yellow, Direction.Clockwise);
                                }
                                else if (color == 6)
                                {
                                    rubixCube.Rotate(Color.White, Direction.Clockwise);
                                }
                                else
                                {
                                    rubixCube.Rotate(Color.Purple, Direction.Clockwise);
                                }
                            }
                        }
                    }
                }
            }

            arrow = (keys[Key.LeftArrow] || keys[Key.RightArrow]);
        }
        /// <summary>
        /// this function is the part of the program that gives
        /// variables to the position of the camera when
        /// it is told to move.
        /// </summary>
        private void PositionCamera()
        {
            cameraPos.Y = -(float)(rho * Math.Cos(phi));//makes the camera postion variable to a Y position depending on Phi

            cameraPos.X = (float)(rho * Math.Sin(phi) * Math.Sin(theta));//makes the camera postion variable to a X position depending on Phi and theta
            cameraPos.Z = (float)(rho * Math.Sin(phi) * Math.Cos(theta));//makes the camera postion variable to a Z position depending on Phi and theta
            
            d3ddevice.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1, 100);//puts in the camera projection
            d3ddevice.Transform.View = Matrix.LookAtLH(cameraPos, new Vector3(0, 0, 0), new Vector3(0, 1, 0));//place camera postion depending on Camera posion variable
        }
        /// <summary>
        /// this function disposes of objects(used when i was testing)
        /// it will get ride of components
        /// </summary>
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
        //Initializes all the parts of each button
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
            this.button9 = new System.Windows.Forms.Button();
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
            this.button1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.r);
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
            this.button5.Text = "Purple";
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
            this.button7.KeyUp += new System.Windows.Forms.KeyEventHandler(this.c);
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
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(405, 301);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 8;
            this.button9.Text = "Shuffle";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // UI
            // 
            this.ClientSize = new System.Drawing.Size(492, 466);
            this.Controls.Add(this.button9);
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
            this.Load += new System.EventHandler(this.UI_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.c);
            this.ResumeLayout(false);

        }
        /// <summary>
        /// this function initializes the ability to aceess the windows form and also access the hardware
        /// </summary>
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
        /// <summary>
        /// this function makes the outlines of the cube
        /// </summary>
        /// <param name="e">this parameter is a paint function from visual studios</param>
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

        /// <summary>
        /// this is main function that initializes the devices and the camera then runs the program
        /// </summary>
        static void Main()
        {
            UI ui = UI.getInstance();
            ui.InitializeDevice();
            ui.PositionCamera();
            Application.Run(ui);
        }
        /// <summary>
        /// this function is the button for the red color 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Red;
        }
        /// <summary>
        /// this function is the button for the white color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            rotateFace = Color.White;
        }
        /// <summary>
        /// this function is the button for the green color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Green;
        }
        /// <summary>
        /// this function is the button for the yellow color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Yellow;
        }
        /// <summary>
        /// this function is the button for the purple color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Purple;
        }
        /// <summary>
        /// this function is the button for the blue color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            rotateFace = Color.Blue;
        }
        /// <summary>
        /// this button checks if color is press already then if it is moves that face clockwise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            if (rotateFace != Color.Hidden)
            {
                rubixCube.Rotate(rotateFace, Direction.Clockwise);
            }
            rotateFace = Color.Hidden;
        }
        /// <summary>
        /// this button checks if color is press already then if it is moves that face Counter clockwise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            if (rotateFace != Color.Hidden)
            {
                rubixCube.Rotate(rotateFace, Direction.CounterClockwise);
            }
            rotateFace = Color.Hidden;
        }

        private void UI_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// this function was for testing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void r(object sender, KeyPressEventArgs e)
        {
            rotateFace = Color.Red;
        }
        /// <summary>
        /// this function is for testing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void c(object sender, KeyEventArgs e)
        {
            if (rotateFace != Color.Hidden)
            {
                rubixCube.Rotate(rotateFace, Direction.Clockwise);
            }
            rotateFace = Color.Hidden;
        }
        /// <summary>
        /// this button is for scrambling the cube
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            rubixCube.Scramble();
            

        }
        

    }
}
