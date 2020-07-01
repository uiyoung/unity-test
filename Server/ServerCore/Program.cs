using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static bool _stop = false;

        static void ThreadMain()
        {
            Console.WriteLine("스레드 시작");
            while(_stop == false)
            {
                // 누군가가 stop 신호를 해주기를 기달린다.
            }
            Console.WriteLine("스레드 종료");
        }

        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(1000);

            _stop = true;

            Console.WriteLine("stop 호출");
            Console.WriteLine("종료 대기중");

            t.Wait();
            Console.WriteLine("종료 성공");
        }
    }
}
