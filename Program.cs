using MyMath;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            double[,] arr = new double[3, 3];
            arr[0, 0] = 1;
            arr[0, 1] = 2;
            arr[0, 2] = 5;
            arr[1, 0] = 2;
            arr[1, 1] = 3;
            arr[1, 2] = 7;
            arr[2, 0] = 1;
            arr[2, 1] = 5;
            arr[2, 2] = 6;

            Matrix mat = new Matrix(arr);

            mat = mat.adj() / mat.Det() * mat;
            mat.PrintMat();
        }
    }
}

namespace MyMath
{
    class funcs
    {
        public static double sgn(int i)
        {
            if (i % 2 == 0) return 1;
            return -1;
        }
    }

    class Matrix
    {
        public double[,] mat;

        public double this[int index1, int index2]
        {
            get
            {
                if (index1 < 0 ||
                    index1 >= mat.GetLength(0) ||
                    index2 < 0 ||
                    index2 >= mat.GetLength(1))
                {
                    throw new IndexOutOfRangeException();
                }
                return mat[index1, index2];
            }
            set
            {
                if (index1 < 0 ||
                    index1 >= mat.GetLength(0) ||
                    index2 < 0 ||
                    index2 >= mat.GetLength(1))
                {
                    throw new IndexOutOfRangeException();
                }
                mat[index1, index2] = value;
            }
        }

        public Matrix(int x)
        {
            mat = new double[x, x];
        }

        public Matrix(int x, int y)
        {
            mat = new double[x, y];
        }

        public Matrix(double[,] mat)
        {
            this.mat = mat;
        }

        public static Matrix operator +(Matrix m, Matrix n)
        {
            double[,] mat = new double[m.mat.GetLength(0), m.mat.GetLength(0)];

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = m.mat[i, j] + n.mat[i, j];
                }
            }

            return new Matrix(mat);
        }

        public static Matrix operator -(Matrix m, Matrix n)
        {
            double[,] mat = new double[m.mat.GetLength(0), m.mat.GetLength(0)];

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = m.mat[i, j] - n.mat[i, j];
                }
            }

            return new Matrix(mat);
        }

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            for (int i = 0; i < m1.mat.GetLength(0); i++)
            {
                for (int j = 0; j < m1.mat.GetLength(1); j++)
                {
                    if (m1.mat[i, j] != m2.mat[i, j]) return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            for (int i = 0; i < m1.mat.GetLength(0); i++)
            {
                for (int j = 0; j < m1.mat.GetLength(1); j++)
                {
                    if (m1.mat[i, j] != m2.mat[i, j]) return true;
                }
            }

            return false;
        }

        public static Matrix operator ~(Matrix m) // transposed matrix
        {
            double[,] mat = new double[m.mat.GetLength(0), m.mat.GetLength(1)];

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = m.mat[j, i];
                }
            }
            
            return new Matrix(mat);
        }

        public static Matrix operator *(double x, Matrix m)
        {
            for (int i = 0; i < m.mat.GetLength(0); i++)
            {
                for (int j = 0; j < m.mat.GetLength(1); j++)
                {
                    m.mat[i, j] *= x;
                }
            }

            return m;
        }

        public static Matrix operator *(Matrix m, double x)
        {
            for (int i = 0; i < m.mat.GetLength(0); i++)
            {
                for (int j = 0; j < m.mat.GetLength(1); j++)
                {
                    m.mat[i, j] *= x;
                }
            }

            return m;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            double[,] matrix = new double[m1.mat.GetLength(0), m2.mat.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    double sum = 0;
                    for (int n = 0; n < m1.mat.GetLength(1); n++)
                    {
                        sum += m1.mat[i, n] * m2.mat[n, j];
                    }

                    matrix[i, j] = sum;
                }
            }

            return new Matrix(matrix);
        }

        public static Matrix operator /(Matrix m1, double x)
        {
            return m1 * (1 / x);
        }

        public double[] Vec()
        {
            double[] vector = new double[mat.Length];
            int cnt = 0;
            
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    vector[cnt] = mat[i, j];
                    cnt++;
                }
            }

            return vector;
        }

        public void PrintMat()
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    Console.Write(mat[i, j]+ " ");
                }
                Console.WriteLine();
            }
        }

        public double[] ArgMax(int axis = 0)
        {
            double[] arr = new double[mat.GetLength(1 - axis)];

            for (int i = 0; i < mat.GetLength(1 - axis); i++)
            {
                double max = mat[i, 0];

                for (int j = 0; j < mat.GetLength(axis); j++)
                {
                    if (max < mat[i, j]) max = mat[i, j];
                }

                arr[i] = max;
            }

            return arr;
        }

        private double[,] Minor(double[,] matrix, int i, int j)
        {
            int n = matrix.GetLength(0), k = 0;
            double[,] arr = new double[n - 1, n - 1];

            for (int a = 0; a < n - 1; a++)
            {
                int c = 0;
                for (int b = 0; b < n - 1; b++)
                {
                    if (b == j) c = 1;
                    if (a == i) k = 1;

                    arr[a, b] = matrix[a + k, b + c];
                }
            }

            return arr;
        }

        public Matrix Minor(int i, int j)
        {
            return new Matrix(Minor(mat, i, j));
        }

        private double DetExe(double[,] matrix)
        {
            if (matrix.GetLength(0) == 0) return 1;
            else if (matrix.GetLength(0) == 1) return matrix[0, 0];

            int n = matrix.GetLength(0);
            double sum = 0;

            for (int i = 0; i < n; i++)
            {
                double[,] arr = Minor(matrix, 0, i);
                sum += funcs.sgn(i) * matrix[0, i] * DetExe(arr);
            }

            return sum;
        }

        private double DetExe(Matrix matrix)
        {
            return DetExe(matrix.mat);
        }

        public double Det()
        {
            return DetExe(mat);
        }

        public Matrix adj()
        {
            int n = mat.GetLength(0);
            double[,] C = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = funcs.sgn(i + j) * DetExe(Minor(mat, i, j));
                }
            }

            Matrix matrix = new Matrix(C);
            return ~matrix;
        }
    }
}