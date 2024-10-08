using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);
            var tasks = new List<Task>();

            for (long i = 0; i < m1.RowCount; i++)
            {
                var rowElements = GetRowElements(m1, i);
                for (long j = 0; j < m2.ColCount; j++)
                {
                    var rowNumber = i;
                    var columNumber = j;
                    tasks.Add(Task.Run(() =>
                    {
                        var columnElements = GetColumnElements(m2, columNumber);
                        var dotProduct = CalculateDotProduct(rowElements, columnElements);
                        resultMatrix.SetElement(rowNumber, columNumber, dotProduct);
                    }));
                }
            };

            Task.WaitAll(tasks.ToArray());

            return resultMatrix;
        }

        private long[] GetRowElements(IMatrix m, long rowNumber)
        {
            long[] rowElements = new long[m.ColCount];

            for (long i = 0; i < m.ColCount; i++)
            {
                rowElements[i] = m.GetElement(rowNumber, i);
            }

            return rowElements;
        }

        private long[] GetColumnElements(IMatrix m, long columnNumber)
        {
            long[] columnElements = new long[m.RowCount];

            for (long i = 0; i < m.RowCount; i++)
            {
                columnElements[i] = m.GetElement(i, columnNumber);
            }

            return columnElements;
        }
        private long CalculateDotProduct(long[] rowElements, long[] columnElements)
        {
            long sum = 0;
            for (long i = 0; i < rowElements.Length; i++)
            {
                sum += rowElements[i] * columnElements[i];
            }
            return sum;
        }
    }
}
