using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Abp.Data;
using Abp.EntityFrameworkCore;
using CROPS.Projects;
using Microsoft.EntityFrameworkCore;

namespace CROPS.EntityFrameworkCore.Repositories
{
    public class ProjectRepository : CROPSRepositoryBase<Project>, IProjectRepository
    {
        private readonly IActiveTransactionProvider transactionProvider;

        public ProjectRepository(IDbContextProvider<CROPSDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider)
        {
            this.transactionProvider = transactionProvider;
        }

        public async Task<SP_GetProjectDataByProjectAreaPath_Result> GetProjectDataByProjectAreaPath(string projectAreaPath)
        {
            EnsureConnectionOpen();
            List<SP_GetProjectDataByProjectAreaPath_Result> result = new List<SP_GetProjectDataByProjectAreaPath_Result>();
            using (var command = CreateCommand("SP_GetProjectDataByProjectAreaPath", CommandType.StoredProcedure, new SqlParameter("@ProjectAreaPath", projectAreaPath)))
            {
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    while (dataReader.Read())
                    {
                        result.Add(new SP_GetProjectDataByProjectAreaPath_Result
                        {
                            ParentNodeSK = (int)dataReader["ParentNodeSK"],
                            ProjectNodeSK = (int)dataReader["ProjectNodeSK"],
                            ReleaseSourceId = (int)dataReader["ReleaseSourceId"]
                        });
                    }

                    return result.FirstOrDefault();
                }
            }
        }

        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = Context.Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction();

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        private void EnsureConnectionOpen()
        {
            var connection = Context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
            {
                { "ContextType", typeof(CROPSDbContext) },
                { "MultiTenancySide", MultiTenancySide }
            });
        }
    }
}
