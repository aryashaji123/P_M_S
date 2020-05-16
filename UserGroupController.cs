using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PmsApiNew.Domain.Models;
using PmsApiNew.Services.Interfaces;
using PmsApiNew.Utilities;

namespace PmsApiNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly IUserGroupService _userGroupService;
        private readonly IGenerateIdService _generateIdService;

        public UserGroupController(IUserGroupService userGroupService, IGenerateIdService generateIdService)
        {
            _userGroupService = userGroupService;
            _generateIdService = generateIdService;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<UserGroupList> ListAsync()
        {
            UserGroupList objItem = new UserGroupList();
            MessageInfo messageInfo = new MessageInfo();
            try
            {
                var roles = await _userGroupService.ListAsync();

                if (roles.Count() > 0)
                {
                    objItem.UserGroup = roles;
                    messageInfo.Info = "200";
                    messageInfo.Message = "Success";
                    objItem.Message = messageInfo;

                }
                else
                {
                    messageInfo.Info = "400";
                    messageInfo.Message = "No Data";
                    objItem.Message = messageInfo;
                }
            }
            catch (Exception Ex)
            {
                objItem.Message.Info = "400";
                objItem.Message.Message = Ex.Message;
                objItem.Message = messageInfo;
            }
            return objItem;



        }

        //GET: api/Roles/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpGet("{id}"/*, Name = "Get"*/)]
        public async Task<UserGroupById> ListAsyncId(int id)
        {
            UserGroupById objItem = new UserGroupById();
            MessageInfo obj = new MessageInfo();
            try
            {
                var roles = await _userGroupService.ListAsyncId(id);

                if (roles != null)
                {
                    objItem.UserGroup = roles;
                    obj.Info = "200";
                    obj.Message = "Success";
                    objItem.Message = obj;
                }
                else
                {
                    obj.Info = "400";
                    obj.Message = "No Data";
                    objItem.Message = obj;
                }
            }
            catch (Exception Ex)
            {

                obj.Info = "400";
                obj.Message = Ex.Message;
                objItem.Message = obj;
            }

            return objItem;

        }
        // POST: api/Roles
        [HttpPost]
        public async Task<MessageInfo> AddAsync([FromBody]UserGroup user)
        {
            user.CREATED_BY = 1;
            user.CREATED_ON = DateTime.Now;
            user.LAST_EDITED_BY = 1;
            user.LAST_EDITED_ON = DateTime.Now;
            MessageInfo obj = new MessageInfo();
            try
            {
                var ItemId = await _generateIdService.GetId(Enumerators.Identifiers.GROUP_ID);
                user.GROUP_ID = ItemId.NEXT_GROUP_ID;


                var roles = await _userGroupService.AddAsync(user);
                if (roles > 0)
                {
                    obj.Info = "200";
                    obj.Message = "Successfully Inserted";


                }
                else
                {
                    obj.Info = "400";
                    obj.Message = "Insertion Failed";


                }
            }
            catch (Exception Ex)
            {

                obj.Info = "400";
                obj.Message = Ex.Message;
            }

            return obj;

        }



        // PUT: api/Roles/5
        [HttpPut]
        public async Task<MessageInfo> UpdateAsync([FromBody]UserGroup user)
        {
            // Item.CREATED_BY = 1;
            //Item.CREATED_ON = DateTime.Now;
            user.LAST_EDITED_BY = 1;
            user.LAST_EDITED_ON = DateTime.Now;
            MessageInfo obj = new MessageInfo();
            try
            {
                var roles = await _userGroupService.UpdateAsync(user);

                if (roles > 0)
                {
                    obj.Info = "200";
                    obj.Message = "Successfully Updated";
                }
                else
                {
                    obj.Info = "400";
                    obj.Message = "Updation Failed";
                }
            }
            catch (Exception Ex)
            {

                obj.Info = "400";
                obj.Message = Ex.Message;
            }
            return obj;

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<MessageInfo> DeleteAsync(int id)
        {
            MessageInfo obj = new MessageInfo();
            try
            {
                var roles = await _userGroupService.DeleteAsync(id);
                if (roles > 0)
                {
                    obj.Info = "200";
                    obj.Message = "Successfully deleted";
                }
                else
                {
                    obj.Info = "400";
                    obj.Message = "Deletion Failed";
                }
            }
            catch (Exception Ex)
            {

                obj.Info = "400";
                obj.Message = Ex.Message;
            }
            return obj;

        }
    }
}