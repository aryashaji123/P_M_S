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
    public class UserMasterRepository : IUserMasterRepository
    {
        IConnectionFactory _connectionFactory;

        public UserMasterRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<UserMaster>> GetAllAsync()
        {

            string sQuery = "SELECT * FROM USER_MASTER WHERE IS_DELETED=0";
            var result = await _connectionFactory.Connection.QueryAsync<UserMaster>(sQuery);
            return result.ToList();
        }

        /*
         *  Get Role By ID 
         *  Params: id 
         */
        public async Task<UserMaster> GetAsync(int id)
        {
            string sQuery = "SELECT * FROM USER_MASTER  WHERE USER_ID = @USER_ID AND IS_DELETED = 0";
            var result = await _connectionFactory.Connection.QueryAsync<UserMaster>(sQuery, new { USER_ID = id });
            return result.FirstOrDefault();
        }


        /*
         *  Add Role
         */
        public async Task<int> AddAsync(UserMaster user)
        {
            var result = 0;
            try
            {

                result = await _connectionFactory.Connection.ExecuteAsync(@"insert into [dbo].[USER_MASTER]([USER_ID],[USER_NAME],[LOGIN_NAME],[PASSWORD],[EMAIL_ID],[TOKEN],[CREATED_BY],[CREATED_ON],[LAST_EDITED_BY],[LAST_EDITED_ON]) values (@userid,@username, @loginname, @password,@email_id,@token,@created_by,@created_on,@editedby,@editedon)",
                    new { userid = user.USER_ID, username = user.USER_NAME, loginname = user.LOGIN_NAME, password = user.PASSWORD, email_id = user.EMAIL_ID, token = user.TOKEN, created_by = user.CREATED_BY, created_on = user.CREATED_ON ,editedby = user.LAST_EDITED_BY,editedon = user.LAST_EDITED_ON}
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
                result = await _connectionFactory.Connection.ExecuteAsync(@"update [dbo].USER_MASTER set [IS_DELETED]=1 WHERE USER_ID = @USER_ID", new { USER_ID = id });
            }
            catch (Exception Ex)
            {
                throw new System.InvalidOperationException(Ex.Message);
            }
            return Int32.Parse(result.ToString());
        }
        public async Task<int> UpdateAsync(UserMaster user)
        {
            var result = 0;
            try
            {

                result = await _connectionFactory.Connection.ExecuteAsync(@"update [dbo].[USER_MASTER] set [USER_NAME]=@username,[LOGIN_NAME]=@loginname,[EMAIL_ID]=@email_id,[TOKEN]=@token,[PASSWORD]=@password,[LAST_EDITED_BY]=@editedby,[LAST_EDITED_ON]=@editedon where [USER_ID]=@userid",
                    new { userid = user.USER_ID, username = user.USER_NAME, loginname = user.LOGIN_NAME, password = user.PASSWORD, email_id = user.EMAIL_ID, token = user.TOKEN,editedby=user.LAST_EDITED_BY,editedon=user.LAST_EDITED_ON}
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
