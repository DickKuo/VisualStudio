using StandredImplement;
using CommTool;

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
            trs.AddTriggers(new ControlPriceService());
            return trs;
        }
    }
}