using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using helpers;

namespace cli
{
    public class TaskFivePreProcess
    {
        MatrixData averageFace;
        public TaskFivePreProcess()
        {
            averageFace = new MatrixData("Data\\MeanFace.csv",false,true,',');
            averageFace = averageFace[0].GetReShapedMatrix(19);
            averageFace.UpScale(16).ToImage("AverageFace.bmp");
        }
        int[,] sharpenFilter = new int[,]{
                {-1,-2,-1},
                {-2,16,-2},
                {-1,-2,-1}
            };
        int[,] sobelX = new int[,]{
                {-1,0,1},
                {-1,0,1},
                {-1,0,1}
            };
        int[,] sobelY = new int[,]{
                { 1, 1, 1},
                { 0, 0, 0},
                {-1,-1,-1}
            };
        int[,] blurFilter = new int[,]{
                {1,2,1},
                {2,4,2},
                {1,2,1}
            };
        int[,] embossfilter = new int[,]{
                {0,0,0},
                {1,0,-1},
                {0,0,0}
            };
        int[,] embossfilter1 = new int[,]{
                {1,0,0},
                {0,0,0},
                {0,0,-1}
            };
        int[,] embossfilter2 = new int[,]{
                {0,1,0},
                {0,0,0},
                {0,-1,0}
            };
        int[,] embossfilter3 = new int[,]{
                {0,0,1},
                {0,0,0},
                {-1,0,0}
            };

        public void ProcessFile(string inputFileName, string outputFileName, int ProcessNo)
        {
            //Read the input file
            MatrixData m = new MatrixData(inputFileName, false, true, ',');

            //Initialise the output list
            List<dynamic[]> outputList = new List<dynamic[]>();
            //Process everyfile and add it to the output
            Parallel.ForEach(SteppedIntegerList(0, m.NumberOfRows, 1), r =>
            {
                System.Console.WriteLine(inputFileName + " ("+outputList.Count+"//"+m.NumberOfRows+")");
                switch (ProcessNo)
                {
                    case 1:
                        outputList.Add(ProcessImage1(m[r]));
                    break;
                    case 2:
                        outputList.Add(ProcessImage2(m[r]));
                    break;
                    case 3:
                        outputList.Add(ProcessImage3(m[r]));
                    break;
                    case 4:
                        outputList.Add(ProcessImage4(m[r]));
                    break;
                    case 5:
                        //outputList.Add(ProcessImage5(m[r]));
                    break;
                }
                

                // force cleanup managed resources every 100 items
                // this prevents pageing on systems with a low amount of ram
                if(outputList.Count%50 == 1)
                {
                    GC.Collect();
                }
            });
            //Create the output matrix 
            MatrixData outputMatrix = new MatrixData(outputList.ToArray());
            outputMatrix.WriteCSV(outputFileName);

        }
        public dynamic[] ProcessImage1(dynamic[] input)
        {
            //Turn the input vector into a matrix
            //This matrix represents the image
            MatrixData pixelMatrix = input.GetReShapedMatrix(19);

            //Sharpen the image
            pixelMatrix = pixelMatrix.ApplyConvolutionFilter(sharpenFilter);

            //Apply the SobelEdge filter
            pixelMatrix = pixelMatrix.GetSobelEdge();

            //reduce the size of the image to 18x18 by removing
            // a random pixel from each row and column
            pixelMatrix = pixelMatrix.ReduceDimensionByOne(0).ReduceDimensionByOne(1);

            //Blur the image
            pixelMatrix = pixelMatrix.ApplyConvolutionFilter(blurFilter);

            //Reduce the 18x18 image to 6x6 using a custom downsampling algorithm
            pixelMatrix = pixelMatrix.Convert18To6Px();

            //Clamp the output matrix values between 0 and 255 
            pixelMatrix = pixelMatrix.Clamp(0, 255);

            //return the pixel matrix as a vector
            // the size of this vector will be 36 (6x6)
            return pixelMatrix.GetVectorizedMatrix();
        }
        public dynamic[] ProcessImage2(dynamic[] input)
        {
            //Turn the input vector into a matrix
            //This matrix represents the image
            MatrixData pixelMatrix = input.GetReShapedMatrix(19);

            //Get the emboss edge
            pixelMatrix = pixelMatrix.GetEmbossEdge();

            //Apply with hist EQ function
            pixelMatrix = pixelMatrix.HistEQ();

            //Crop 1 pixel off the edge of the image
            //this results in a 17x17 image
            pixelMatrix = pixelMatrix.CropEdges(1);

            //reduce the size of the image to 16x16 by removing
            // a random pixel from each row and column
            pixelMatrix = pixelMatrix.ReduceDimensionByOne(0).ReduceDimensionByOne(1);

            //Blur the image
            pixelMatrix = pixelMatrix.ApplyConvolutionFilter(blurFilter);

            //Reduce the 16x16 image to 8x8 using a custom downsampling algorithm
            pixelMatrix = pixelMatrix.DownScale(2);

            //Clamp the output matrix values between 0 and 255 
            pixelMatrix = pixelMatrix.Clamp(0, 255);

            //return the pixel matrix as a vector
            // the size of this vector will be 36 (6x6)
            return pixelMatrix.GetVectorizedMatrix();
        }
        public dynamic[] ProcessImage3(dynamic[] input)      
        {
            //Turn the input vector into a matrix
            //This matrix represents the image
            MatrixData pixelMatrix = input.GetReShapedMatrix(19);

            //hadamard product
            pixelMatrix = pixelMatrix*averageFace;

            //reduce the size of the image to 18x18 by removing
            // a random pixel from each row and column
            pixelMatrix = pixelMatrix.ReduceDimensionByOne(0).ReduceDimensionByOne(1);

            pixelMatrix = pixelMatrix.HistEQ();

            
            return pixelMatrix.DownScale(2,false).GetVectorizedMatrix();

        }  
        public dynamic[] ProcessImage4(dynamic[] input)      
        {
            //Turn the input vector into a matrix
            //This matrix represents the image
            MatrixData pixelMatrix = input.GetReShapedMatrix(19);
            
            //reduce the size of the image to 18x18 by removing
            // a random pixel from each row and column
            pixelMatrix = pixelMatrix.ReduceDimensionByOne(0).ReduceDimensionByOne(1);


            //Reduce the 18x18 image to 6x6 using a custom downsampling algorithm
            pixelMatrix = pixelMatrix.Convert18To6Px(false);

            return pixelMatrix.GetVectorizedMatrix();

        } 

        public void Run(string fileName)
        {
            MatrixData m = new MatrixData(fileName, false, true, ',');
                        
            for (int i = 0; i <2; i++)
            {
                OutputSteps(m[i].GetReShapedMatrix(19),i.ToString());
            }

            void OutputSteps(MatrixData input, string dir)
            {
                string _dir = "Data\\"+dir+"";

                MatrixData pixelMatrix = input;

                pixelMatrix.Clamp(0,255).UpScale(16).ToImage(_dir+"0.bmp");             

                pixelMatrix = pixelMatrix.ReduceDimensionByOne(0).ReduceDimensionByOne(1);
                pixelMatrix.UpScale(16).Clamp(0,255).ToImage(_dir+"1.bmp");  

                pixelMatrix = pixelMatrix.Convert18To6Px(false);
                pixelMatrix.Clamp(0,255);
                pixelMatrix.UpScale(16).ToImage(_dir+"3.bmp");  
            }
        }
        public void GrayTo255(string fileName)
        {
            MatrixData m = new MatrixData(fileName, true, true, ',') * 255;
            m = m.RoundAll();
            m.WriteCSV(fileName.Replace(".csv", "") + "255" + ".csv", false);
        }
        public static IEnumerable<int> SteppedIntegerList(int startIndex, int endEndex, int stepSize)
        {
            for (int i = startIndex; i < endEndex; i += stepSize)
            {
                yield return i;
            }
        }
    }
}