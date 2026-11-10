using System;
using MatrixLibrary;

namespace MatrixApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Matrix A = new double[,]
            {
                { 1.0, 2.0, 3.0 },
                { 4.0, 5.0, 6.0 }
            };

            Matrix B = new double[,]
            {
                { 7.0, 8.0 },
                { 9.0, 1.0 },
                { 2.0, 3.0 }
            };

            // Display initial matrices
            Console.WriteLine("Matrix A (2x3):");
            Console.WriteLine(A);

            Console.WriteLine("Matrix B (3x2):");
            Console.WriteLine(B);

            // Matrix transposition using overloaded unary operator !
            Matrix At = !A;
            Console.WriteLine("Transposed matrix A (!A) (3x2):");
            Console.WriteLine(At);

            // Matrix addition using overloaded operator +
            Matrix sum = B + At;
            Console.WriteLine("Addition B + (!A):");
            Console.WriteLine(sum);

            // Matrix multiplication using overloaded operator *
            Matrix mult = A * B;
            Console.WriteLine("Multiplication A * B (2x2):");
            Console.WriteLine(mult);

            // Scalar multiplication using overloaded operator *
            Matrix scalarMult = A * 2.5;
            Console.WriteLine("Multiplication A * 2.5:");
            Console.WriteLine(scalarMult);

            // Verify true and false operators in conditional expressions
            Matrix zeroMatrix = new Matrix(2, 2);
            Console.WriteLine("Evaluation in 'if' statements:");
            if (A)
                Console.WriteLine("-> Matrix A contains non-zero elements (true).");
            if (!zeroMatrix)
                Console.WriteLine("-> Matrix zeroMatrix contains only zeros (false).\n");

            // Explicit type conversion to double (computing Frobenius norm) and relational comparison
            double normA = (double)A;
            double normB = (double)B;
            Console.WriteLine($"Norm of matrix A: {normA:F2}");
            Console.WriteLine($"Norm of matrix B: {normB:F2}");

            if (A < B)
                Console.WriteLine("-> Norm of matrix A is less than norm of matrix B (operator <).");
            else
                Console.WriteLine("-> Norm of matrix A is not less than norm of matrix B.");
        }
    }
}