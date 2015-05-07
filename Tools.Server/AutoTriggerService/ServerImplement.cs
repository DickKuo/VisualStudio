using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool;

namespace AutoTriggerService
{
    public class CaseTriggerStandard : TriggerService
    {
        public override TriggerService GetAutoTriggerService()
        {
            TriggerService trs = base.GetAutoTriggerService();
            trs.AddTriggers(new GetPTTBueaty());            
            return trs;
        }
    }
}
