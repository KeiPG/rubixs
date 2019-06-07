using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
namespace Rubix
{
    /// <summary>
    /// this class construtor. this makes the colors for each face.
    /// </summary>
    public abstract class Block
    {
        public Color top;
        public Color bottom;
        public Color front;
        public Color back;
        public Color left;
        public Color right;
        /// <summary>
        /// sets up the vertices for a face set up.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public CustomVertex.PositionColored[] colorFace(CustomVertex.PositionColored[] vertices, System.Drawing.Color c)
        {
            vertices[0].Color = c.ToArgb();
            vertices[1].Color = c.ToArgb();
            vertices[2].Color = c.ToArgb();
            vertices[3].Color = c.ToArgb();
            return vertices;
        }

        /// <summary>
        /// draws the blocks. this makes 3D vectors for each of the corners then using directX joins them up with lines to make a 
        /// outline of the block. the x,y,z parameters are the location of the first point
        /// </summary>
        /// <param name="x">this parameter is x postion</param>
        /// <param name="y">this parameter is y postion</param>
        /// <param name="z">this parameter is z postion</param>
        public void draw(float x,float y,float z)
        {
            CustomVertex.PositionColored[] vertices = new CustomVertex.PositionColored[8];

            
            //bottom left front block vertex
            vertices[0].Position = new Vector3(x, y, z);
            vertices[1].Position = new Vector3(x, y, z + UI.scale);
            vertices[2].Position = new Vector3(x + UI.scale, y, z + UI.scale);
            vertices[3].Position = new Vector3(x + UI.scale, y, z);
            vertices[4].Position = new Vector3(x, y + UI.scale, z);
            vertices[5].Position = new Vector3(x, y + UI.scale, z + UI.scale);
            vertices[6].Position = new Vector3(x + UI.scale, y + UI.scale, z + UI.scale);
            vertices[7].Position = new Vector3(x + UI.scale, y + UI.scale, z);

            UI.d3ddevice.VertexFormat = CustomVertex.PositionColored.Format;

            CustomVertex.PositionColored[] frontside = { vertices[0], vertices[4], vertices[7], vertices[3] };
            frontside = colorFace(frontside, mapColor(front));
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.TriangleFan, 2, frontside);
            CustomVertex.PositionColored[] flines = { vertices[0], vertices[4], vertices[4], vertices[7], vertices[7], vertices[3], vertices[3], vertices[0] };
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.LineList, 4, flines);
            

            CustomVertex.PositionColored[] backside = { vertices[1], vertices[5], vertices[6], vertices[2] };
            frontside = colorFace(backside, mapColor(back));
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.TriangleFan, 2, backside);
            CustomVertex.PositionColored[] blines = { vertices[1], vertices[5], vertices[5], vertices[6], vertices[6], vertices[2], vertices[2], vertices[1] };
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.LineList, 4, blines);

            CustomVertex.PositionColored[] leftside = { vertices[0], vertices[1], vertices[5], vertices[4] };
            frontside = colorFace(leftside, mapColor(left));
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.TriangleFan, 2, leftside);
            CustomVertex.PositionColored[] llines = { vertices[0], vertices[1], vertices[1], vertices[5], vertices[5], vertices[4], vertices[4], vertices[0] };
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.LineList, 4, flines);

            CustomVertex.PositionColored[] rightside = { vertices[3], vertices[7], vertices[6], vertices[2] };
            frontside = colorFace(rightside, mapColor(right));
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.TriangleFan, 2, rightside);
            CustomVertex.PositionColored[] rlines = { vertices[3], vertices[7], vertices[7], vertices[6], vertices[6], vertices[2], vertices[2], vertices[3] };
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.LineList, 4, rlines);

            CustomVertex.PositionColored[] topside = { vertices[4], vertices[5], vertices[6], vertices[7] };
            frontside = colorFace(topside, mapColor(top));
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.TriangleFan, 2, topside);
            CustomVertex.PositionColored[] tlines = { vertices[4], vertices[5], vertices[5], vertices[6], vertices[6], vertices[7], vertices[7], vertices[4] };
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.LineList, 4, tlines);

            CustomVertex.PositionColored[] bottomside = { vertices[0], vertices[1], vertices[2], vertices[3] };
            frontside = colorFace(bottomside, mapColor(bottom));
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.TriangleFan, 2, bottomside);
            CustomVertex.PositionColored[] botlines = { vertices[0], vertices[1], vertices[1], vertices[2], vertices[2], vertices[3], vertices[3], vertices[0] };
            UI.d3ddevice.DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType.LineList, 4, botlines);
        }

        /// <summary>
        /// this function try to assign a colour to the face
        /// </summary>
        /// <param name="rubixColor">this parameter holds the color to be checked</param>
        /// <returns></returns>
        public static System.Drawing.Color mapColor(Rubix.Color rubixColor)
        {
            switch (rubixColor)
            {
                case Color.Blue:
                    return System.Drawing.Color.Blue;
                case Color.Green:
                    return System.Drawing.Color.Green;
                case Color.Hidden:
                    return System.Drawing.Color.Black;
                case Color.Purple:
                    return System.Drawing.Color.Purple;
                case Color.Red:
                    return System.Drawing.Color.Red;
                case Color.White:
                    return System.Drawing.Color.White;
                case Color.Yellow:
                    return System.Drawing.Color.Yellow;
                default:
                    throw new Exception("Mapping non mapable color");
            }
        }
        /// <summary>
        /// this function contructs the block with the colours input
        /// </summary>
        /// <param name="top">this parameter is top face</param>
        /// <param name="bottom">this parameter is bottom</param>
        /// <param name="front">this parameter is fornt</param>
        /// <param name="back">this parameter is back</param>
        /// <param name="left">this parameter is left</param>
        /// <param name="right">this parameter is right</param>
        public Block(Color top, Color bottom, Color front, Color back, Color left, Color right)
        {
            this.top = top;
            this.bottom = bottom;
            this.front = front;
            this.back = back;
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// this function rotates the block along a certain axis
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public Block Rotate(Direction direction, Color axis)
        {
            Color bucket = this.top;
            
            if ( (axis == Color.White && direction==Direction.Clockwise) ||
                 (axis == Color.Yellow && direction==Direction.CounterClockwise)) 
            { // x axis
                bucket = this.front;
                this.front = this.right;
                this.right = this.back;
                this.back = this.left;
                this.left = bucket;
            } 
            else if ( (axis==Color.White && direction==Direction.CounterClockwise) ||
                      (axis==Color.Yellow && direction==Direction.Clockwise))
            {
                bucket = this.front;
                this.front = this.left;
                this.left = this.back;
                this.back = this.right;
                this.right = bucket;
            }
            else if ( (axis==Color.Red && direction==Direction.Clockwise) ||
                      (axis==Color.Purple && direction==Direction.CounterClockwise) )
            { // z axis

                bucket = this.top;
                this.top = this.front;
                this.front = this.bottom;
                this.bottom = this.back;
                this.back = bucket;
            } 
            else if ( (axis==Color.Red && direction==Direction.CounterClockwise) ||
                      (axis==Color.Purple && direction==Direction.Clockwise) ) 
            {
                bucket = this.top;
                this.top = this.back;
                this.back = this.bottom;
                this.bottom = this.front;
                this.front = bucket;
            }
            else if ( (axis==Color.Green && direction==Direction.CounterClockwise) ||
                      (axis==Color.Blue && direction==Direction.Clockwise) ) 
            { // y axis
                bucket = this.top;
                this.top = this.right;
                this.right = this.bottom;
                this.bottom = this.left;
                this.left = bucket;
            }
            else if ((axis == Color.Green && direction == Direction.Clockwise) ||
                      (axis == Color.Blue && direction == Direction.CounterClockwise))
            {
                bucket = this.top;
                this.top = this.left;
                this.left = this.bottom;
                this.bottom = this.right;
                this.right = bucket;
            }
            else
            {
                throw new Exception("Wrong axis specified");
            }
            return this;
        }
        /// <summary>
        /// this function changes the block into a string to be checked easliy(to be understood)
        /// </summary>
        /// <returns>the string</returns>
        public override string ToString()
        {
            return "Left\tTop\tRight\tBottom\tBack\tFront\n" + left + "\t" + top + "\t" + right + "\t" + bottom + "\t" + back + "\t" + front;
        }

        /// <summary>
        /// this function checks if two block are the same as each other
        /// </summary>
        /// <param name="block">the check block</param>
        /// <returns>true or false</returns>
        public Boolean Equals(Block block)
        {
            return (this.back == block.back &&
                    this.bottom == block.bottom &&
                    this.front == block.front &&
                    this.left == block.left &&
                    this.right == block.right &&
                    this.top == block.top);
        }

        /// <summary>
        /// this function check if a block has a certain coloured face
        /// </summary>
        /// <param name="color">this parameter is the color that you are checking if it is on</param>
        /// <returns>true or false</returns>
        public Boolean HasColor(Color color)
        {
            return (this.back == color ||
                    this.bottom == color ||
                    this.front == color ||
                    this.left == color ||
                    this.right == color ||
                    this.top == color);
        }
        /// <summary>
        /// this function prints the block
        /// </summary>
        public void Print()
        {
            Console.WriteLine("=== {0} ===", this.GetType());
            Console.WriteLine("Top: {0}, Bottom: {1}, Front: {2}, Back: {3}, Left: {4}, Right: {5}",
                              top, bottom, front, back, left, right);
        }
    }
    /// <summary>
    /// this class is a block that is a corner and has 3 colored faces
    /// </summary>
    public class Corner : Block
    {
        public Corner(Color top, Color bottom, Color front, Color back, Color left, Color right) 
            : base(top, bottom, front, back, left, right) 
        {
        }
    }
    /// <summary>
    /// this class is a block that is a corner and has 2 colored faces
    /// </summary>
    public class Edge : Block
    {
        public Edge(Color top, Color bottom, Color front, Color back, Color left, Color right) 
            : base(top, bottom, front, back, left, right) 
        {
        }
    }
    /// <summary>
    /// this class is a block that is a corner and has 1 colored faces
    /// </summary>
    public class Middle : Block
    {
        public Middle(Color top, Color bottom, Color front, Color back, Color left, Color right)
            : base(top, bottom, front, back, left, right)
        {
        }
    }
    /// <summary>
    /// this class is a block that is a corner and has 0 colored faces
    /// </summary>
    public class Hidden : Block
    {
        public Hidden(Color top, Color bottom, Color front, Color back, Color left, Color right)
            : base(top, bottom, front, back, left, right)
        {
        }
    }
    /// <summary>
    /// this function makes a block called orientor used in rotations and other functions
    /// </summary>
    public class Orientor : Block
    {
        public Orientor(Color top, Color bottom, Color front, Color back, Color left, Color right)
            : base(top, bottom, front, back, left, right)
        {
        }
    }
}
