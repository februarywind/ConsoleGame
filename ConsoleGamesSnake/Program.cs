namespace ConsoleApp2
{
    internal class Program
    {

        public struct Point
        {
            public int x;
            public int y;
        }

        static Point[] SnakeBody = new Point[5];

        static Point starPoint = new Point();

        static bool running = true;

        static int wasd = 4;
        
        static int score = 4;

        static int speed = 200;
        
        static void Main(string[] args)
        {
            GameStart();

            // 게임루프
            while (running)
            {
                // 0.2초마다
                Thread.Sleep(speed);

                SnakeMove();
                SnakePaint();

                PaintMap();

                // 입력 있을때만 ReadKey실행
                if (Console.KeyAvailable)
                {
                    Wasd();
                }
            }
        }

        static void PaintMap()
        {
            // 윗면, 아랫면 그리기
            for (int i = 0; i < 40; i += 2)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("□");
                Console.SetCursorPosition(i, 19);
                Console.Write("□");
            }
            // 옆면 그리기
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("□");
                Console.SetCursorPosition(40, i);
                Console.WriteLine("□");
            }
        }

        static void Wasd()
        {
            ConsoleKey a = Console.ReadKey(false).Key;
            switch (a)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    wasd = 1;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    wasd = 2;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    wasd = 3;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    wasd = 4;
                    break;
                case ConsoleKey.Spacebar:
                    if (speed == 200)
                    {
                        speed = 100;
                    }
                    else
                    {
                        speed = 200;
                    }
                    break;
                default:
                    break;
            }
        }

        static void SnakeMove()
        {
            // 뱀 몸통 배열의 복사본 생성
            Point[] tempBody = new Point[SnakeBody.Length];
            for (int i = 0; i < SnakeBody.Length; i++)
            {
                tempBody[i].x = SnakeBody[i].x;
                tempBody[i].y = SnakeBody[i].y;
            }

            // 뱀 꼬리 텍스트 삭제
            Console.SetCursorPosition(SnakeBody[SnakeBody.Length - 1].x, SnakeBody[SnakeBody.Length - 1].y);
            Console.WriteLine(" ");

            switch (wasd)
            {
                // 모든 배열 값을 한칸씩 밀어서 마지막 배열값을 지우고 움직이는 방향쪽으로 첫번째 배열값을 변경
                case 1:
                    for (int i = 0; i < SnakeBody.Length - 1; i++)
                    {
                        SnakeBody[i + 1].x = tempBody[i].x;
                        SnakeBody[i + 1].y = tempBody[i].y;
                    }
                    SnakeBody[0].y--;
                    break;
                case 2:
                    for (int i = 0; i < SnakeBody.Length - 1; i++)
                    {
                        SnakeBody[i + 1].x = tempBody[i].x;
                        SnakeBody[i + 1].y = tempBody[i].y;
                    }
                    SnakeBody[0].x--;
                    break;
                case 3:
                    for (int i = 0; i < SnakeBody.Length - 1; i++)
                    {
                        SnakeBody[i + 1].x = tempBody[i].x;
                        SnakeBody[i + 1].y = tempBody[i].y;
                    }
                    SnakeBody[0].y++;
                    break;
                case 4:
                    for (int i = 0; i < SnakeBody.Length - 1; i++)
                    {
                        SnakeBody[i + 1].x = tempBody[i].x;
                        SnakeBody[i + 1].y = tempBody[i].y;
                    }
                    SnakeBody[0].x++;
                    break;
            }

            // 레벨업 별을 먹으면 배열 길이를 증가시킨다. 이동하면 자동으로 생성된 배열이 채워진다.
            foreach (var item in SnakeBody)
            {
                if (starPoint.x == item.x && starPoint.y == item.y)
                {
                    Console.SetCursorPosition(starPoint.x, starPoint.y);
                    Console.Write("     ");
                    LevelUp();
                    Array.Resize(ref SnakeBody, SnakeBody.Length + 1);
                }
            }

            // 뱀이 자신에게 닿으면 게임 종료
            for (int i = 0; i < SnakeBody.Length; i++)
            {
                if (SnakeBody[0].x == SnakeBody[i].x && SnakeBody[0].y == SnakeBody[i].y && i != 0)
                {
                    GameOver();
                }
            }

            // 뱀이 벽에 부디치면 게임 종료
            if (SnakeBody[0].x <= 1 || SnakeBody[0].x >= 40 || SnakeBody[0].y < 1 || SnakeBody[0].y >= 19)
            {
                GameOver();
            }
        }

        static void LevelUp()
        {
            score++;
            starPoint.x = new Random().Next(2, 39);
            starPoint.y = new Random().Next(1, 19);
            Console.SetCursorPosition(starPoint.x, starPoint.y);
            Console.Write("L");
        }
        static void SnakePaint()
        {
            foreach (var item in SnakeBody)
            {
                Console.SetCursorPosition(item.x, item.y);
                Console.WriteLine("S");
            }
        }
    
        static void GameOver()
        {
            running = false;
            Console.SetCursorPosition(50, 6);
            Console.WriteLine($"게임 오버 {score}점 입니다.");
        }
        static void GameStart()
        {
            // 커서 안보이게
            Console.CursorVisible = false;

            // 시작 뱀 설정
            for (int i = 0; i < 5; i++)
            {
                SnakeBody[i].x = 4 - i;
                SnakeBody[i].y = 10;
            }

            // 레벨업 별 생성
            LevelUp();

            Console.SetCursorPosition(50, 5);
            Console.WriteLine($"wasd로 움직이고 스페이스 토글로 속도가 두배라니.");
        }
    }
}
