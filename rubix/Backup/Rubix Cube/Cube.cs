using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Rubix
{
    public enum Color { White, Yellow, Red, Green, Orange, Blue, Hidden };
    public enum Direction { Clockwise, CounterClockwise };

    class Cube
    {
        public List<Block> blocks;

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
        
        /**
         * The cube is oriented with White on top and Green as front face.
         */
        public Cube()
        {
            blocks = new List<Block>();
            //top
            //front
            blocks.Add(new Corner(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Orange, Color.Hidden));
            blocks.Add(new Edge(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Corner(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Red));
            //mid
            blocks.Add(new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Hidden, Color.Orange, Color.Hidden));
            blocks.Add(new Middle(Color.White, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Red));
            //back
            blocks.Add(new Corner(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Orange, Color.Hidden));
            blocks.Add(new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Hidden));
            blocks.Add(new Corner(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Red));

            //middle
            //front
            blocks.Add(new Edge(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Orange, Color.Hidden));
            blocks.Add(new Middle(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Red));
            //mid
            blocks.Add(new Middle(Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Orange, Color.Hidden));
            blocks.Add(new Hidden(Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Middle(Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Red));
            //back
            blocks.Add(new Edge(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Orange, Color.Hidden));
            blocks.Add(new Middle(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Red));

            //bottom
            //front
            blocks.Add(new Corner(Color.Hidden, Color.Yellow, Color.Green, Color.Hidden, Color.Orange, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Yellow, Color.Green, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Corner(Color.Hidden, Color.Yellow, Color.Green, Color.Hidden, Color.Hidden, Color.Red));
            //mid
            blocks.Add(new Edge(Color.Hidden, Color.Yellow, Color.Hidden, Color.Hidden, Color.Orange, Color.Hidden));
            blocks.Add(new Middle(Color.Hidden, Color.Yellow, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Yellow, Color.Hidden, Color.Hidden, Color.Hidden, Color.Red));
            //back
            blocks.Add(new Corner(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Orange, Color.Hidden));
            blocks.Add(new Edge(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Hidden, Color.Hidden));
            blocks.Add(new Corner(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Hidden, Color.Red));
            
        }

        public void Solve()
        {
            Solver solver = new Solver(this);
            solver.SolveWhiteCross();
            //solver.SolveMiddleLayer(Color.Red);
        }

        public void Scramble()
        {
            Random RandomClass = new Random();
            int number_of_total_rotations = RandomClass.Next(75, 150);
            Direction direction_to_rotate;
            Color color_to_rotate;

            //scramble the cube;
            while (--number_of_total_rotations != 0)
            {
                int direction_to_rotate_i = RandomClass.Next(100, 200);
                //note: generates number greater than or equal to one, and less than 7.
                int color_to_rotate_i = RandomClass.Next(1, 7);

                //set the direction
                if ((direction_to_rotate_i % 2) == 0)
                    direction_to_rotate = Direction.Clockwise;
                else
                    direction_to_rotate = Direction.CounterClockwise;

                //set the face color to rotate
                switch (color_to_rotate_i)
                {
                    case 1:
                        color_to_rotate = Color.Blue;
                        break;
                    case 2:
                        color_to_rotate = Color.Green;
                        break;
                    case 3:
                        color_to_rotate = Color.Orange;
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

                Console.WriteLine(number_of_total_rotations + ". Rotating " + color_to_rotate + " " + direction_to_rotate);
            }
        }

        /**
         * Moves all of the blocks in the specified color slice
         * the direction specified.
         */
        public void Rotate(Color color, Direction direction)
        {
            List<Block> slice = GetSlice(color);
            Orientor orientation = GetOrientation(color);

            Edge leftmid = (Edge)GetBlock(0, 1, color);
            Edge topmid = (Edge)GetBlock(1, 2, color);
            Edge rightmid = (Edge)GetBlock(2, 1, color);
            Edge botmid = (Edge)GetBlock(1, 0, color);

            Corner topleft = (Corner)GetBlock(0, 2, color);
            Corner topright = (Corner)GetBlock(2, 2, color);
            Corner botleft = (Corner)GetBlock(0, 0, color);
            Corner botright = (Corner)GetBlock(2, 0, color);

            if (direction == Direction.Clockwise)
            {
                Edge edge_bucket = leftmid;
                leftmid = (Edge)botmid.Rotate(direction, color);
                botmid = (Edge)rightmid.Rotate(direction, color);
                rightmid = (Edge)topmid.Rotate(direction, color);
                topmid = (Edge)edge_bucket.Rotate(direction, color);

                Corner corner_bucket = topleft;
                topleft = (Corner)botleft.Rotate(direction, color);
                botleft = (Corner)botright.Rotate(direction, color);
                botright = (Corner)topright.Rotate(direction, color);
                topright = (Corner)corner_bucket.Rotate(direction, color);
            }
            else
            {
                Edge edge_bucket = leftmid;
                leftmid = (Edge)topmid.Rotate(direction, color);
                topmid = (Edge)rightmid.Rotate(direction, color);
                rightmid = (Edge)botmid.Rotate(direction, color);
                botmid = (Edge)edge_bucket.Rotate(direction, color);

                Corner corner_bucket = topleft;
                topleft = (Corner)topright.Rotate(direction, color);
                topright = (Corner)botright.Rotate(direction, color);
                botright = (Corner)botleft.Rotate(direction, color);
                botleft = (Corner)corner_bucket.Rotate(direction, color);
            }
            //Console.WriteLine("Rotated " + color + " to " + direction);
        }

        /**
         * Returns a Block type that represnts an orientation of a the axis with
         * the given argument as the face of the 
         * 
         * Used in the rotation function.
         */
        public static Orientor GetOrientation(Color face)
        {
            Orientor orientor;

            switch(face)
            {
                case Color.White:
                    orientor = new Orientor(Color.Red, Color.Orange, Color.White, Color.Yellow, Color.Blue, Color.Green);
                    break;
                case Color.Yellow:
                    orientor = new Orientor(Color.Red, Color.Orange, Color.Yellow, Color.White, Color.Green, Color.Blue);
                    break;
                case Color.Red:
                    orientor = new Orientor(Color.White, Color.Yellow, Color.Red, Color.Orange, Color.Green, Color.Blue);
                    break;
                case Color.Orange:
                    orientor = new Orientor(Color.White, Color.Yellow,Color.Orange,Color.Red, Color.Blue, Color.Green);
                    break;
                case Color.Green:
                    orientor = new Orientor(Color.White, Color.Yellow, Color.Green, Color.Blue, Color.Orange, Color.Red);
                    break;
                case Color.Blue:
                    orientor = new Orientor(Color.White, Color.Yellow, Color.Blue, Color.Green, Color.Red, Color.Orange);
                    break;
                default:
                    throw new Exception("Invalid face");
            }
            return orientor;
        }

        /**
         * Used for when this is an orientation, will return the block in the
         * position specified given a face.
         */
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

        public Block GetBlock(int x, int y, int z)
        {
            List<Block> dataset;
            switch (x) {
                case 0:
                    dataset = GetSlice(Color.Orange);
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

        /**
         * Convience if we want to GetSlice with full 27 cubes
         */
        public List<Block> GetSlice(Color position)
        {
            return GetSlice(position, this.blocks);
        }

        /**
         * When we don't need to specify an axis i.e. an outer slice
         */
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
                case Color.Orange:
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
