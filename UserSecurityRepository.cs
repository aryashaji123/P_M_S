using Dapper;
using PmsApiNew.Domain.Models;
using PmsApiNew.Infrastructure;
using PmsApiNew.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PmsApiNew.Repositories
{
    public class UserSecurityRepository : IUserSecurityRepository
    {
        IConnectionFactory _connectionFactory;

        public UserSecurityRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<UserSecurity>> GetAllAsync()
        {

            string sQuery = "SELECT * FROM USER_SECURITY WHERE IS_DELETED=0";
            var result = await _connectionFactory.Connection.QueryAsync<UserSecurity>(sQuery);
            return result.ToList();
        }

        /*
         *  Get Role By ID 
         *  Params: id 
         */
        public async Task<UserSecurity> GetAsync(int id)
        {
            string sQuery = "SELECT * FROM USER_SECURITY  WHERE USER_ID = @USER_ID AND IS_DELETED = 0";
            var result = await _connectionFactory.Connection.QueryAsync<UserSecurity>(sQuery, new { USER_ID = id });
            return result.FirstOrDefault();
        }


        /*
         *  Add Role
         */
        public async Task<int> AddAsync(UserSecurity user)
        {
            var result = 0;
            try
            {

                result = await _connectionFactory.Connection.ExecuteAsync(@"insert into [dbo].[USER_SECURITY]([USER_ID],[OBJECT_ID],[ACCESS_TYPE],[SECURITY_TYPE],[CREATED_BY],[CREATED_ON],[LAST_EDITED_BY],[LAST_EDITED_ON]) values (@userid,@objectid, @accesstype, @securitytype,@created_by,@created_on,@editedby,@editedon)",
                    new { userid = user.USER_ID, objectid = user.OBJECT_ID, accesstype = user.ACCESS_TYPE, securitytype = user.SECURITY_TYPE, created_by = user.CREATED_BY, created_on = user.CREATED_ON, editedby = user.LAST_EDITED_BY, editedon = user.LAST_EDITED_ON }
                    );
            }
            catch (Exception Ex)
            {

                throw new System.InvalidOperationException(Ex.Message);
            }
            return Int32.Parse(result.ToString());

        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = 0;
            try
            {
                result = await _connectionFactory.Connection.ExecuteAsync(@"update [dbo].USER_SECURITY set [IS_DELETED]=1 WHERE USER_ID = @USER_ID", new { USER_ID = id });
            }
            catch (Exception Ex)
            {
                throw new System.InvalidOperationException(Ex.Message);
            }
            return Int32.Parse(result.ToString());
        }
        public async Task<int> UpdateAsync(UserSecurity user)
        {
            var result = 0;
            try
            {

                result = await _connectionFactory.Connection.ExecuteAsync(@"update [dbo].[USERSECURITY] set [OBJECT_ID]=@objectid,[ACCESS_TYPE]=@accesstype,[SECURITY_TYPE]=@securitytype,[LAST_EDITED_BY]=@editedby,[LAST_EDITED_ON]=@editedon where [USER_ID]=@userid",
                    new { userid = user.USER_ID, objectid = user.OBJECT_ID, accesstype = user.ACCESS_TYPE, securitytype = user.SECURITY_TYPE, editedby = user.LAST_EDITED_BY, editedon = user.LAST_EDITED_ON }
                   );
            }
            catch (Exception Ex)
            {
                throw new System.InvalidOperationException(Ex.Message);
            }
            return Int32.Parse(result.ToString());
        }
    }
}
