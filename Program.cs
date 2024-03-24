using System;
using System.Collections.Generic;
using static EventsDelegates.SquareMatrix;

namespace EventsDelegates
{
    public class SquareMatrix : IComparable<SquareMatrix>
    {
        public delegate SquareMatrix DiagonalizeMatrixDelegate(SquareMatrix matrix);
        public delegate SquareMatrix MatrixOperationDelegate(SquareMatrix firstMatrix, SquareMatrix secondMatrix);
        public delegate bool MatrixComparisonDelegate(SquareMatrix firstMatrix, SquareMatrix secondMatrix);
        public int Extension
        {
            get
            {
                return (int)Math.Sqrt(MatrixArray.Length);
            }
        }
        public int[,] MatrixArray { get; set; }
        private int hash;
        public int Hash
        {
            get
            {
                hash = MatrixArray[0, 0];

                for (int rowIndex = 0; rowIndex < Extension; ++rowIndex)
                {
                    for (int columnIndex = 0; columnIndex < Extension; ++columnIndex)
                    {
                        hash = hash * MatrixArray[rowIndex, columnIndex] + hash % MatrixArray[rowIndex, columnIndex];
                    }
                }

                return hash;
            }
        }

        public SquareMatrix(int[,] elements)
        {
            MatrixArray = elements;
        }

        public SquareMatrix(int[][] elements)
        {
            MatrixArray = GetMatrixFromArrayOfArrays(elements);
        }

        public SquareMatrix(params int[] elements)
        {
            MatrixArray = GetMatrixFromArray(elements);
        }

        public int[,] GetMatrixFromArray(int[] elements)
        {
            int extension = (int)Math.Sqrt(elements.Length);
            int[,] matrix = new int[extension, extension];
            int elementIndex = 0;

            for (int rowIndex = 0; rowIndex < extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < extension; ++columnIndex)
                {

                    try
                    {
                        matrix[rowIndex, columnIndex] = elements[elementIndex];
                    }
                    catch (System.IndexOutOfRangeException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }

                    ++elementIndex;
                }
            }

            return matrix;
        }

        public int[,] GetMatrixFromArrayOfArrays(int[][] elements)
        {
            int extension = (int)Math.Sqrt(elements.Length);
            int[,] matrix = new int[extension, extension];

            for (int rowIndex = 0; rowIndex < extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < extension; ++columnIndex)
                {
                    matrix[rowIndex, columnIndex] = elements[rowIndex][columnIndex];
                }
            }

            return matrix;
        }

        public void AutoFill(int extension, int minElement = -10, int MaxElement = 10)
        {
            int[] elements = new int[extension * extension];
            var random = new Random();

            for (int elementIndex = 0; elementIndex < extension * extension; ++elementIndex)
            {
                elements[elementIndex] = random.Next(minElement, MaxElement);
            }

            MatrixArray = GetMatrixFromArray(elements);
        }

        public SquareMatrix Clone()
        {
            int[,] elements = new int[this.Extension, this.Extension];

            for (int rowIndex = 0; rowIndex < this.Extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < this.Extension; ++columnIndex)
                {
                    elements[rowIndex, columnIndex] = this.MatrixArray[rowIndex, columnIndex];
                }
            }

            return new SquareMatrix(elements);
        }

        public int SumOfElements()
        {
            int sumOfElements = 0;

            for (int rowIndex = 0; rowIndex < this.Extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < this.Extension; ++columnIndex)
                {
                    sumOfElements += this.MatrixArray[rowIndex, columnIndex];
                }
            }

            return sumOfElements;
        }

        public static SquareMatrix operator +(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (firstMatrix.Extension != secondMatrix.Extension)
            {
                throw new DifferentSizesException("Операцию сложения можно выполнять только с матрицами одинаковой размерности");
            }

            var result = firstMatrix.Clone();
            int extension = firstMatrix.Extension;

            for (int rowIndex = 0; rowIndex < extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < extension; ++columnIndex)
                {
                    result.MatrixArray[rowIndex, columnIndex] += secondMatrix.MatrixArray[rowIndex, columnIndex];
                }
            }

            return result;
        }

        public static SquareMatrix operator -(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (firstMatrix.Extension != secondMatrix.Extension)
            {
                throw new DifferentSizesException("Операцию вычитания можно выполнять только с матрицами одинаковой размерности");
            }

            var result = firstMatrix.Clone();
            int extension = firstMatrix.Extension;

            for (int rowIndex = 0; rowIndex < extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < extension; ++columnIndex)
                {
                    result.MatrixArray[rowIndex, columnIndex] -= secondMatrix.MatrixArray[rowIndex, columnIndex];
                }
            }

            return result;
        }

        public static SquareMatrix operator *(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (firstMatrix.Extension != secondMatrix.Extension)
            {
                throw new DifferentSizesException("Операцию умножения можно выполнять только с матрицами одинаковой размерности");
            }

            var result = firstMatrix.Clone();
            int extension = firstMatrix.Extension;

            for (int rowIndexOfFirstMatrix = 0; rowIndexOfFirstMatrix < firstMatrix.Extension; ++rowIndexOfFirstMatrix)
            {
                for (int columnIndex = 0; columnIndex < extension; ++columnIndex)
                {
                    result.MatrixArray[rowIndexOfFirstMatrix, columnIndex] = 0;

                    for (int indexOfSecondElement = 0; indexOfSecondElement < extension; ++indexOfSecondElement)
                    {
                        result.MatrixArray[rowIndexOfFirstMatrix, columnIndex] += firstMatrix.MatrixArray[rowIndexOfFirstMatrix, indexOfSecondElement] * secondMatrix.MatrixArray[indexOfSecondElement, columnIndex];
                    }
                }
            }

            return result;
        }

        public int CompareTo(SquareMatrix other)
        {
            if (this.Extension == other.Extension)
            {
                if (this.SumOfElements() > other.SumOfElements())
                {
                    return 1;
                } else if (this.SumOfElements() == other.SumOfElements())
                {
                    return 0;
                } else if (this.SumOfElements() < other.SumOfElements())
                {
                    return -1;
                }
            }
            return -1;
        }

        public static bool operator >(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            return firstMatrix.CompareTo(secondMatrix) > 0;
        }

        public static bool operator <(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            return firstMatrix.CompareTo(secondMatrix) < 0;
        }

        public static bool operator >=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            return firstMatrix.CompareTo(secondMatrix) >= 0;
        }

        public static bool operator <=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            return firstMatrix.CompareTo(secondMatrix) <= 0;
        }

        public override bool Equals(object other)
        {
            var second = other as SquareMatrix;
            return this.SumOfElements() == second.SumOfElements();
        }

        public static bool operator ==(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            return Object.Equals(firstMatrix, secondMatrix);
        }

        public static bool operator !=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            return !Object.Equals(firstMatrix, secondMatrix);
        }

        public static bool operator false(SquareMatrix matrix)
        {
            return matrix.SumOfElements() == 0;
        }

        public static bool operator true(SquareMatrix matrix)
        {
            return matrix.SumOfElements() == 1;
        }

        public override int GetHashCode()
        {
            return this.Hash;
        }

        public override string ToString()
        {
            string matrixString = "";

            for (int rowIndex = 0; rowIndex < this.Extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < this.Extension; ++columnIndex)
                {
                    if (MatrixArray[rowIndex, columnIndex] < 0 || MatrixArray[rowIndex, columnIndex] > 9)
                    {
                        matrixString += " " + MatrixArray[rowIndex, columnIndex];
                    } else
                    {
                        matrixString += "  " + MatrixArray[rowIndex, columnIndex];
                    }
                }
                matrixString += "\n";
            }

            return matrixString;
        }

        public static implicit operator SquareMatrix(int[] elements)
        {
            return new SquareMatrix(elements);
        }

        public static explicit operator string(SquareMatrix matrix)
        {
            return matrix.ToString();
        }

        public static explicit operator int[,](SquareMatrix matrix)
        {
            return matrix.MatrixArray;
        }

        public static int CalculateDeterminant(SquareMatrix matrix)
        {
            if (matrix.Extension == 1)
            {
                return matrix.MatrixArray[0, 0];
            }

            int determinant = 0;

            for (int columnIndex = 0; columnIndex < matrix.Extension; ++columnIndex)
            {
                int minorDeterminant = matrix.MatrixArray[0, columnIndex] * CalculateDeterminant(CalculateMinor(matrix.MatrixArray, 0, columnIndex));
                determinant += (columnIndex % 2 == 0) ? minorDeterminant : -minorDeterminant;
            }

            return determinant;
        }

        public static SquareMatrix CalculateMinor(int[,] matrix, int excludedRow, int excludedColumn)
        {
            int[][] minor = new int[matrix.Length - 1][];
            int minorIndex = 0;

            for (int rowIndex = 0; rowIndex < matrix.Length; ++rowIndex)
            {
                if (rowIndex == excludedRow)
                    continue;

                List<int> minorRow = new List<int>();

                for (int columnIndex = 0; columnIndex < matrix.Length; ++columnIndex)
                {
                    if (columnIndex == excludedColumn)
                        continue;

                    minorRow.Add(matrix[rowIndex, columnIndex]);
                }

                minor[minorIndex++] = minorRow.ToArray();
            }

            return new SquareMatrix(minor);
        }

        public void Diagonalize(SquareMatrix matrix, DiagonalizeMatrixDelegate diagonalizeMethod)
        {
            diagonalizeMethod(matrix);
        }

        public static DiagonalizeMatrixDelegate diagonalize = delegate (SquareMatrix matrix)
        {
            var result = matrix.Clone();
            for (int rowIndex = 0; rowIndex < matrix.Extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix.Extension; ++columnIndex)
                {
                    if (rowIndex != columnIndex)
                    {
                        result.MatrixArray[rowIndex, columnIndex] = 0;
                    }
                }
            }

            return result;
        };
    }

    public static class MatrixExtensions
    {
        public static SquareMatrix CalculateTransposeMatrix(this SquareMatrix matrix)
        {
            int[,] result = new int[matrix.Extension, matrix.Extension];

            for (int rowIndex = 0; rowIndex < matrix.Extension; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < matrix.Extension; ++columnIndex)
                {
                    result[rowIndex, columnIndex] = matrix.MatrixArray[columnIndex, rowIndex];
                }
            }

            return new SquareMatrix(result);
        }

        public static int CalculateMatrixTrace(this SquareMatrix matrix)
        {
            int trace = 0;

            for (int diagonalElementIndex = 0; diagonalElementIndex < matrix.Extension; ++diagonalElementIndex)
            {
                trace += matrix.MatrixArray[diagonalElementIndex, diagonalElementIndex];
            }

            return trace;
        }
    }

    class DifferentSizesException : System.Exception
    {
        public DifferentSizesException() : base() { }
        public DifferentSizesException(string message) : base(message) { }
        public DifferentSizesException(string message, System.Exception inner) : base(message, inner) { }
    }

    public abstract class BaseMenuHandler
    {
        public BaseMenuHandler NextHandler { get; set; }

        public virtual void SetNextHandler(BaseMenuHandler nextHandler)
        {
            NextHandler = nextHandler;
        }

        public abstract void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix);
    }

    public class AdditionMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("+"))
            {
                Console.WriteLine("Результат операции сложения:");
                SquareMatrix sumOfMatrix = firstMatrix + secondMatrix;
                Console.WriteLine(sumOfMatrix.ToString());
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }
    public class SubtractionMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("-"))
            {
                Console.WriteLine("Результат операции вычитания:");
                SquareMatrix multiplicationOfMatrix = firstMatrix - secondMatrix;
                Console.WriteLine(multiplicationOfMatrix.ToString());
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class MultiplicationMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("*"))
            {
                Console.WriteLine("Результат операции умножения:");
                SquareMatrix multiplicationOfMatrix = firstMatrix * secondMatrix;
                Console.WriteLine(multiplicationOfMatrix.ToString());
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class LessThanMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("<"))
            {
                Console.WriteLine($"Матрица 1 < Матрица 2: {firstMatrix < secondMatrix}");
                Console.WriteLine();
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class LessThanOrEqualMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("<="))
            {
                Console.WriteLine($"Матрица 1 <= Матрица 2: {firstMatrix <= secondMatrix}");
                Console.WriteLine();
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class GreaterThanMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains(">"))
            {
                Console.WriteLine($"Матрица 1 > Матрица 2: {firstMatrix > secondMatrix}");
                Console.WriteLine();
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class GreaterThanOrEqualMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains(">="))
            {
                Console.WriteLine($"Матрица 1 >= Матрица 2: {firstMatrix >= secondMatrix}");
                Console.WriteLine();
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }
    public class EqualityMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("=="))
            {
                Console.WriteLine($"Матрица 1 == Матрица 2: {firstMatrix == secondMatrix}");
                Console.WriteLine();
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class InequalityMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("!="))
            {
                Console.WriteLine($"Матрица 1 != Матрица 2: {firstMatrix != secondMatrix}");
                Console.WriteLine();
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class TransposeMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("transpose"))
            {
                Console.WriteLine("Транспонированная матрица 1:");
                Console.WriteLine(firstMatrix.CalculateTransposeMatrix().ToString());
                Console.WriteLine("Транспонированная матрица 2:");
                Console.WriteLine(secondMatrix.CalculateTransposeMatrix().ToString());
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class DeterminantMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("determinant"))
            {
                Console.Write("Детерминант матрицы 1: ");
                Console.WriteLine(CalculateDeterminant(firstMatrix));
                Console.Write("Детерминант матрицы 2: ");
                Console.WriteLine(CalculateDeterminant(secondMatrix));
                Console.WriteLine();
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class TraceMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("trace"))
            {
                Console.Write("След матрицы 1: ");
                Console.WriteLine(firstMatrix.CalculateMatrixTrace());
                Console.Write("След матрицы 2: ");
                Console.WriteLine(secondMatrix.CalculateMatrixTrace());
                Console.WriteLine();
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class DiagonalizeMatrixHandler : BaseMenuHandler
    {
        public override void Handle(string userInput, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
        {
            if (userInput.Contains("diagonalize"))
            {
                Console.WriteLine("Матрица 1, приведенная к диагольному виду:");
                Console.WriteLine(diagonalize(firstMatrix));
                Console.WriteLine("Матрица 1, приведенная к диагольному виду:");
                Console.WriteLine(diagonalize(secondMatrix));
            }
            NextHandler?.Handle(userInput, firstMatrix, secondMatrix);
        }
    }

    public class MenuApplication
    {
        public static bool IsWorking { get; set; }
        private BaseMenuHandler menuHandler;

        public MenuApplication(BaseMenuHandler menuHandler)
        {
            IsWorking = true;
            this.menuHandler = menuHandler;
        }

        public void Run()
        {
            while (IsWorking)
            {
                Console.Write("Введите размерность для первой матрицы (одно число - матрица квадратная): ");
                int firstMatrixExtension = Convert.ToInt32(Console.ReadLine());
                SquareMatrix firstMatrix = new SquareMatrix(GetMatrixElements(firstMatrixExtension));
                Console.WriteLine("Матрица 1 создана:");
                Console.Write(firstMatrix.ToString());
                Console.Write("Введите размерность для второй матрицы (такую же, как и у первой, чтобы производить операции с данными матрицами): ");
                int secondMatrixExtension = Convert.ToInt32(Console.ReadLine());
                SquareMatrix secondMatrix = new SquareMatrix(GetMatrixElements(secondMatrixExtension));
                Console.WriteLine("Матрица 2 создана:");
                Console.Write(secondMatrix.ToString());

                Console.Write("Доступные операции: +, -, *, >, >=, <, <=, ==, !=, transpose, trace, diagonalize\nВведите операции через пробел: ");
                string userInput = Console.ReadLine();
                menuHandler.Handle(userInput, firstMatrix, secondMatrix);

                Console.Write("Продолжить выполнение программы? ");
                userInput = Console.ReadLine();
                if (userInput.ToLower() == "нет")
                {
                    IsWorking = false;
                }
            }
        }

        static int[] GetMatrixElements(int extesion)
        {
            int[] elements = new int[extesion * extesion];
            for (int elementIndex = 0; elementIndex < extesion * extesion; ++elementIndex)
            {
                Console.Write($"Введите {elementIndex + 1}-й элемент матрицы: ");
                elements[elementIndex] = Convert.ToInt32(Console.ReadLine());
            }
            return elements;
        }
    }

    class Programm
    {
        static void Main(string[] args)
        {
            BaseMenuHandler additionHandler = new AdditionMatrixHandler();
            BaseMenuHandler subtractionHandler = new SubtractionMatrixHandler();
            BaseMenuHandler multiplicationHandler = new MultiplicationMatrixHandler();
            BaseMenuHandler lessThanHandler = new LessThanMatrixHandler();
            BaseMenuHandler lessThanOrEqualHandler = new LessThanOrEqualMatrixHandler();
            BaseMenuHandler greaterThanHandler = new GreaterThanMatrixHandler();
            BaseMenuHandler greaterThanOrEqualHandler = new GreaterThanOrEqualMatrixHandler();
            BaseMenuHandler equalityHandler = new EqualityMatrixHandler();
            BaseMenuHandler inequalityHandler = new InequalityMatrixHandler();
            BaseMenuHandler transposeHandler = new TransposeMatrixHandler();
            BaseMenuHandler determinantHandler = new DeterminantMatrixHandler();
            BaseMenuHandler traceHandler = new TraceMatrixHandler();
            BaseMenuHandler diagonalizeHandler = new DiagonalizeMatrixHandler();

            additionHandler.SetNextHandler(subtractionHandler);
            subtractionHandler.SetNextHandler(multiplicationHandler);
            multiplicationHandler.SetNextHandler(lessThanHandler);
            lessThanHandler.SetNextHandler(lessThanOrEqualHandler);
            lessThanOrEqualHandler.SetNextHandler(greaterThanHandler);
            greaterThanHandler.SetNextHandler(greaterThanOrEqualHandler);
            greaterThanOrEqualHandler.SetNextHandler(equalityHandler);
            equalityHandler.SetNextHandler(inequalityHandler);
            inequalityHandler.SetNextHandler(transposeHandler);
            transposeHandler.SetNextHandler(determinantHandler);
            determinantHandler.SetNextHandler(traceHandler);
            traceHandler.SetNextHandler(diagonalizeHandler);

            MenuApplication menuApp = new MenuApplication(additionHandler);
            menuApp.Run();

            Console.WriteLine("Выполнение программы завершено,\nНажмите любую клавишу, чтобы закрыть это окно");
            // Ожидание нажатия клавиши (чтобы окно не закрывалось сразу после выполнения программы)
            Console.ReadKey();
        }
    }
}
