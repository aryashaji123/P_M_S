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
    public class UserInGroupRepository : IUserInGroupRepository
    {
        IConnectionFactory _connectionFactory;

        public UserInGroupRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<UserInGroup>> GetAllAsync()
        {

            string sQuery = "SELECT * FROM USER_MASTER WHERE IS_DELETED=0";
            var result = await _connectionFactory.Connection.QueryAsync<UserInGroup>(sQuery);
            return result.ToList();
        }

        /*
         *  Get Role By ID 
         *  Params: id 
         */
        public async Task<UserInGroup> GetAsync(int id)
        {
            string sQuery = "SELECT * FROM USER_IN_GROUP  WHERE USER_ID = @USER_ID AND IS_DELETED = 0";
            var result = await _connectionFactory.Connection.QueryAsync<UserInGroup>(sQuery, new { USER_ID = id });
            return result.FirstOrDefault();
        }


        /*
         *  Add Role
         */
        public async Task<int> AddAsync(UserInGroup user)
        {
            var result = 0;
            try
            {

                result = await _connectionFactory.Connection.ExecuteAsync(@"insert into [dbo].[USER_IN_GROUP]([USER_ID],[GROUP_ID],[CREATED_BY],[CREATED_ON],[LAST_EDITED_BY],[LAST_EDITED_ON]) values (@userid,@groupid,@created_by,@created_on,@editedby,@editedon)",
                    new { userid = user.USER_ID, groupid = user.GROUP_ID, created_by = user.CREATED_BY, created_on = user.CREATED_ON, editedby = user.LAST_EDITED_BY, editedon = user.LAST_EDITED_ON }
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

                result = await _connectionFactory.Connection.ExecuteAsync(@"update [dbo].USER_IN_GROUP set [IS_DELETED]=1 WHERE USER_ID = @USER_ID", new { USER_ID = id });
            }
            catch (Exception Ex)
            {
                throw new System.InvalidOperationException(Ex.Message);
            }
            return Int32.Parse(result.ToString());
        }
        public async Task<int> UpdateAsync(UserInGroup user)
        {
            var result = 0;
            try
            {

                result = await _connectionFactory.Connection.ExecuteAsync(@"update [dbo].[USER_IN_GROUP] set [USER_ID]=@uid,[GROUP_ID]=@gid,[LAST_EDITED_BY]=@editedby,[LAST_EDITED_ON]=@editedon where [USER_ID]=@uid",
                    new { uid = user.USER_ID, gid = user.GROUP_ID,editedby = user.LAST_EDITED_BY, editedon = user.LAST_EDITED_ON }
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
