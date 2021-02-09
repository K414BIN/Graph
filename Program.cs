using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Добрый день!  ");
            Console.Write("Сегодня будем строить направленные нагруженные графы. ");
            Console.Write("Введите целочисленное значение: ");
            int userX = Convert.ToInt32(System.Console.ReadLine());
            userX = Math.Abs(userX);
            int[,] matrix=  InitMatrix(userX);
            // если число графов меньше 24, то можно посмотреть на экране эту матрицу.
            // если больше, то некрасиво будет выглядеть.
            if (userX<24) printMatrix(matrix);
            // это кортеж - я научился им пользоваться!
            (List <Edge> ,Graph) AllBeAdded =   CollectEdgesAndVortex(matrix);
            Graph graph = AllBeAdded.Item2;
            var edglist =  AllBeAdded.Item1;
            System.Console.WriteLine();
            Console.WriteLine("Вызываем обход в ширину (волновой метод)");
            for (int r=0; r<userX;r++)    Wave(edglist,r,userX);
            ShowMeGraph(graph);
            Console.WriteLine();
            #region
            ///< summary >
            /// Этот пример расчета кратчайшего пути взят из методички. 
            /// Смысл этого кода для меня непонятен, но привожу его как проверочный.
            /// Весь код сохранен с комментариями из методички.
            ///  </summary>  
            int[] active = new int [userX]; // Состояния вершин (просмотрена или не просмотрена)
            int[] route = new int[userX];
            int[] peak = new int[userX];
            int i;
            int j;
            int min;
            int kMin = 0;
            // В начале программы присваиваем начальные значения.
            // Если = 1, то вершина ещё не просмотрена 
            for (i = 0; i < userX; i++)
            {
                active[i] = 1;
                route[i] = matrix[0, i];
                peak[i] = 0;
            }

            // Сразу помечаем, что вершина A(0) просмотрена,
            // с неё начинается маршрут
            active[0] = 0;
            for (i = 0; i < userX - 1; i++)
            {
                // Среди активных вершин
                // ищем вершину с минимальным соответствующим значением в массиве R
                // и проверяем, не лучше ли ехать через неё:
                min = int.MaxValue;
                for (j = 0; j < userX; j++)
                    if (active[j] == 1 && route[j] < min)
                    {
                        min = route[j]; // Минимальный маршрут
                        kMin = j; // Номер вершины с минимальным маршрутом
                    }

                active[kMin] = 0; // Просмотрели эту точку
                                  // Проверка маршрута через вершину kMin
                                  // Есть ли более короткий путь
                for (j = 0; j <userX; j++)
                    // Если текущий путь в вершину J (R[j]
                    // больше чем путь из найденной вершины (R[kMin] +
                    // путь из этой вершины W[kMin][j], то
                    if (route[j] > route[kMin] + matrix[j, kMin] &&
                        matrix[j, kMin] != int.MaxValue && active[j] == 1)
                    {
                        // мы запоминаем новое расстояние
                        route[j] = route[kMin] + matrix[j, kMin];
                        // и запоминаем, что можем добраться туда более
                        // коротким путём в массиве P
                        peak[j] = kMin;
                    }
            }
            ///< summary >
            /// Найти все кратчайшие пути из любой вершины в любую другую можно с помощью алгоритма Флойда — Уоршелла.
            /// </summary >
            i = userX - 1;
            Console.WriteLine("3десь через пробел выводится самые короткие пути из одной точки в другую.");
            while (i != 0) { 
             ///< summary >
             /// Вот здесь через пробел выводится самые короткие пути из одной точки в другую.
             /// </summary >
                Console.Write($"{i}");
                Console.Write( ' ');
                i = peak[i];
            }
            ///< summary >
            /// Здесь запускаем в методе алгоритм Флойда — Уоршелла. 
            /// Я не понял, что это такое и зачем это нужно.
            /// Хотелось бы понять зачем это рекомендовано нам в методичке?
            ///  </summary>  
            FloydWarshal(matrix,userX);
            #endregion
         
    
                Console.ReadLine();
        }
        private static void Wave(List<Edge> edgeslist, int NodeIndex, int Nodes)
        {
            int g = NodeIndex;
            Stack<Node> routeFrom = new Stack<Node>(Nodes * Nodes );
            routeFrom.Clear();
            // Cтек постоянно теряет значения больше первой волны
            while (g != Nodes)
            {
                if (edgeslist[g].From.Value == NodeIndex + 1 && edgeslist[g].Weight != 0)
                {
                    routeFrom.Push(edgeslist[g].To);
                   
                }
                g++;
            }
            g = routeFrom.Count;
            Console.Write("Уровень волны : ");
            Console.WriteLine(NodeIndex + 1);
            while (g != 0)
            {
              //  if (routeFrom.Count == 0) Console.Write("Стек пуст");
                Node route = routeFrom.Pop();
                Console.Write(route.Value);
                Console.Write(",");
                Console.Write(" ");
                g--;
            }
            
            Console.WriteLine();
        }
        // Заполняем список ребер и наш граф    
         public static (List<Edge>,Graph)  CollectEdgesAndVortex(int[,] M)
        {
            int j = M.GetLength(1) - 1;
            var graph = new Graph();
            for (int i = 0; i <= j; i++) graph.AddVertex(new Node(i + 1));
            for (int i = 0; i <= j; i++)
            {
                 var h = new Node(i + 1);
                for (int k = 0; k <= j; k++)
                {
                    var v = new Node(k + 1);
                    if (M[i, k] != 0)
                    {
                        graph.AddEdge(h, v, M[i, k]);
                    }
                }
            }
            return (graph.Edges,graph);       
        }
        static void ShowMeGraph(Graph graph)
        {
            Console.WriteLine();
            Console.Write("Всего ребер = ");
            Console.WriteLine(graph.Edges.Count);
            Console.WriteLine("№) Из вершины ----> В другую вершину = Вес");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            foreach (var item in graph.Edges)
            {
                Console.Write(graph.Edges.IndexOf(item)+1);
               Console.Write(")");
                Console.Write("     ");
                Console.Write(item.From.Value);
                Console.Write("      ----> ");
                Console.Write("       ");
                Console.Write(item.To.Value);
                Console.Write("         ");
                Console.Write("  = ");
                Console.Write(item.Weight);
                Console.WriteLine();
            }
        }
        static void printMatrix(int[,] M)
        {
            int userX = M.GetLength(1);
            for (int i = 0; i < userX; i++)
            {
                Console.Write(' ');
                if (i < 10) { Console.Write(' '); }
                if (i == 9) { Console.Write('\b'); Console.Write('\b'); }
                Console.Write(i + 1);
                Console.Write(' ');

            }
            System.Console.WriteLine();
            for (int j = 0; j < 4 * userX + 1; j++) Console.Write('-');
            System.Console.WriteLine();
            for (int j = 0; j < userX; j++)
            {

                for (int i = 0; i < userX; i++)
                {
                    if (M[j, i] == 0) { Console.Write(' '); }
                    System.Console.Write(M[j, i]);
                    Console.Write(' ');
                    Console.Write(' ');
                }
                Console.Write('|');
                Console.Write(' ');
                System.Console.WriteLine(j + 1);
            }
        }
        // Чтобы рассчитать расстояния в графе используют матрицу смежности.
        // Я использовал матрицу смежности для создания графов.
        // В итоге получаем направленные нагруженные графы.
        static int[,] InitMatrix(int x)
        {
            int[,] W = new int[x,x];
            var rnd = new Random();
       
            for (int j = 0; j < x; j++)
            {
                for (int i = 0; i < x; i++)
                {

                    W[j, i] = rnd.Next(2) * (rnd.Next(89) + 10);
                 
                }
            }
            return W;
        }
        // из методички метод
       static void FloydWarshal(int[,] W, int n)
        {
            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (W[i, k] + W[k, j] < W[i, j]) W[i, j] = W[i, k] + W[k, j];

        }
    }
}
