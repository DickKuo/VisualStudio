using CommTool.Business.Services;
using DService.Business.Services;
using CommTool;
using System;

namespace Implement.Services
{
    [ServiceClass(typeof(ITestService), ServiceCreateType.Callback)]
    public class TestService
    {
        public void HelloWord()
        {
            Console.WriteLine("HelloWord!!");
        }
    }
}