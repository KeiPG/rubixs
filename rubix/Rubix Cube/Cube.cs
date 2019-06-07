using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Rubix
{
    /// <summary>
    /// these enum are for color and direction that is used in rotation of a face
    /// </summary>
    public enum Color { White, Yellow, Red, Green, Purple, Blue, Hidden };
    public enum Direction { Clockwise, CounterClockwise };
    /// <summary>
    /// this class makes a cube of that is a list of blocks
    /// </summary>
    class Cube
    {
        public List<Block> blocks;
        /// <summary>
        /// this function is main and does checks for tests as well as makes a test cube and test solver
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Cube testCube = new Cube();
            Solver testSolver = new Solver(testCube);

            //testprint();
            Console.WriteLine("Top Cross solved before scramble: {0}", testSolver.WhiteCrossSolved());
            Console.WriteLine("Middle Layer solved before scramble: {0}", testSolver.MiddleLayerSolved());
            testCube.Scramble();

            /****  failing test case *********/
            /***********************************/

            Console.WriteLine("Top cross solved after scramble:  {0}", testSolver.WhiteCrossSolved());
            Console.WriteLine("Top layer solved after scramble:  {0}", testSolver.WhiteCornersSolved());
            Console.WriteLine("Middle Level solved after scramble:  {0}", testSolver.MiddleLayerSolved());
            testSolver.SolveWhiteCross();
            Console.WriteLine("Top cross solved after solve (cross):  {0}", testSolver.WhiteCrossSolved());
            testSolver.SolveWhiteCorners();
            Console.WriteLine("Top layer solved after solve (corners):  {0}", testSolver.WhiteCornersSolved());
            //testSolver.SolveMiddleLayer(Color.Red);
            Console.WriteLine("Middle layer solved after solve:  {0}", testSolver.MiddleLayerSolved());
            Console.ReadLine();
        }
        
        /// <summary>
        /// this function makes 27 different blocks for the cube then they are placed in to the list of blocks 
        /// that makes up the cube. each block is put into the list in a certain order for later uses
        /// as well as fills in from the top layer then moves backward then down.
        /// </summary>
        public Cube()
        {
            blocks = new List<Block>();
            //top
            //front
            blocks.Add(new Corner(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Purple, Color.Hidden));
            blocks.Add(new Edge(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Corner(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Red));
            //mid
            blocks.Add(new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Hidden, Color.Purple, Color.Hidden));
            blocks.Add(new Middle(Color.White, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Red));
            //back
            blocks.Add(new Corner(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Purple, Color.Hidden));
            blocks.Add(new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Hidden));
            blocks.Add(new Corner(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Red));

            //middle
            //front
            blocks.Add(new Edge(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Purple, Color.Hidden));
            blocks.Add(new Middle(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Red));
            //mid
            blocks.Add(new Middle(Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Purple, Color.Hidden));
            blocks.Add(new Hidden(Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Middle(Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Red));
            //back
            blocks.Add(new Edge(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Purple, Color.Hidden));
            blocks.Add(new Middle(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Red));

            //bottom
            //front
            blocks.Add(new Corner(Color.Hidden, Color.Yellow, Color.Green, Color.Hidden, Color.Purple, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Yellow, Color.Green, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Corner(Color.Hidden, Color.Yellow, Color.Green, Color.Hidden, Color.Hidden, Color.Red));
            //mid
            blocks.Add(new Edge(Color.Hidden, Color.Yellow, Color.Hidden, Color.Hidden, Color.Purple, Color.Hidden));
            blocks.Add(new Middle(Color.Hidden, Color.Yellow, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Yellow, Color.Hidden, Color.Hidden, Color.Hidden, Color.Red));
            //back
            blocks.Add(new Corner(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Purple, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Hidden, Color.Hidden));
            blocks.Add(new Corner(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Hidden, Color.Red));
            
        }
        /// <summary>
        /// this function solves the first face by putting the cube into the solver
        /// </summary>
        public void Solve()
        {
            Solver solver = new Solver(this);
            solver.SolveWhiteCross();
            //solver.SolveMiddleLayer(Color.Red);
        }
        /// <summary>
        /// this function scrambles the cube.(fully commented)
        /// </summary>
        public void Scramble()
        {
            
            Random RandomClass = new Random();//makes a random number generater
            int number_of_total_rotations = RandomClass.Next(75, 150);//creates a random number for amount of rotaions
            Direction direction_to_rotate;//make a direction variable to be used
            Color color_to_rotate;//makes a color variable to be used

            //scramble the cube;
            while (--number_of_total_rotations != 0)//does for a random amount of rotations defined earlier
            {
                int direction_to_rotate_i = RandomClass.Next(100, 200);//finds a random of direction
                //note: generates number greater than or equal to one, and less than 7.
                int color_to_rotate_i = RandomClass.Next(1, 7);//finds a random colour

                //set the direction
                if ((direction_to_rotate_i % 2) == 0)//if the ranbom direction is positive make it clockwise
                    direction_to_rotate = Direction.Clockwise;//makes direction variable clockwise
                else//if it isnt positive make it anti-clockwise
                    direction_to_rotate = Direction.CounterClockwise;//makes direction variable counter clockwise

                //set the face color to rotate
                switch (color_to_rotate_i)//case statemet for random color (each number is a color)
                {
                    case 1:
                        color_to_rotate = Color.Blue;
                        break;
                    case 2:
                        color_to_rotate = Color.Green;
                        break;
                    case 3:
                        color_to_rotate = Color.Purple;
                        break;
                    case 4:
                        color_to_rotate = Color.Red;
                        break;
                    case 5:
                        color_to_rotate = Color.White;
                        break;
                    default:
                        color_to_rotate = Color.Yellow;
                        break;
                }

                //make the rotation
                Rotate(color_to_rotate, direction_to_rotate);

                Console.WriteLine(number_of_total_rotations + ". Rotating " + color_to_rotate + " " + direction_to_rotate);//writes the color and direction to console
            }
        }

        /// <summary>
        /// this function rotates a face of the cube.(fully commented)
        /// </summary>
        /// <param name="color">this parameter is the face that will be rotated</param>
        /// <param name="direction">this parameter is the the direction that is rotated</param>
        public void Rotate(Color color, Direction direction)
        {
            
            List<Block> slice = GetSlice(color);//takes a slice of the cube depending on the color
            Orientor orientation = GetOrientation(color);//takes a orientor of the cube

            Edge leftmid = (Edge)GetBlock(0, 1, color);//makes copys of an edge block
            Edge topmid = (Edge)GetBlock(1, 2, color);//makes copys of an edge block
            Edge rightmid = (Edge)GetBlock(2, 1, color);//makes copys of an edge block
            Edge botmid = (Edge)GetBlock(1, 0, color);//makes copys of an edge block

            Corner topleft = (Corner)GetBlock(0, 2, color);//makes copys of an corner block
            Corner topright = (Corner)GetBlock(2, 2, color);//makes copys of an corner block
            Corner botleft = (Corner)GetBlock(0, 0, color);//makes copys of an corner block
            Corner botright = (Corner)GetBlock(2, 0, color);//makes copys of an corner block

            if (direction == Direction.Clockwise)//checks if direction is clockwise
            {
                Edge edge_bucket = leftmid;//makes a temp variable to be used in rotaing 
                leftmid = (Edge)botmid.Rotate(direction, color);//moves a block to another posision 
                botmid = (Edge)rightmid.Rotate(direction, color);//moves a block to another posision 
                rightmid = (Edge)topmid.Rotate(direction, color);//moves a block to another posision 
                topmid = (Edge)edge_bucket.Rotate(direction, color);//puts temp to a position

                Corner corner_bucket = topleft;//makes a temp variable to be used in rotaing 
                topleft = (Corner)botleft.Rotate(direction, color);//moves a block to another posision 
                botleft = (Corner)botright.Rotate(direction, color);//moves a block to another posision 
                botright = (Corner)topright.Rotate(direction, color);//moves a block to another posision 
                topright = (Corner)corner_bucket.Rotate(direction, color);//puts temp to a position
            }
            else//if it is direction is anti-clockwise
            {
                Edge edge_bucket = leftmid;//makes a temp variable to be used in rotaing 
                leftmid = (Edge)topmid.Rotate(direction, color);//moves a block to another posision 
                topmid = (Edge)rightmid.Rotate(direction, color);//moves a block to another posision 
                rightmid = (Edge)botmid.Rotate(direction, color);//moves a block to another posision 
                botmid = (Edge)edge_bucket.Rotate(direction, color);//puts temp to a position

                Corner corner_bucket = topleft;//makes a temp variable to be used in rotaing 
                topleft = (Corner)topright.Rotate(direction, color);//moves a block to another posision 
                topright = (Corner)botright.Rotate(direction, color);//moves a block to another posision 
                botright = (Corner)botleft.Rotate(direction, color);//moves a block to another posision 
                botleft = (Corner)corner_bucket.Rotate(direction, color);//puts temp to a position
            }
            Console.WriteLine("Rotated " + color + " to " + direction);
        }

        /// <summary>
        /// this function gets the a colour and makes an orientation to help with solving and rotations
        /// </summary>
        /// <param name="face">this parameter is the face orientated around</param>
        /// <returns>the orietation of the cube</returns>
        public static Orientor GetOrientation(Color face)
        {
            Orientor orientor;

            switch(face)
            {
                case Color.White:
                    orientor = new Orientor(Color.Red, Color.Purple, Color.White, Color.Yellow, Color.Blue, Color.Green);
                    break;
                case Color.Yellow:
                    orientor = new Orientor(Color.Red, Color.Purple, Color.Yellow, Color.White, Color.Green, Color.Blue);
                    break;
                case Color.Red:
                    orientor = new Orientor(Color.White, Color.Yellow, Color.Red, Color.Purple, Color.Green, Color.Blue);
                    break;
                case Color.Purple:
                    orientor = new Orientor(Color.White, Color.Yellow,Color.Purple,Color.Red, Color.Blue, Color.Green);
                    break;
                case Color.Green:
                    orientor = new Orientor(Color.White, Color.Yellow, Color.Green, Color.Blue, Color.Purple, Color.Red);
                    break;
                case Color.Blue:
                    orientor = new Orientor(Color.White, Color.Yellow, Color.Blue, Color.Green, Color.Red, Color.Purple);
                    break;
                default:
                    throw new Exception("Invalid face");
            }
            return orientor;
        }

        /// <summary>
        /// this functions fetches a block by making a slice and picking it off of the slice
        /// </summary>
        /// <param name="x">this parameter is the x postion of the block</param>
        /// <param name="y">this parameter is the y position of the block</param>
        /// <param name="face"></param>
        /// <returns>the block</returns>
        public Block GetBlock(int x, int y, Color face)
        {
            Orientor orientation = GetOrientation(face);
            List<Block> slice = GetSlice(face);

            var block = from Block b in slice select b;

            /* These are intended to fall through. */
            switch (x)
            {
                case 0:
                    slice = GetSlice(orientation.left, slice); break;
                case 2:
                    slice = GetSlice(orientation.right, slice); break;
            }

            switch (y)
            {
                case 0:
                    slice = GetSlice(orientation.bottom, slice); break;
                case 2:
                    slice = GetSlice(orientation.top, slice); break;
            }

            if (slice.Count == 1) //only have one, then we good!
                return slice[0]; // TODO: cast this to block type?

            /* if we get here, we had a non-corner piece, so get rid of the corners. */
            var non_corners = from Block wantedBlock in slice
                               where (wantedBlock.top == Color.Hidden && wantedBlock.bottom == Color.Hidden) ||
                                     (wantedBlock.left == Color.Hidden && wantedBlock.right == Color.Hidden) ||
                                     (wantedBlock.back == Color.Hidden && wantedBlock.front == Color.Hidden)
                               select wantedBlock;

            List<Block> toReturn = new List<Block>();

            foreach (Block c in non_corners)
            {
                toReturn.Add(c);
            }

            if (toReturn.Count != 1)
            {
                throw new Exception("Error with 2d Orientation");
            }

            return toReturn[0]; // TODO: cast to block type?
        }
        /// <summary>
        /// this functions fetches a block from a face and picking it off of the face(corner)
        /// </summary>
        /// <param name="x">this parameter is the x postion of the block</param>
        /// <param name="y">this parameter is the y position of the block</param>
        /// <param name="face"></param>
        /// <returns>the block</returns>
        public Block GetBlock(int x, int y, int z)
        {
            List<Block> dataset;
            switch (x) {
                case 0:
                    dataset = GetSlice(Color.Purple);
                    break;
                case 1:
                    dataset = GetSlice(Color.Hidden);
                    break;
                case 2:
                    dataset = GetSlice(Color.Red);
                    break;
                default:
                    throw new Exception("Invalid Coordinate");
            }
            switch (y) {
                case 0:
                    dataset = GetSlice(Color.Yellow, dataset);                    
                    break;
                case 1:
                    dataset = GetSlice(Color.Hidden, dataset,1);
                    break;
                case 2:
                    dataset = GetSlice(Color.White, dataset);
                    break;
                default:
                    throw new Exception("Invalid Coordinate");
            }
            switch (z)
            {
                case 0:
                    dataset = GetSlice(Color.Green, dataset);
                    break;
                case 1:
                    dataset = GetSlice(Color.Hidden, dataset,2);
                    break;
                case 2:
                    dataset = GetSlice(Color.Blue, dataset);
                    break;
                default:
                    throw new Exception("Invalid Coordinate");
            }
            if (dataset.Count != 1)
            {
                throw new Exception("Invalid results, should never come here");
            }
            return (Block)dataset[0];
        }
        /// <summary>
        /// this functions fetches a block by the face then picking it off the face(edge)
        /// </summary>
        /// <param name="x">this parameter is the x postion of the block</param>
        /// <param name="y">this parameter is the y position of the block</param>
        /// <param name="face"></param>
        /// <returns>the block</returns>
        public List<Block> GetBlock(Color color, Boolean edge)
        {
            List<Block> Blocks = this.blocks;

            var matching_blocks = from Block block in Blocks select block;
            if (edge)
            {
                matching_blocks = from Block block in Blocks
                                  where (block.top == color ||
                                        block.bottom == color ||
                                        block.front == color ||
                                        block.back == color ||
                                        block.left == color ||
                                        block.right == color) &&
                                        ((block.top == Color.Hidden && block.bottom == Color.Hidden) ||
                                          (block.left == Color.Hidden && block.right == Color.Hidden) ||
                                          (block.front == Color.Hidden && block.back == Color.Hidden))
                                  select block;
            }
            else
            {
                matching_blocks = from Block block in Blocks
                                  where (block.top == color ||
                                        block.bottom == color ||
                                        block.front == color ||
                                        block.back == color ||
                                        block.left == color ||
                                        block.right == color)
                                  select block;
            }
            List<Block> toReturn = new List<Block>();
            foreach (Block b in matching_blocks)
            {
                toReturn.Add(b);
            }
            return toReturn;
        }
        /// <summary>
        /// this functions fetches a block from the list of blocks and the face
        /// </summary>
        /// <param name="x">this parameter is the x postion of the block</param>
        /// <param name="y">this parameter is the y position of the block</param>
        /// <param name="face"></param>
        /// <returns>the block</returns>
        public List<Block> GetBlock(Color color, Boolean edge, List<Block> Blocks)
        {
            var matching_blocks=from Block block in Blocks select block;
            if (edge)
            {
                matching_blocks = from Block block in Blocks
                                      where (block.top == color ||
                                            block.bottom == color ||
                                            block.front == color ||
                                            block.back == color ||
                                            block.left == color ||
                                            block.right == color) &&
                                            ( (block.top==Color.Hidden && block.bottom==Color.Hidden) ||
                                              (block.left == Color.Hidden && block.right==Color.Hidden) ||
                                              (block.front==Color.Hidden && block.back==Color.Hidden) )
                                      select block;
            }
            else
            {
                matching_blocks = from Block block in Blocks
                                      where (block.top == color ||
                                            block.bottom == color ||
                                            block.front == color ||
                                            block.back == color ||
                                            block.left == color ||
                                            block.right == color)
                                      select block;
            }
            List<Block> toReturn = new List<Block>();
            foreach(Block b in matching_blocks) {
                toReturn.Add(b);
            }
            return toReturn;
        }

        /// <summary>
        /// this function gets a slice of a part of the cube
        /// </summary>
        /// <param name="position">this parameter is the postion needed</param>
        /// <returns>a list of blocks</returns>
        public List<Block> GetSlice(Color position)
        {
            return GetSlice(position, this.blocks);
        }

        
        /// <summary>
        /// When we don't need to specify an axis i.e. an outer slice
        /// </summary>
        /// <param name="position"></param>
        /// <param name="dataset"></param>
        /// <returns>a list of blocks</returns>

         
        public List<Block> GetSlice(Color position, List<Block> dataset)
        {
            return GetSlice(position, dataset, 0);
        }

        /**
         * Gets a slice of the 
         * A slice is defined by the 9 cubes of a face that are on
         * the specified color's axis.
         * 
         * Color.Hidden requries an axis, otherwise we don't know which
         * slice we are looking for.
         */
        public List<Block> GetSlice(Color position, List<Block> dataset, int axis)
        {
            var slice = from Block block in dataset select block;
            switch (position)
            {
                case Color.Blue:
                    slice = from Block block in dataset
                                where block.back != Color.Hidden &&
                                block.front == Color.Hidden
                                select block;
                    break;
                case Color.Green:
                    slice = from Block block in dataset
                                where block.front != Color.Hidden &&
                                block.back == Color.Hidden
                                select block;
                    break;
                case Color.Hidden:
                    //@todo someone fix this so we don't need helper
                    if (axis == 0)
                    {//x
                        slice = from Block block in dataset
                                where block.left == Color.Hidden && block.right == Color.Hidden
                                select block;

                    }
                    else if (axis == 1)
                    {//y
                        slice = from Block block in dataset
                                where block.bottom == Color.Hidden && block.top == Color.Hidden
                                select block;

                    }
                    else if (axis == 2)
                    {//z
                        slice = from Block block in dataset
                                where block.front == Color.Hidden && block.back == Color.Hidden
                                select block;

                    }
                    break;
                case Color.Purple:
                    slice = from Block block in dataset
                                where block.left != Color.Hidden &&
                                block.right == Color.Hidden
                                select block;
                    break;
                case Color.Red:
                    slice = from Block block in dataset
                                where block.right != Color.Hidden &&
                                block.left == Color.Hidden
                                select block;
                    break;
                case Color.White:
                    slice = from Block block in dataset
                                where block.top != Color.Hidden &&
                                block.bottom == Color.Hidden
                                select block;
                    break;
                case Color.Yellow:
                    slice = from Block block in dataset
                                where block.bottom != Color.Hidden &&
                                block.top == Color.Hidden
                                select block;
                    break;
            }
            List<Block> toReturn = new List<Block>();
            foreach (Block b in slice)
            {
                toReturn.Add(b);
            }
            return toReturn;
        }
        /// <summary>
        /// this function prints the postion of a block to the console
        /// </summary>
        public void Print()
        {
            for (int z = 0; z < 3; z++)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        Console.WriteLine("(" + x + "," + y + "," + z + ")\n" + GetBlock(x, y, z));
                    }
                }
            }
        }
     }
}
