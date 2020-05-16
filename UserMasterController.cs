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
    public class UserMasterController : ControllerBase
    {
        private readonly IUserMasterService _userMasterService;
        private readonly IGenerateIdService _generateIdService;

        public UserMasterController(IUserMasterService userMasterService,IGenerateIdService generateIdService)
        {
            _userMasterService = userMasterService;
            _generateIdService = generateIdService;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<UserMasterList> ListAsync()
        {
            UserMasterList objItem = new UserMasterList();
            MessageInfo messageInfo = new MessageInfo();
            try
            {
                var roles = await _userMasterService.ListAsync();

                if (roles.Count() > 0)
                {
                    objItem.UserMaster = roles;
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
        public async Task<UserMasterById> ListAsyncId(int id)
        {
            UserMasterById objItem = new UserMasterById();
            MessageInfo obj = new MessageInfo();
            try
            {
                var roles = await _userMasterService.ListAsyncId(id);

                if (roles != null)
                {
                    objItem.UserMaster = roles;
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
        public async Task<MessageInfo> AddAsync([FromBody]UserMaster user)
        {
            user.CREATED_BY = 1;
            user.CREATED_ON = DateTime.Now;
            user.LAST_EDITED_BY = 1;
            user.LAST_EDITED_ON = DateTime.Now;
            MessageInfo obj = new MessageInfo();
            try
            {
                var ItemId = await _generateIdService.GetId(Enumerators.Identifiers.USER_ID);
                user.USER_ID = ItemId.NEXT_USER_ID;


                var roles = await _userMasterService.AddAsync(user);
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
        public async Task<MessageInfo> UpdateAsync([FromBody]UserMaster user)
        {
            // Item.CREATED_BY = 1;
            //Item.CREATED_ON = DateTime.Now;
            user.LAST_EDITED_BY = 1;
            user.LAST_EDITED_ON = DateTime.Now;
            MessageInfo obj = new MessageInfo();
            try
            {
                var roles = await _userMasterService.UpdateAsync(user);

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
                var roles = await _userMasterService.DeleteAsync(id);
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