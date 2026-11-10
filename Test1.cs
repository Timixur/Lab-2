using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MatrixLibrary;

namespace MatrixTests
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void Constructor_ValidDimensions_CreatesZeroMatrix()
        {
            Matrix m = new Matrix(2, 3);
            Assert.AreEqual(2, m.Rows);
            Assert.AreEqual(3, m.Cols);
            Assert.AreEqual(0.0, m[0, 0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NegativeDimensions_ThrowsException()
        {
            Matrix m = new Matrix(-2, 3);
        }

        [TestMethod]
        public void Indexer_ValidIndices_SetsAndGetsValue()
        {
            Matrix m = new Matrix(2, 2);
            m[1, 1] = 5.5;
            Assert.AreEqual(5.5, m[1, 1]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Indexer_OutOfBounds_ThrowsException()
        {
            Matrix m = new Matrix(2, 2);
            var x = m[2, 0];
        }

        [TestMethod]
        public void Add_SameSize_ReturnsCorrectSum()
        {
            Matrix m1 = new double[,] { { 1, 2 }, { 3, 4 } };
            Matrix m2 = new double[,] { { 5, 6 }, { 7, 8 } };
            Matrix expected = new double[,] { { 6, 8 }, { 10, 12 } };

            Matrix actual = m1 + m2;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_DifferentSizes_ThrowsException()
        {
            Matrix m1 = new double[,] { { 1, 2 } };
            Matrix m2 = new double[,] { { 1 }, { 2 } };
            var res = m1 + m2;
        }

        [TestMethod]
        public void Multiply_CompatibleMatrices_ReturnsCorrectProduct()
        {
            Matrix m1 = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } }; // 2x3
            Matrix m2 = new double[,] { { 7, 8 }, { 9, 1 }, { 2, 3 } }; // 3x2
            Matrix expected = new double[,] { { 31, 19 }, { 85, 55 } }; // 2x2

            Matrix actual = m1 * m2;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Multiply_IncompatibleSizes_ThrowsException()
        {
            Matrix m1 = new double[,] { { 1, 2 } };
            Matrix m2 = new double[,] { { 1, 2 } };
            var res = m1 * m2;
        }

        [TestMethod]
        public void Multiply_Scalar_MultipliesAllElements()
        {
            Matrix m = new double[,] { { 2, -3 }, { 4, 0 } };
            Matrix expected = new double[,] { { 4, -6 }, { 8, 0 } };

            Matrix actual = m * 2.0;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Transpose_OperatorNot_TransposesMatrix()
        {
            Matrix m = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } }; // 2x3
            Matrix expected = new double[,] { { 1, 4 }, { 2, 5 }, { 3, 6 } }; // 3x2

            Matrix actual = !m;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OperatorTrueFalse_NonZero_IsTrue()
        {
            Matrix m = new double[,] { { 0, 0 }, { 0, 1 } };
            Assert.IsTrue(m ? true : false);
        }

        [TestMethod]
        public void OperatorTrueFalse_AllZeros_IsFalse()
        {
            Matrix m = new Matrix(2, 2);
            Assert.IsFalse(m ? true : false);
        }

        [TestMethod]
        public void ExplicitCast_ToDouble_ReturnsFrobeniusNorm()
        {
            Matrix m = new double[,] { { 3, 0 }, { 0, 4 } };
            double norm = (double)m;
            Assert.AreEqual(5.0, norm, 1e-4);
        }

        [TestMethod]
        public void OperatorComparison_ComparesByNorm()
        {
            Matrix smaller = new double[,] { { 1, 0 }, { 0, 1 } }; // norm = sqrt(2)
            Matrix bigger = new double[,] { { 2, 0 }, { 0, 2 } };  // norm = sqrt(8)

            Assert.IsTrue(smaller < bigger);
            Assert.IsTrue(bigger > smaller);
        }
    }
}