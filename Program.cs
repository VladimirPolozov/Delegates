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
        return matrix.MatrixArray[0, 0];

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

    // Метод, использующий делегат для приведения матрицы к диагональному виду
    public void DiagonalizeMatrix(SquareMatrix matrix, DiagonalizeMatrixDelegate diagonalizeMethod)
    {
      diagonalizeMethod(matrix);
    }

    public static SquareMatrix PerformMatrixOperation(MatrixOperationDelegate operationDelegate, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return operationDelegate(firstMatrix, secondMatrix);
    }

    public static bool PerformMatrixComparisonDelegate(MatrixComparisonDelegate operationDelegate, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return operationDelegate(firstMatrix, secondMatrix);
    }

    public static DiagonalizeMatrixDelegate diagonalize = delegate (SquareMatrix matrix) {
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

  class Programm
  {
    static void Main(string[] args)
    {
      bool isWorking = true;
      bool isCalcMode;
      string userChoise;
      string operationChoose;

      MatrixOperationDelegate additionDelegate = (first, second) => first + second;
      MatrixOperationDelegate subtractionDelegate = (first, second) => first - second;
      MatrixOperationDelegate multiplicationDelegate = (first, second) => first * second;
      MatrixComparisonDelegate lessThanDelegate = (first, second) => first < second;
      MatrixComparisonDelegate lessThanOrEqualDelegate = (first, second) => first <= second;
      MatrixComparisonDelegate greaterThanDelegate = (first, second) => first > second;
      MatrixComparisonDelegate greaterThanOrEqualDelegate = (first, second) => first >= second;
      MatrixComparisonDelegate equalityDelegate = (first, second) => first == second;
      MatrixComparisonDelegate inequalityDelegate = (first, second) => first != second;

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

      while (isWorking)
      {
        Console.Write("Выбор действий:\n1 - Продемонстрировать работу программы\n2 - Выполнить операции с матрицами\n0 - Выйти\nВведите число: ");
        userChoise = Console.ReadLine();

        switch (userChoise)
        {
          case "0":
            isWorking = false;
            break;
          case "1":
            ShowTest();
            break;
          case "2":
            isCalcMode = true;

            while (isCalcMode)
            {
              Console.Write("Доступные операции: +, -, *, >, >=, <, <=, ==, !=, 0 - выйти\nВведите символ операции: ");
              operationChoose = Console.ReadLine();

              switch (operationChoose)
              {
                case "0":
                  isCalcMode = false;
                  break;
                case "+":
                  Console.WriteLine("Результат операции сложения:");
                  SquareMatrix sumOfMatrix = PerformMatrixOperation(additionDelegate, firstMatrix, secondMatrix);
                  Console.WriteLine(sumOfMatrix.ToString());
                  break;
                case "-":
                  Console.WriteLine("Результат операции вычитания:");
                  SquareMatrix subtractionOfMatrix = PerformMatrixOperation(subtractionDelegate, firstMatrix, secondMatrix);
                  Console.WriteLine(subtractionOfMatrix.ToString());
                  break;
                case "*":
                  Console.WriteLine("Результат операции умножения:");
                  SquareMatrix multiplicationOfMatrix = PerformMatrixOperation(multiplicationDelegate, firstMatrix, secondMatrix);
                  Console.WriteLine(multiplicationOfMatrix.ToString());
                  break;
                case ">":
                  Console.WriteLine($"Матрица 1 > Матрица 2: {PerformMatrixComparisonDelegate(greaterThanDelegate, firstMatrix, secondMatrix)}");
                  break;
                case ">=":
                  Console.WriteLine($"Матрица 1 >= Матрица 2: {PerformMatrixComparisonDelegate(greaterThanOrEqualDelegate, firstMatrix, secondMatrix)}");
                  break;
                case "<":
                  Console.WriteLine($"Матрица 1 < Матрица 2: {PerformMatrixComparisonDelegate(lessThanDelegate, firstMatrix, secondMatrix)}");
                  break;
                case "<=":
                  Console.WriteLine($"Матрица 1 <= Матрица 2: {PerformMatrixComparisonDelegate(lessThanOrEqualDelegate, firstMatrix, secondMatrix)}");
                  break;
                case "==":
                  Console.WriteLine($"Матрица 1 == Матрица 2: {PerformMatrixComparisonDelegate(equalityDelegate, firstMatrix, secondMatrix)}");
                  break;
                case "!=":
                  Console.WriteLine($"Матрица 1 != Матрица 2: {PerformMatrixComparisonDelegate(inequalityDelegate, firstMatrix, secondMatrix)}");
                  break;
                default:
                  Console.WriteLine("Данная операция не реализована");
                  break;
              }
            }

            break;
        }
      }

      Console.WriteLine("\nВыполнение программы завершено,\nНажмите любую клавишу, чтобы закрыть это окно");
      // Ожидание нажатия клавиши (чтобы окно не закрывалось сразу после выполнения программы)
      Console.ReadKey();
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

    static void ShowTest()
    {
      Console.WriteLine("\nСоздание случайной матрицы 3x3:");
      SquareMatrix mymatrix = new SquareMatrix();
      mymatrix.AutoFill(3);
      Console.Write(mymatrix.ToString());
      Console.WriteLine("Создание случайной матрицы 3x3:");
      SquareMatrix mymatrix2 = new SquareMatrix();
      mymatrix2.AutoFill(3);
      Console.Write(mymatrix2.ToString());

      Console.WriteLine("\nТест операций сложения, вычитания и умножения");

      Console.WriteLine("\nРезультат операции сложения:");
      SquareMatrix sumOfMatrix = mymatrix + mymatrix2;
      Console.Write(sumOfMatrix.ToString());

      Console.WriteLine("\nРезультат операции вычитания:");
      SquareMatrix subOfMatrix = mymatrix - mymatrix2;
      Console.Write(subOfMatrix.ToString());

      Console.WriteLine("\nРезультат операции умножения:");
      SquareMatrix multiOfMatrix = mymatrix * mymatrix2;
      Console.Write(multiOfMatrix.ToString());

      Console.WriteLine("\nТест операций сравнения");
      int[] minorMatrixArray = new int[4] { 0, 1, 2, 3 };
      int[] majorMatrixArray = new int[4] { 1, 2, 3, 4 };
      int[] equalMatrixArray = new int[4] { 0, 1, 2, 3 };
      SquareMatrix minorMatrix = new SquareMatrix(minorMatrixArray);
      SquareMatrix majorMatrix = new SquareMatrix(majorMatrixArray);
      SquareMatrix equalMatrix = new SquareMatrix(equalMatrixArray);
      Console.WriteLine("\nminorMatrix:");
      Console.Write(minorMatrix.ToString());
      Console.WriteLine("\nmajorMatrix:");
      Console.Write(majorMatrix.ToString());
      Console.WriteLine("\nequalMatrix:");
      Console.Write(equalMatrix.ToString());

      Console.WriteLine($"\nmajorMatrix > minorMatrix: {majorMatrix > minorMatrix}");
      Console.WriteLine($"minorMatrix > majorMatrix: {minorMatrix > majorMatrix}");
      Console.WriteLine($"minorMatrix > equalMatrix: {minorMatrix > equalMatrix}");

      Console.WriteLine($"majorMatrix < minorMatrix: {majorMatrix < minorMatrix}");
      Console.WriteLine($"minorMatrix < majorMatrix: {minorMatrix < majorMatrix}");
      Console.WriteLine($"minorMatrix < equalMatrix: {minorMatrix < equalMatrix}");

      Console.WriteLine($"minorMatrix == majorMatrix: {minorMatrix == majorMatrix}");
      Console.WriteLine($"minorMatrix != majorMatrix: {minorMatrix != majorMatrix}");
      Console.WriteLine($"minorMatrix == equalMatrix: {minorMatrix == equalMatrix}");
      Console.WriteLine($"minorMatrix != equalMatrix: {minorMatrix != equalMatrix}");

      Console.WriteLine("\nТест операции нахождения обратной матрицы для матрицы 1:");
      SquareMatrix transposeMatrix = mymatrix.CalculateTransposeMatrix();
      Console.Write(transposeMatrix.ToString());

      Console.Write("\nТест операции нахождения следа матрицы для матрицы 1: ");
      int matrixTrace = mymatrix.CalculateMatrixTrace();
      Console.WriteLine(matrixTrace.ToString());

      Console.WriteLine("\nТест операции приведения матрицы к диагональному виду для матрицы 1:");
      SquareMatrix diagonalMatrix = diagonalize(mymatrix);
      Console.Write(diagonalMatrix.ToString());

      Console.WriteLine("\nТест операций приведения типов");
      Console.WriteLine("\nМатрица -> двумерный массив:");
      int[,] matrixToArray = new int[2, 2];
      matrixToArray = (int[,])minorMatrix;
      Console.WriteLine(matrixToArray);

      Console.WriteLine("\nМатрица -> строка:");
      string matrixToString;
      matrixToString = (string)majorMatrix;
      Console.WriteLine(matrixToString);

      Console.WriteLine("Одномерный массив -> матрица:");
      int[] arrayToMatrix = new int[4] { 0, 1, 2, 3 };
      SquareMatrix newMatrix = arrayToMatrix;
      Console.Write(newMatrix.ToString());
      Console.WriteLine();
    }
  }
}
