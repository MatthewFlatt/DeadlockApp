using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedGate.Shared.SQL;

namespace DeadlockApp
{
    class SprocRunner
    {
        public void RunSProc(string instance, string databaseName, string sprocName, string username, string password)
        {
            // Create database connection
            var connection = string.IsNullOrEmpty(username) ? new DBConnectionInformation(instance, databaseName) : new DBConnectionInformation(instance, databaseName, username, password);
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
