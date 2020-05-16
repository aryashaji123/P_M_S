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
    public class UserGroupRepository : IUserGroupRepository
    {
        IConnectionFactory _connectionFactory;
        public UserGroupRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<IEnumerable<UserGroup>> GetAllAsync()
        {

            string sQuery = "SELECT * FROM USER_GROUP WHERE IS_DELETED=0";
            var result = await _connectionFactory.Connection.QueryAsync<UserGroup>(sQuery);
            return result.ToList();
        }

        /*
         *  Get Role By ID 
         *  Params: id 
         */
        public async Task<UserGroup> GetAsync(int id)
        {
            string sQuery = "SELECT * FROM USER_GROUP  WHERE GROUP_ID = @ID AND IS_DELETED = 0";
            var result = await _connectionFactory.Connection.QueryAsync<UserGroup>(sQuery, new {ID = id });
            return result.FirstOrDefault();
        }


        /*
         *  Add Role
         */
        public async Task<int> AddAsync(UserGroup group)
        {
            var result = 0;
            try
            {

                result = await _connectionFactory.Connection.ExecuteAsync(@"insert into [dbo].[USER_GROUP]([GROUP_ID],[GROUP_NAME],[CREATED_BY],[CREATED_ON],[LAST_EDITED_BY],[LAST_EDITED_ON]) values (@gid,@gname,@created_by,@created_on,@editedby,@editedon)",
                    new { gid = group.GROUP_ID, gname = group.GROUP_NAME,  created_by = group.CREATED_BY, created_on = group.CREATED_ON, editedby = group.LAST_EDITED_BY, editedon = group.LAST_EDITED_ON }
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

                result = await _connectionFactory.Connection.ExecuteAsync(@"update [dbo].USER_GROUP set [IS_DELETED]=1 WHERE GROUP_ID = @ID", new { ID = id });
            }
            catch (Exception Ex)
            {
                throw new System.InvalidOperationException(Ex.Message);
            }
            return Int32.Parse(result.ToString());
        }
        public async Task<int> UpdateAsync(UserGroup group)
        {
            var result = 0;
            try
            {

                result = await _connectionFactory.Connection.ExecuteAsync(@"update [dbo].[USER_GROUP] set [GROUP_NAME]=@gname,[LAST_EDITED_BY]=@editedby,[LAST_EDITED_ON]=@editedon where [GROUP_ID]=@gid",
                    new { gid = group.GROUP_ID, gname = group.GROUP_NAME,  editedby = group.LAST_EDITED_BY, editedon = group.LAST_EDITED_ON }
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
