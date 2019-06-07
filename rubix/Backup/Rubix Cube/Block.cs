using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
namespace Rubix
{
    public abstract class Block
    {
        public Color top;
        public Color bottom;
        public Color front;
        public Color back;
        public Color left;
        public Color right;

        public CustomVertex.PositionColored[] colorFace(CustomVertex.PositionColored[] vertices, System.Drawing.Color c)
        {
            vertices[0].Color = c.ToArgb();
            vertices[1].Color = c.ToArgb();
            vertices[2].Color = c.ToArgb();
            vertices[3].Color = c.ToArgb();
            return vertices;
        }

        //draw whole cube
        public void draw(float x,float y,float z)
        {
            CustomVertex.PositionColored[] vertices = new CustomVertex.PositionColored[8];

            /*
             * top: 
             * 56
             * 47
             * bottom: 
             * 12
             * 03
             */
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
                case Color.Orange:
                    return System.Drawing.Color.Orange;
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

        public Block(Color top, Color bottom, Color front, Color back, Color left, Color right)
        {
            this.top = top;
            this.bottom = bottom;
            this.front = front;
            this.back = back;
            this.left = left;
            this.right = right;
        }

        /**
         * Rotates a block along a given axis
         */
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
                      (axis==Color.Orange && direction==Direction.CounterClockwise) )
            { // z axis

                bucket = this.top;
                this.top = this.front;
                this.front = this.bottom;
                this.bottom = this.back;
                this.back = bucket;
            } 
            else if ( (axis==Color.Red && direction==Direction.CounterClockwise) ||
                      (axis==Color.Orange && direction==Direction.Clockwise) ) 
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
        
        public override string ToString()
        {
            return "Left\tTop\tRight\tBottom\tBack\tFront\n" + left + "\t" + top + "\t" + right + "\t" + bottom + "\t" + back + "\t" + front;
        }

        /* This is only because the Object.Equals(Object) not working */
        public Boolean Equals(Block block)
        {
            return (this.back == block.back &&
                    this.bottom == block.bottom &&
                    this.front == block.front &&
                    this.left == block.left &&
                    this.right == block.right &&
                    this.top == block.top);
        }

        /*
         * function that returns if a given block contains a certain color
         */
        public Boolean HasColor(Color color)
        {
            return (this.back == color ||
                    this.bottom == color ||
                    this.front == color ||
                    this.left == color ||
                    this.right == color ||
                    this.top == color);
        }

        public void Print()
        {
            Console.WriteLine("=== {0} ===", this.GetType());
            Console.WriteLine("Top: {0}, Bottom: {1}, Front: {2}, Back: {3}, Left: {4}, Right: {5}",
                              top, bottom, front, back, left, right);
        }
    }

    public class Corner : Block
    {
        public Corner(Color top, Color bottom, Color front, Color back, Color left, Color right) 
            : base(top, bottom, front, back, left, right) 
        {
        }
    }

    public class Edge : Block
    {
        public Edge(Color top, Color bottom, Color front, Color back, Color left, Color right) 
            : base(top, bottom, front, back, left, right) 
        {
        }
    }

    public class Middle : Block
    {
        public Middle(Color top, Color bottom, Color front, Color back, Color left, Color right)
            : base(top, bottom, front, back, left, right)
        {
        }
    }

    public class Hidden : Block
    {
        public Hidden(Color top, Color bottom, Color front, Color back, Color left, Color right)
            : base(top, bottom, front, back, left, right)
        {
        }
    }

    public class Orientor : Block
    {
        public Orientor(Color top, Color bottom, Color front, Color back, Color left, Color right)
            : base(top, bottom, front, back, left, right)
        {
        }
    }
}
