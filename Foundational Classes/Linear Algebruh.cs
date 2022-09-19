using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJom
{
    class LinearAlgebruh
    {
        static public List<float> MatrixTransform(float[,] Matrix, float[] Point)
        {
            if (Matrix.GetLength(0) != Point.GetLength(0))
            {
                return null;
            }
            else
            {
                List<float> Output = new List<float>( new float[Matrix.GetLength(1)] );
                for (int n = 0; n < Matrix.GetLength(0); n++)
                {
                    for (int m = 0; m < Matrix.GetLength(1); m++)
                    {
                        Matrix[n,m] = Matrix[n,m] * Point[n];
                        Output[m] += Matrix[n, m];
                    }
                }
                return Output;
            }
        }
    }
}
