using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockApp
{
    class SprocRunner
    {
        public void RunSProc(string instance, string databaseName, string sprocName)
        {
            // Create database connection
            var connection = new RedGate.Shared.SQL.DBConnectionInformation(instance, databaseName);
            var batch = String.Format("EXEC {0}", sprocName);

            using (var block = new RedGate.Shared.SQL.ExecutionBlock.ExecutionBlock())
            {
                block.AddBatch(batch);
                var executor = new RedGate.Shared.SQL.ExecutionBlock.BlockExecutor();
                try
                {
                    executor.ExecuteBlock(block, connection);
                }
                catch (Exception)
                {
                    
                }
                
            }

        }

    }
}
