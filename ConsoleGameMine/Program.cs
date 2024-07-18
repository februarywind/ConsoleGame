namespace ConsoleApp1
{
    internal class Program
    {
        struct Point
        {
            public bool mine;
            public bool mineVisible;
            public int mineCount;
            public bool mineFlag;
        }
        static Point[] MineMap = new Point[80];

        struct PlayerXy
        {
            public int x, y;
        }
        static PlayerXy playerXy = new PlayerXy();

        static bool running = true;
        static int score = 0;

        static int gameBoardX;
        static int gameBoardY;
        static int gameMine;

        static void Main(string[] args)
        {
            GameStart();

            while (running)
            {
                Arrow(Console.ReadKey(false).Key);
                scoreCheck();
            }
        }

        static void GameStart()
        {
            int select;
            do
            {
                Console.WriteLine("방향키로 움직이고 스페이스로 클릭, W로 깃발 꽃기");
                Console.WriteLine("난이도를 선택 하세요.\n1. 초급\n2. 중급\n3. 상급");
                int.TryParse(Console.ReadLine(), out select);
            } while (select < 1 || select > 3);
            Console.Clear();
            switch (select)
            {
                case 1:
                    gameBoardX = 9;
                    gameBoardY = 9;
                    gameMine = 9;
                    break;
                case 2:
                    gameBoardX = 16;
                    gameBoardY = 16;
                    gameMine = 40;
                    break;
                case 3:
                    gameBoardX = 30;
                    gameBoardY = 16;
                    gameMine = 99;
                    break;
            }
            Array.Resize(ref MineMap, gameBoardX * gameBoardY);
            Console.CursorVisible = false;

            playerXy.x = 0;
            playerXy.y = 0;

            SetMine();
            PaintMap();
        }

        static void SetMine()
        {
            for (int i = 0; i < gameMine; i++)
            {
                int randMine = new Random().Next(0, gameBoardX * gameBoardY);
                if (!MineMap[randMine].mine)
                {
                    MineMap[randMine].mine = true;
                }
                else
                {
                    i--;
                }
            }

            for (int i = 0; i < gameBoardY; i++)
            {
                for (int j = 0; j < gameBoardX; j++)
                {
                    MineMap[j + (i * gameBoardX)].mineCount = MineCheck(j + (i * gameBoardX), false);
                }
            }
        }
        static void PaintMap()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            bool PlayerPoint = false;
            for (int i = 0; i < gameBoardY; i++)
            {
                for (int j = 0; j < gameBoardX; j++)
                {
                    if (playerXy.x == j && playerXy.y == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        PlayerPoint = true;
                    }
                    else
                    {
                        PlayerPoint = false;
                    }
                    Console.SetCursorPosition(j, i);
                    if (MineMap[j + (i * gameBoardX)].mineFlag)
                    {
                        if (!PlayerPoint)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                        Console.Write("F ");
                    }
                    else if (!MineMap[j + (i * gameBoardX)].mineVisible)
                    {
                        Console.Write("?");
                    }
                    else if (MineMap[j + (i * gameBoardX)].mine)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("M");
                    }
                    else
                    {
                        if (!PlayerPoint)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.Write(MineMap[j + (i * gameBoardX)].mineCount);
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
            }
        }

        static void Arrow(ConsoleKey consoleKey)
        {
            switch (consoleKey)
            {
                case ConsoleKey.UpArrow:
                    if (playerXy.y > 0)
                    {
                        playerXy.y--;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (playerXy.x > 0)
                    {
                        playerXy.x--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (playerXy.y < gameBoardY - 1)
                    {
                        playerXy.y++;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (playerXy.x < gameBoardX - 1)
                    {
                        playerXy.x++;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    MineMap[playerXy.x + playerXy.y * gameBoardX].mineVisible = true;
                    if (MineMap[playerXy.x + playerXy.y * gameBoardX].mineCount == 0)
                    {
                        MineCheck(playerXy.x + playerXy.y * gameBoardX, true);
                    }
                    if (MineMap[playerXy.x + playerXy.y * gameBoardX].mine)
                    {
                        running = false;
                    }
                    PaintMap();
                    break;
                case ConsoleKey.W:
                    MineMap[playerXy.x + playerXy.y * gameBoardX].mineFlag = !MineMap[playerXy.x + playerXy.y * gameBoardX].mineFlag;
                    PaintMap();
                    break;
            }
            PaintMap();
        }

        static int MineCheck(int p, bool zeroCheck)
        {
            int mineCount = 0;
            if (p > gameBoardX - 1 && p % gameBoardX != 0)
            {
                if (zeroCheck)
                {
                    MineMap[p - gameBoardX - 1].mineVisible = true;
                }
                else if (MineMap[p - gameBoardX - 1].mine)
                    mineCount++;
            }
            if (p > gameBoardX - 1)
            {
                if (zeroCheck)
                {
                    MineMap[p - gameBoardX].mineVisible = true;
                }
                else if (MineMap[p - gameBoardX].mine)
                    mineCount++;
            }
            if (p > gameBoardX - 1 && p % gameBoardX != gameBoardX - 1)
            {
                if (zeroCheck)
                {
                    MineMap[p - gameBoardX + 1].mineVisible = true;
                }
                else if (MineMap[p - gameBoardX + 1].mine)
                    mineCount++;
            }
            if (p % gameBoardX != 0)
            {
                if (zeroCheck)
                {
                    MineMap[p - 1].mineVisible = true;
                }
                else if (MineMap[p - 1].mine)
                    mineCount++;
            }
            if (p % gameBoardX != gameBoardX - 1)
            {
                if (zeroCheck)
                {
                    MineMap[p + 1].mineVisible = true;
                }
                else if (MineMap[p + 1].mine)
                    mineCount++;
            }
            if (p % gameBoardX != 0 && p < gameBoardX * gameBoardY - gameBoardX)
            {
                if (zeroCheck)
                {
                    MineMap[p + gameBoardX - 1].mineVisible = true;
                }
                else if (MineMap[p + gameBoardX - 1].mine)
                    mineCount++;
            }
            if (p < gameBoardX * gameBoardY - gameBoardX)
            {
                if (zeroCheck)
                {
                    MineMap[p + gameBoardX].mineVisible = true;
                }
                else if (MineMap[p + gameBoardX].mine)
                    mineCount++;
            }
            if (p < gameBoardX * gameBoardY - gameBoardX && p % gameBoardX != gameBoardX - 1)
            {
                if (zeroCheck)
                {
                    MineMap[p + gameBoardX + 1].mineVisible = true;
                }
                else if (MineMap[p + gameBoardX + 1].mine)
                    mineCount++;
            }
            return mineCount;
        }

        static void scoreCheck()
        {
            int temp = 0;
            foreach (var item in MineMap)
            {
                if (item.mineVisible)
                {
                    temp++;
                }
            }
            if (temp == gameBoardX * gameBoardY - gameBoardX)
            {
                running = false;
            }
        }
    }
}