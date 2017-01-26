using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool;
using StandredImplement;

namespace DStandardServer
{
    public class ServerImplement : TriggerService
    {
        public override TriggerService GetAutoTriggerService()
        {
            TriggerService trs = base.GetAutoTriggerService();
            //trs.AddTriggers(new SampleService());  //Sample
            //trs.AddTriggers(new GetGoldTrigger()); 

            trs.AddTriggers(new GetBueaty());            
            trs.AddTriggers(new OptionTrigger());
            return trs;
        }
    }
}
