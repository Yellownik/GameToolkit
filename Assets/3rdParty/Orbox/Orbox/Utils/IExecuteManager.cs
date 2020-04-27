using System;

namespace Orbox.Utils
{
    // You need to add script as last in Script Execution Order Settings
    // that call IExecuteManager.RaiseLastAwake()

    public interface IExecuteManager 
    {
        void RaiseLastAwake();
        void RunOnLastAwake(Action action);       
    }
}