using Dapper;
using PmsApiNew.Domain.Models;
using PmsApiNew.Infrastructure;
using PmsApiNew.Repositories.Interfaces;
using PmsApiNew.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PmsApiNew.Repositories
{
    public class GenerateIdRepository : IGenerateIdRepository
    {
        IConnectionFactory _connectionFactory;



        public GenerateIdRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<GetId> GetId(Enumerators.Identifiers eIdentifier)
        {

            string sQuery = string.Empty;
            switch (eIdentifier)
            {
                case Enumerators.Identifiers.USER_ID:
                    {
                        sQuery = "SELECT NEXT_USER_ID  FROM SYS_COUNTERS ";
                        sQuery = SQLManager.FilterFields(sQuery, "NEXT_USER_ID ");
                        sQuery = sQuery + ";" + "UPDATE SYS_COUNTERS SET NEXT_USER_ID  = NEXT_USER_ID  + 1";

                    }
                    break;
                case Enumerators.Identifiers.GROUP_ID:
                    {
                        sQuery = "SELECT NEXT_GROUP_ID  FROM SYS_COUNTERS ";
                        sQuery = SQLManager.FilterFields(sQuery, "NEXT_GROUP_ID ");
                        sQuery = sQuery + ";" + "UPDATE SYS_COUNTERS SET NEXT_GROUP_ID  = NEXT_GROUP_ID  + 1";

                    }
                    break;

            }
            var result = await _connectionFactory.Connection.QueryAsync<GetId>(sQuery);
            return result.FirstOrDefault();
        }

       
    }
}
