using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Rubix
{
    class Solver
    {
        private Cube cube;

        public Solver(Cube cube)
        {
            this.cube = cube;
        }

        /**
         * Written by Nathan: Step one in solving the rubiks cube.  
         * This will solve the white cross on top of the cube.
         * NOTE: This is the version I had before cloning Luke's repo.
         */
        public void SolveWhiteCross()
        {
            while (!WhiteCrossSolved())
            {
                List<Block> yellow_slice = cube.GetSlice(Color.Yellow);
                Orientor front_face;
                Edge edge_piece;
                Boolean block_with_white_on_bottom = false, block_with_white_in_center, block_on_top_not_oriented_correctly;

                //will take any edge piece with the white on the bottom and place it where it needs to be.
                foreach (Block block_in_yellow_slice in yellow_slice)
                {
                    //if the blocks in the yellow slice that have a white color also have a green color
                    //is an edge piece
                    if (cube.GetBlock(Color.Green, true, cube.GetBlock(Color.White, true, yellow_slice)).Count == 1)
                    {
                        block_with_white_on_bottom = true;

                        edge_piece = (Edge)cube.GetBlock(Color.Green, true, cube.GetBlock(Color.White, true, yellow_slice))[0];

                        //while the edgepeice isn't directly under the side it needs to be,
                        //cube.Rotate it until it is.
                        while (!edge_piece.Equals(cube.GetBlock(1, 0, 0)))
                            cube.Rotate(Color.Yellow, Direction.Clockwise);

                        //get the block that will give you the orientation to use as the front face
                        front_face = Cube.GetOrientation(Color.Green);

                        PlaceBottomEdgePieceInCross(edge_piece, front_face);
                        //yellow_slice = cube.GetSlice(Color.Yellow);
                        break;
                    }
                    else if (cube.GetBlock(Color.Orange, true, cube.GetBlock(Color.White, true, yellow_slice)).Count == 1)
                    {
                        block_with_white_on_bottom = true;

                        edge_piece = (Edge)cube.GetBlock(Color.Orange, true, cube.GetBlock(Color.White, true, yellow_slice))[0];

                        while (!edge_piece.Equals(cube.GetBlock(0, 0, 1)))
                            cube.Rotate(Color.Yellow, Direction.Clockwise);

                        front_face = Cube.GetOrientation(Color.Orange);

                        PlaceBottomEdgePieceInCross(edge_piece, front_face);
                        //yellow_slice = cube.GetSlice(Color.Yellow);
                        break;
                    }
                    else if (cube.GetBlock(Color.Red, true, cube.GetBlock(Color.White, true, yellow_slice)).Count == 1)
                    {
                        block_with_white_on_bottom = true;

                        edge_piece = (Edge)cube.GetBlock(Color.Red, true, cube.GetBlock(Color.White, true, yellow_slice))[0];

                        while (!edge_piece.Equals(cube.GetBlock(2, 0, 1)))
                            cube.Rotate(Color.Yellow, Direction.Clockwise);

                        front_face = Cube.GetOrientation(Color.Red);

                        PlaceBottomEdgePieceInCross(edge_piece, front_face);
                        //yellow_slice = cube.GetSlice(Color.Yellow);
                        break;
                    }
                    else if (cube.GetBlock(Color.Blue, true, cube.GetBlock(Color.White, true, yellow_slice)).Count == 1)
                    {
                        block_with_white_on_bottom = true;

                        edge_piece = (Edge)cube.GetBlock(Color.Blue, true, cube.GetBlock(Color.White, true, yellow_slice))[0];
                        //Block temp = cube.GetBlock(1, 0, 2);

                        while (!edge_piece.Equals(cube.GetBlock(1, 0, 2)))
                            cube.Rotate(Color.Yellow, Direction.Clockwise);

                        front_face = Cube.GetOrientation(Color.Blue);

                        PlaceBottomEdgePieceInCross(edge_piece, front_face);
                        break;
                    }
                    else
                        block_with_white_on_bottom = false;
                }

                /*****************************************************
                 * if edge piece is in the center, then put it in the bottom layer,
                 * and the above will put it in the correct position.
                 */
                if (block_with_white_in_center = cube.GetBlock(0, 1, 0).HasColor(Color.White))
                    cube.Rotate(Color.Green, Direction.CounterClockwise);
                else if (block_with_white_in_center = cube.GetBlock(0, 1, 2).HasColor(Color.White))
                    cube.Rotate(Color.Orange, Direction.CounterClockwise);
                else if (block_with_white_in_center = cube.GetBlock(2, 1, 2).HasColor(Color.White))
                    cube.Rotate(Color.Blue, Direction.CounterClockwise);
                else if (block_with_white_in_center = cube.GetBlock(2, 1, 0).HasColor(Color.White))
                    cube.Rotate(Color.Red, Direction.CounterClockwise);
                else
                    block_with_white_in_center = false;

                if (block_with_white_in_center)
                    continue;
                /******************************************************/

                /*****************************************************
                 * if edge piece is in the correct position, but the incorrect
                 * orientation, then put it in the bottom layer, and let the 
                 * placeBottomEdgePieceInCross(...) function handle puting it
                 * in the correct position
                 */
                if (block_on_top_not_oriented_correctly = cube.GetBlock(1, 2, 0).front == Color.White)
                    front_face = Cube.GetOrientation(Color.Green);
                else if (block_on_top_not_oriented_correctly = cube.GetBlock(0, 2, 1).left == Color.White)
                    front_face = Cube.GetOrientation(Color.Orange);
                else if (block_on_top_not_oriented_correctly = cube.GetBlock(1, 2, 2).back == Color.White)
                    front_face = Cube.GetOrientation(Color.Blue);
                else if (block_on_top_not_oriented_correctly = cube.GetBlock(2, 2, 1).right == Color.White)
                    front_face = Cube.GetOrientation(Color.Red);
                else
                {
                    front_face = null;
                    block_on_top_not_oriented_correctly = false;
                }

                if (block_on_top_not_oriented_correctly)
                {
                    cube.Rotate(front_face.front, Direction.Clockwise);
                    cube.Rotate(front_face.front, Direction.Clockwise);
                }
                /*****************************************************/

                /*
                 * if the cross is solved, but incorreclty oriented
                 */
                if (!block_with_white_on_bottom &&
                    !block_with_white_in_center &&
                    !block_on_top_not_oriented_correctly)
                    CheckWhiteCross();
            }
        }

        /*
     * Function that decides whether the white cross is solved on top (part of step 1 of algorithm)
     */
        public Boolean WhiteCrossSolved()
        {
            Edge cross_bottom = new Edge(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Hidden);
            Edge cross_left = new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Hidden, Color.Orange, Color.Hidden);
            Edge cross_top = new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Hidden);
            Edge cross_right = new Edge(Color.White, Color.Hidden, Color.Hidden, Color.Hidden, Color.Hidden, Color.Red);

            return (cross_bottom.Equals(cube.GetBlock(1, 2, 0)) &&
                     cross_left.Equals(cube.GetBlock(0, 2, 1)) &&
                     cross_right.Equals(cube.GetBlock(2, 2, 1)) &&
                     cross_top.Equals(cube.GetBlock(1, 2, 2)));
        }

        /*
         *  Written by Nathan: Method to solve step two of the algorithm, solving the white top corners. 
         */
        public void SolveWhiteCorners()
        {
            //blocks that represent the corners in their proper positions
            Corner front_left_corner_in_prop_pos = new Corner(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Orange, Color.Hidden);
            Corner front_right_corner_in_prop_pos = new Corner(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Red);
            Corner back_right_corner_in_prop_pos = new Corner(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Red);
            Corner back_left_corner_in_prop_pos = new Corner(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Orange, Color.Hidden);

            //get the corner block that has a green, white and orange side
            Corner front_left_corner = (Corner)cube.GetBlock(Color.Orange, false, cube.GetBlock(Color.Green, false, cube.GetBlock(Color.White, false)))[0];
            Corner front_right_corner = (Corner)cube.GetBlock(Color.Red, false, cube.GetBlock(Color.Green, false, cube.GetBlock(Color.White, false)))[0];
            Corner back_right_corner = (Corner)cube.GetBlock(Color.Red, false, cube.GetBlock(Color.Blue, false, cube.GetBlock(Color.White, false)))[0];
            Corner back_left_corner = (Corner)cube.GetBlock(Color.Orange, false, cube.GetBlock(Color.Blue, false, cube.GetBlock(Color.White, false)))[0];

            int max_sequence_applications = 5;
            int max_rotations = 4;
            Boolean corner_in_bottom_layer = true;

            while (!WhiteCornersSolved())
            {
                //if the front left corner peice isn't in the proper position
                if (!front_left_corner_in_prop_pos.Equals(front_left_corner))
                {
                    //if the front left corner piece is in the correct position, but oriented incorrecly
                    //then perform this step's rotation algorithm
                    if (front_left_corner.Equals(cube.GetBlock(0, 2, 0)))
                        CornersRotation(Color.Orange);

                    corner_in_bottom_layer = true;
                    //while the front left corner isn't directly underneith where it's supposed to be
                    //then cube.Rotate it until it is, will stop if doesn't find after 4 rotations
                    while (!front_left_corner.Equals(cube.GetBlock(0, 0, 0)) && (max_rotations-- > 0))
                    {
                        cube.Rotate(Color.Yellow, Direction.Clockwise);
                        //if you have cube.Rotated the yellow face four times, and the corner still isn't where
                        //it's supposed to be
                        if (max_rotations == 0)
                            corner_in_bottom_layer = false;
                    }

                    //reset max_rotations
                    max_rotations = 4;

                    //if corner not in the correct position, figure out where it is, and put it in the bottom layer.
                    if (!corner_in_bottom_layer)
                    {
                        if (front_left_corner.Equals(cube.GetBlock(2, 2, 0)))
                            PutCornerInBottomLayer(Color.Green);
                        else if (front_left_corner.Equals(cube.GetBlock(2, 2, 2)))
                            PutCornerInBottomLayer(Color.Red);
                        else if (front_left_corner.Equals(cube.GetBlock(0, 2, 2)))
                            PutCornerInBottomLayer(Color.Blue);
                        continue;
                    }
                    else
                    {
                        //while the corner piece isn't in the correct position, and while the max number of times
                        //the rotation algorithm (5 times) isn't exceeded: perfrom this step's rotation algorithm
                        while (!front_left_corner_in_prop_pos.Equals(front_left_corner) && (max_sequence_applications-- > 0))
                        {
                            CornersRotation(Color.Orange);
                            max_sequence_applications = 5;
                        }
                    }
                }
                if (!front_right_corner_in_prop_pos.Equals(front_right_corner))
                {
                    if (front_right_corner.Equals(cube.GetBlock(2, 2, 0)))
                        CornersRotation(Color.Green);

                    corner_in_bottom_layer = true;
                    while (!front_right_corner.Equals(cube.GetBlock(2, 0, 0)) && (max_rotations-- > 0))
                    {
                        cube.Rotate(Color.Yellow, Direction.Clockwise);
                        if (max_rotations == 0)
                            corner_in_bottom_layer = false;
                    }
                    max_rotations = 4;

                    if (!corner_in_bottom_layer)
                    {
                        if (front_right_corner.Equals(cube.GetBlock(0, 2, 0)))
                            PutCornerInBottomLayer(Color.Orange);
                        else if (front_right_corner.Equals(cube.GetBlock(2, 2, 2)))
                            PutCornerInBottomLayer(Color.Red);
                        else if (front_right_corner.Equals(cube.GetBlock(0, 2, 2)))
                            PutCornerInBottomLayer(Color.Blue);
                        continue;
                    }
                    else
                    {
                        while (!front_right_corner_in_prop_pos.Equals(front_right_corner) && (max_sequence_applications-- > 0))
                        {
                            CornersRotation(Color.Green);
                            max_sequence_applications = 5;
                        }
                    }
                }
                if (!back_right_corner_in_prop_pos.Equals(back_right_corner))
                {
                    if (back_right_corner.Equals(cube.GetBlock(2, 2, 2)))
                        CornersRotation(Color.Red);

                    corner_in_bottom_layer = true;
                    while (!back_right_corner.Equals(cube.GetBlock(2, 0, 2)) && (max_rotations-- > 0))
                    {
                        cube.Rotate(Color.Yellow, Direction.Clockwise);
                        if (max_rotations == 0)
                            corner_in_bottom_layer = false;
                    }
                    max_rotations = 4;

                    if (!corner_in_bottom_layer)
                    {
                        if (back_right_corner.Equals(cube.GetBlock(2, 2, 0)))
                            PutCornerInBottomLayer(Color.Green);
                        else if (back_right_corner.Equals(cube.GetBlock(0, 2, 0)))
                            PutCornerInBottomLayer(Color.Orange);
                        else if (back_right_corner.Equals(cube.GetBlock(0, 2, 2)))
                            PutCornerInBottomLayer(Color.Blue);
                        continue;
                    }
                    else
                    {
                        while (!back_right_corner_in_prop_pos.Equals(back_right_corner) && (max_sequence_applications-- > 0))
                        {
                            CornersRotation(Color.Red);
                            max_sequence_applications = 5;
                        }
                    }
                }
                if (!back_left_corner_in_prop_pos.Equals(back_left_corner))
                {
                    if (back_left_corner.Equals(cube.GetBlock(0, 2, 2)))
                        CornersRotation(Color.Blue);

                    corner_in_bottom_layer = true;
                    while (!back_left_corner.Equals(cube.GetBlock(0, 0, 2)) && (max_rotations-- > 0))
                    {
                        cube.Rotate(Color.Yellow, Direction.Clockwise);
                        if (max_rotations == 0)
                            corner_in_bottom_layer = false;
                    }
                    max_rotations = 4;

                    if (!corner_in_bottom_layer)
                    {
                        if (back_left_corner.Equals(cube.GetBlock(2, 2, 0)))
                            PutCornerInBottomLayer(Color.Green);
                        else if (back_left_corner.Equals(cube.GetBlock(2, 2, 2)))
                            PutCornerInBottomLayer(Color.Red);
                        else if (back_left_corner.Equals(cube.GetBlock(0, 2, 0)))
                            PutCornerInBottomLayer(Color.Orange);
                        continue;
                    }
                    else
                    {
                        while (!back_left_corner_in_prop_pos.Equals(back_left_corner) && (max_sequence_applications-- > 0))
                        {
                            CornersRotation(Color.Blue);
                            max_sequence_applications = 5;
                        }
                    }
                }
            }
        }

        /*
         * Written by Nathan: Performs the only rotation sequence needed for step two of the 
         * solving algorithm (to place white corners)
         */
        private void CornersRotation(Color front_face_color)
        {
            Orientor front_face = Cube.GetOrientation(front_face_color);

            //Ri Di R D
            cube.Rotate(front_face.right, Direction.CounterClockwise);
            cube.Rotate(front_face.bottom, Direction.CounterClockwise);
            cube.Rotate(front_face.right, Direction.Clockwise);
            cube.Rotate(front_face.bottom, Direction.Clockwise);
        }

        /*
         * Written by Nathan Function that decides whether the white corners are solved on top (step 2 of algorithm)
         */
        public Boolean WhiteCornersSolved()
        {
            Corner front_left_corner = new Corner(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Orange, Color.Hidden);
            Corner front_right_corner = new Corner(Color.White, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Red);
            Corner back_right_corner = new Corner(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Red);
            Corner back_left_corner = new Corner(Color.White, Color.Hidden, Color.Hidden, Color.Blue, Color.Orange, Color.Hidden);

            return (front_left_corner.Equals(cube.GetBlock(0, 2, 0)) &&
                     front_right_corner.Equals(cube.GetBlock(2, 2, 0)) &&
                     back_right_corner.Equals(cube.GetBlock(2, 2, 2)) &&
                     back_left_corner.Equals(cube.GetBlock(0, 2, 2)));
        }

        /*
         * Written by Nathan: Performs rotations to put a corner in the top layer of the cube in the bottom layer
         */
        private void PutCornerInBottomLayer(Color front_face_color)
        {
            Orientor front_face = Cube.GetOrientation(front_face_color);

            cube.Rotate(front_face.right, Direction.CounterClockwise);
            cube.Rotate(front_face.bottom, Direction.CounterClockwise);
            cube.Rotate(front_face.right, Direction.Clockwise);
        }

        /*
         * Helper function for cross, puts edge piece on D face in correct position.
         */
        private void PlaceBottomEdgePieceInCross(Edge edge_piece, Orientor front_face)
        {
            //if the edge piece has white facing bottom,
            //cube.Rotate face twice to put it in the correct position for the cross.
            if (edge_piece.bottom == Color.White)
            {
                cube.Rotate(front_face.front, Direction.Clockwise);
                cube.Rotate(front_face.front, Direction.Clockwise);
            }
            //else the other color (not white) is facing down, apply this algorithm
            //to put the piece in the correct position.
            else
            {
                cube.Rotate(front_face.bottom, Direction.Clockwise);
                cube.Rotate(front_face.right, Direction.Clockwise);
                cube.Rotate(front_face.front, Direction.CounterClockwise);
                cube.Rotate(front_face.right, Direction.CounterClockwise);
            }
        }

        /*
         * If the white cross is solved, but incorrectly oriented, this will orient it correctly.
         * If the white cross is solved, but with the edge pieces in the incorrect locations
         * this will scramble the incorrectly placed edges.
         */
        private void CheckWhiteCross()
        {
            //if the top cross exists
            if (cube.GetBlock(1, 2, 0).top == Color.White &&
                cube.GetBlock(0, 2, 1).top == Color.White &&
                cube.GetBlock(1, 2, 2).top == Color.White &&
                cube.GetBlock(2, 2, 1).top == Color.White)
            {
                //try rotating the white face up the three times to correclty orient the cross
                for (int i = 3; i > 0; i--)
                {
                    if (!WhiteCrossSolved())
                        cube.Rotate(Color.White, Direction.Clockwise);
                    else
                        return;
                }

                //otherwise the white cross is solved, but the edge pieces aren't where they're supposed
                //to be relative to eachother.  To make this code short, this will just scramble everything in the cross positions.
                cube.Rotate(Color.Green, Direction.Clockwise);
                cube.Rotate(Color.Orange, Direction.Clockwise);
                cube.Rotate(Color.Blue, Direction.Clockwise);
                cube.Rotate(Color.Red, Direction.Clockwise);
            }
        }

        /* Middle Layer 
         * This solution uses right recursion to pull off special cases
         * in the set of all cases which will lead to the solution.
         */
        public void SolveMiddleLayer(Color front, UI gui)
        {
            Edge gr_edge_corr_orient = new Edge(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Red);
            Edge rb_edge_corr_orient = new Edge(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Red);
            Edge bo_edge_corr_orient = new Edge(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Orange, Color.Hidden);
            Edge go_edge_corr_orient = new Edge(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Orange, Color.Hidden);
            /* BASE CASE */
            if (front > Color.Blue) // All sides have been solved
                return;

            Orientor orientation = Cube.GetOrientation(front);
            Edge target_edge = (Edge)cube.GetBlock(orientation.left, true, cube.GetBlock(orientation.front, true))[0];

            if (target_edge.Equals(gr_edge_corr_orient) ||
                target_edge.Equals(rb_edge_corr_orient) ||
                target_edge.Equals(bo_edge_corr_orient) ||
                target_edge.Equals(go_edge_corr_orient))
            {
                front++;
            }
            else if (target_edge.bottom == orientation.left || target_edge.bottom == orientation.front) // Edge is on bottom
            {
                Color side_where_found = RotateBottomEdgeIntoPlace(orientation, target_edge);

                if (side_where_found == orientation.front)
                    MiddleLayerEdgeFrontRotation(orientation);
                else
                    MiddleLayerEdgeLeftRotation(orientation);

                front++;
            }
            else // Edge is in middle layer
            {
                // Find our edge piece by rotating the cube and looking for it on the right
                Orientor target_orient = FindBlockInMiddleLayer(target_edge);
                MiddleLayerEdgeLeftRotation(target_orient);
            }

            //gui.Refresh();
            SolveMiddleLayer(front, gui);
        }

        private Orientor FindBlockInMiddleLayer(Edge target_edge)
        {
            Orientor orient;
            Color color = Color.Red;

            while (color <= Color.Blue)
            {
                orient = Cube.GetOrientation(color);
                Edge left = (Edge)cube.GetBlock(0, 1, color);

                if (left.Equals(target_edge))
                    return orient;
                else
                    color++;
            }

            throw new Exception("Middle layer failure");
        }

        private Color RotateBottomEdgeIntoPlace(Orientor oriented, Block edge)
        {
                /********  if edge supposed to be under oriented.front *******/
                if (edge.back == oriented.front ||
                    edge.front == oriented.front ||
                    edge.left == oriented.front ||
                    edge.right == oriented.front)
                {
                    if (oriented.front == Color.Green)
                    {
                        while(!edge.Equals(cube.GetBlock(1, 0, 0)))
                            cube.Rotate(oriented.bottom, Direction.Clockwise);
                    }
                    else if (oriented.front == Color.Red)
                    {
                        while (!edge.Equals(cube.GetBlock(2, 0, 1)))
                            cube.Rotate(oriented.bottom, Direction.Clockwise);
                    }
                    else if (oriented.front == Color.Blue)
                    {
                        while (!edge.Equals(cube.GetBlock(1, 0, 2)))
                            cube.Rotate(oriented.bottom, Direction.Clockwise);
                    }
                    else if (oriented.front == Color.Orange)
                    {
                        while (!edge.Equals(cube.GetBlock(0, 0, 1)))
                            cube.Rotate(oriented.bottom, Direction.Clockwise);
                    }
                    return oriented.front;
                }
               
                /*************************************************************/

                /********  if edge supposed to be under oriented.left *******/
                else if (edge.back == oriented.left ||
                    edge.front == oriented.left ||
                    edge.left == oriented.left ||
                    edge.right == oriented.left)
                {
                    if (oriented.left == Color.Green)
                    {
                        while (!edge.Equals(cube.GetBlock(1, 0, 0)))
                            cube.Rotate(oriented.bottom, Direction.Clockwise);
                    }
                    else if (oriented.left == Color.Red)
                    {
                        while (!edge.Equals(cube.GetBlock(2, 0, 1)))
                            cube.Rotate(oriented.bottom, Direction.Clockwise);
                    }
                    else if (oriented.left == Color.Blue)
                    {
                        while (!edge.Equals(cube.GetBlock(1, 0, 2)))
                            cube.Rotate(oriented.bottom, Direction.Clockwise);
                    }
                    else if (oriented.left == Color.Orange)
                    {
                        while (!edge.Equals(cube.GetBlock(0, 0, 1)))
                            cube.Rotate(oriented.bottom, Direction.Clockwise);
                    }
                    return oriented.left;
                    /************************************************************/
                }
                return Color.Hidden;
        }

        private void MiddleLayerEdgeFrontRotation(Orientor oriented)
        {
            // U R Ui Ri Ui Fi U F ::
            // D L Di Li Di Fi D F
            cube.Rotate(oriented.bottom, Direction.Clockwise);
            cube.Rotate(oriented.left, Direction.Clockwise);
            cube.Rotate(oriented.bottom, Direction.CounterClockwise);
            cube.Rotate(oriented.left, Direction.CounterClockwise);
            cube.Rotate(oriented.bottom, Direction.CounterClockwise);
            cube.Rotate(oriented.front, Direction.CounterClockwise);
            cube.Rotate(oriented.bottom, Direction.Clockwise);
            cube.Rotate(oriented.front, Direction.Clockwise);
        }

        private void MiddleLayerEdgeLeftRotation(Orientor oriented)
        {
            // Ui Fi U F U R Ui Ri ::
            // Di Fi D F D L Di Li
            cube.Rotate(oriented.bottom, Direction.CounterClockwise);
            cube.Rotate(oriented.front, Direction.CounterClockwise);
            cube.Rotate(oriented.bottom, Direction.Clockwise);
            cube.Rotate(oriented.front, Direction.Clockwise);
            cube.Rotate(oriented.bottom, Direction.Clockwise);
            cube.Rotate(oriented.left, Direction.Clockwise);
            cube.Rotate(oriented.bottom, Direction.CounterClockwise);
            cube.Rotate(oriented.left, Direction.CounterClockwise);
        }

        public Boolean MiddleLayerSolved()
        {
            Edge green_red = new Edge(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Hidden, Color.Red);
            Edge red_blue = new Edge(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Hidden, Color.Red);
            Edge blue_orange = new Edge(Color.Hidden, Color.Hidden, Color.Hidden, Color.Blue, Color.Orange, Color.Hidden);
            Edge orange_green = new Edge(Color.Hidden, Color.Hidden, Color.Green, Color.Hidden, Color.Orange, Color.Hidden);

            return (green_red.Equals(cube.GetBlock(2, 1, 0)) &&
                    red_blue.Equals(cube.GetBlock(2, 1, 2)) &&
                    blue_orange.Equals(cube.GetBlock(0, 1, 2)) &&
                    orange_green.Equals(cube.GetBlock(0, 1, 0)));
        }

        public void SolveBottomCross(UI gui)
        {
            Edge bottom_front, bottom_right, bottom_left, bottom_back;

            while (!BottomCrossSolved())
            {

                //while cube isn't in correct of two orientations, rotate bottom of cube.
                while (true)
                {
                    //rotate  bottom of cube
                    cube.Rotate(Color.Yellow, Direction.Clockwise);

                    gui.Refresh();

                    //assign edges
                    bottom_front = (Edge)cube.GetBlock(1, 0, 0);
                    bottom_right = (Edge)cube.GetBlock(2, 0, 1);
                    bottom_back = (Edge)cube.GetBlock(1, 0, 2);
                    bottom_left = (Edge)cube.GetBlock(0, 0, 1);

                    if ((bottom_front.front == Color.Yellow) && (bottom_left.left == Color.Yellow))
                        break;
                    else if ((bottom_front.front == Color.Yellow) && (bottom_left.bottom == Color.Yellow) && (bottom_right.bottom == Color.Yellow))
                        break;
                }

                while (!BottomCrossSolved())
                {
                    BottomCrossRotation();

                    gui.Refresh();
                }

            }
        }

        private Boolean BottomCrossSolved()
        {
            Edge bottom_front = (Edge)cube.GetBlock(1, 0, 0);
            Edge bottom_right = (Edge)cube.GetBlock(2, 0, 1);
            Edge bottom_back = (Edge)cube.GetBlock(1, 0, 2);
            Edge bottom_left = (Edge)cube.GetBlock(0, 0, 1);

            if (bottom_right.bottom == Color.Yellow &&
                bottom_left.bottom == Color.Yellow &&
                bottom_front.bottom == Color.Yellow &&
                bottom_back.bottom == Color.Yellow)
                return true;
            else
                return false;
        }

        private void BottomCrossRotation()
        {
            // F L D L' D' F'  (oriented with white on top)
            cube.Rotate(Color.Green, Direction.Clockwise);
            cube.Rotate(Color.Orange, Direction.Clockwise);
            cube.Rotate(Color.Yellow, Direction.Clockwise);
            cube.Rotate(Color.Orange, Direction.CounterClockwise);
            cube.Rotate(Color.Yellow, Direction.CounterClockwise);
            cube.Rotate(Color.Green, Direction.CounterClockwise);
        }

        public void SolveBottomEdges(UI gui)
        {
            Edge bottom_front, bottom_right, bottom_left, bottom_back;

            while (!BottomEdgesSolved())
            {
                //assign edges
                bottom_front = (Edge)cube.GetBlock(1, 0, 0);
                bottom_right = (Edge)cube.GetBlock(2, 0, 1);
                bottom_back = (Edge)cube.GetBlock(1, 0, 2);
                bottom_left = (Edge)cube.GetBlock(0, 0, 1);

                //turn bottom layer until the green yellow edge piece is solved.
                while (cube.GetBlock(1, 0, 0).front != Color.Green)
                {
                    cube.Rotate(Color.Yellow, Direction.CounterClockwise);
                    gui.Refresh();
                }

                //while the Orange Yellow edge isn't in the right place, do the algorithm
                while (cube.GetBlock(0, 0, 1).left != Color.Orange)
                {
                    SolveBottomEdgesRotation(Color.Green);
                    gui.Refresh();
                }

                //if the red yellow edge isn't solved, then do the algorithm once, followed by a single rotation to orient the edges correctly.
                if (cube.GetBlock(2, 0, 1).right != Color.Red)
                {
                    SolveBottomEdgesRotation(Color.Red);
                    gui.Refresh();
                    cube.Rotate(Color.Yellow, Direction.Clockwise);
                    gui.Refresh();
                }
            }
        }

        private Boolean BottomEdgesSolved()
        {
            Edge gy_edge_corr_orient = new Edge(Color.Hidden,Color.Yellow,Color.Green,Color.Hidden,Color.Hidden,Color.Hidden);
            Edge ry_edge_corr_orient = new Edge(Color.Hidden,Color.Yellow,Color.Hidden,Color.Hidden,Color.Hidden,Color.Red);
            Edge by_edge_corr_orient = new Edge(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Hidden, Color.Hidden);
            Edge oy_edge_corr_orient = new Edge(Color.Hidden, Color.Yellow, Color.Hidden, Color.Hidden, Color.Orange, Color.Hidden);

            Edge front = (Edge)cube.GetBlock(1, 0, 0);
            Edge right = (Edge)cube.GetBlock(2, 0, 1);
            Edge back = (Edge)cube.GetBlock(1, 0, 2);
            Edge left = (Edge)cube.GetBlock(0, 0, 1);

            if (front.Equals(gy_edge_corr_orient) &&
                right.Equals(ry_edge_corr_orient) &&
                back.Equals(by_edge_corr_orient) &&
                left.Equals(oy_edge_corr_orient))
                return true;
            else
                return false;
        }

        private void SolveBottomEdgesRotation(Color front_face_color)
        {
            Orientor front_face = Cube.GetOrientation(front_face_color);

            // R U R' U R U U R'
            // L B L' B L B B L'
            cube.Rotate(front_face.left, Direction.Clockwise);
            cube.Rotate(front_face.bottom, Direction.Clockwise);
            cube.Rotate(front_face.left, Direction.CounterClockwise);
            cube.Rotate(front_face.bottom, Direction.Clockwise);
            cube.Rotate(front_face.left, Direction.Clockwise);
            cube.Rotate(front_face.bottom, Direction.Clockwise);
            cube.Rotate(front_face.bottom, Direction.Clockwise);
            cube.Rotate(front_face.left, Direction.CounterClockwise);
        }

        public void SolveBottomCorners(UI gui)
        {
            Corner gyo_corner, gyr_corner, ryb_corner, byo_corner;
            Color front_face_color;

            while (!BottomCornersPositionedCorrectly())
            {
                gyo_corner = (Corner) cube.GetBlock(Color.Green, false, cube.GetBlock(Color.Yellow, false, cube.GetBlock(Color.Orange, false, cube.blocks)))[0];
                gyr_corner = (Corner) cube.GetBlock(Color.Green, false, cube.GetBlock(Color.Yellow, false, cube.GetBlock(Color.Red, false, cube.blocks)))[0];
                ryb_corner = (Corner) cube.GetBlock(Color.Red, false, cube.GetBlock(Color.Yellow, false, cube.GetBlock(Color.Blue, false, cube.blocks)))[0];
                byo_corner = (Corner) cube.GetBlock(Color.Blue, false, cube.GetBlock(Color.Yellow, false, cube.GetBlock(Color.Orange, false, cube.blocks)))[0];

                if(cube.GetBlock(0,0,0).Equals(gyo_corner))
                        front_face_color = Color.Green;

                else if(cube.GetBlock(2,0,0).Equals(gyr_corner))
                        front_face_color = Color.Red;

                else if(cube.GetBlock(2,0,2).Equals(ryb_corner))
                    front_face_color = Color.Blue;

                else if (cube.GetBlock(0, 0, 2).Equals(byo_corner))
                    front_face_color = Color.Orange;
                else
                {
                    SolveBottomCornersRotation(Color.Green);
                    continue;
                }
                for (int i = 0; i < 5; i++)
                {
                    if (!BottomCornersPositionedCorrectly())
                        SolveBottomCornersRotation(front_face_color);
                }
            }
        }

        private Boolean BottomCornersPositionedCorrectly()
        {
            if ((cube.GetBlock(0, 0, 0).HasColor(Color.Green) && cube.GetBlock(0, 0, 0).HasColor(Color.Yellow) && cube.GetBlock(0, 0, 0).HasColor(Color.Orange)) &&  //gyo_corner
                (cube.GetBlock(2, 0, 0).HasColor(Color.Green) && cube.GetBlock(2, 0, 0).HasColor(Color.Yellow) && cube.GetBlock(2, 0, 0).HasColor(Color.Red)) &&     //gyr_corner
                (cube.GetBlock(2, 0, 2).HasColor(Color.Red) && cube.GetBlock(2, 0, 2).HasColor(Color.Yellow) && cube.GetBlock(2, 0, 2).HasColor(Color.Blue)) &&     //ryb_corner
                (cube.GetBlock(0, 0, 2).HasColor(Color.Blue) && cube.GetBlock(0, 0, 2).HasColor(Color.Yellow) && cube.GetBlock(0, 0, 2).HasColor(Color.Orange)))
                return true;
            else
                return false;
        }

        private void SolveBottomCornersRotation(Color front_face_color)
        {
            Orientor front_face = Cube.GetOrientation(front_face_color);

            //U R U' L' U R' U' L ::
            //D L D' R' D L' D' R

            cube.Rotate(front_face.bottom, Direction.Clockwise);
            cube.Rotate(front_face.left, Direction.Clockwise);
            cube.Rotate(front_face.bottom, Direction.CounterClockwise);
            cube.Rotate(front_face.right, Direction.CounterClockwise);
            cube.Rotate(front_face.bottom, Direction.Clockwise);
            cube.Rotate(front_face.left, Direction.CounterClockwise);
            cube.Rotate(front_face.bottom, Direction.CounterClockwise);
            cube.Rotate(front_face.right, Direction.Clockwise);
        }

        public void SolveBottomCornerOrientation(UI gui)
        {
            while (!BottomCornersOriented())
            {
                while (cube.GetBlock(0, 0, 0).bottom == Color.Yellow)
                {
                    cube.Rotate(Color.Yellow, Direction.Clockwise);
                    gui.Refresh();
                }

                while (cube.GetBlock(0, 0, 0).bottom != Color.Yellow)
                {
                    CornersRotationFinal();
                    gui.Refresh();
                }
                
                //JUST ONE MORE MOVE!!!
                if(cube.GetBlock(0,0,0).bottom == Color.Yellow &&
                    cube.GetBlock(2,0,0).bottom == Color.Yellow &&
                    cube.GetBlock(2,0,2).bottom == Color.Yellow &&
                    cube.GetBlock(0,0,2).bottom == Color.Yellow)
                    cube.Rotate(Color.Yellow, Direction.Clockwise);
            }
        }

        private Boolean BottomCornersOriented()
        {
            Corner gyo_corner_corr_orient = new Corner(Color.Hidden, Color.Yellow, Color.Green, Color.Hidden, Color.Orange, Color.Hidden);
            Corner gyr_corner_corr_orient = new Corner(Color.Hidden, Color.Yellow, Color.Green, Color.Hidden, Color.Hidden, Color.Red);
            Corner ryb_corner_corr_orient = new Corner(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Hidden, Color.Red);
            Corner byo_corner_corr_orient = new Corner(Color.Hidden, Color.Yellow, Color.Hidden, Color.Blue, Color.Orange, Color.Hidden);

            if (cube.GetBlock(0, 0, 0).Equals(gyo_corner_corr_orient) && cube.GetBlock(2, 0, 0).Equals(gyr_corner_corr_orient) &&
                cube.GetBlock(2, 0, 2).Equals(ryb_corner_corr_orient) && cube.GetBlock(0, 0, 2).Equals(byo_corner_corr_orient))
                return true;
            else
                return false;
        }

        private void CornersRotationFinal()
        {
            Orientor front_face = Cube.GetOrientation(Color.Green);

            //Ri Di R D
            cube.Rotate(front_face.left, Direction.CounterClockwise);
            cube.Rotate(front_face.top, Direction.CounterClockwise);
            cube.Rotate(front_face.left, Direction.Clockwise);
            cube.Rotate(front_face.top, Direction.Clockwise);
        }
    }
}
