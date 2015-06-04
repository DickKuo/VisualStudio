using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool;
using CommTool.Business.Services;
using DService.Business.Services;

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
