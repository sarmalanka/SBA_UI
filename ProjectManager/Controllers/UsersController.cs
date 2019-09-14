using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectManager;
using ProjectManager.BusinessLayer;
using ProjectManager.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ProjectManager.Controllers
{
    public class UsersController : ApiController
    {
        private ProjectManagerEntities1 db = new ProjectManagerEntities1();
        UsersBL objUsers = new UsersBL();
        // GET: api/Users
        [ResponseType(typeof(List<UserEntity>))]
        public JArray GetUsers()
        {
            try
            {
                var json = JsonConvert.SerializeObject(objUsers.GetUsers());
                JArray jsonArray = JArray.Parse(json);
                return jsonArray;
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }

        // GET: api/Users/5
        [ResponseType(typeof(UserEntity))]
        public IHttpActionResult GetUser(int id)
        {
            try
            {
                UserEntity objUserEntity = new UserEntity();
                objUserEntity = objUsers.GetUser(id);
                if (objUserEntity == null)
                {
                    return NotFound();
                }

                return Ok(objUserEntity);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("api/Users/{id}")]
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, [FromBody]JObject Juser)
        {
            try
            {
                UserEntity user = JsonConvert.DeserializeObject<UserEntity>(Juser.ToString());
                objUsers.PutUser(id, user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser([FromBody]JObject Juser)
        {
            UserEntity user = JsonConvert.DeserializeObject<UserEntity>(Juser.ToString());
            objUsers.PostUser(user);
            return Ok(user);
        }

        [HttpDelete]
        [Route("api/Users/{id}")]
        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            objUsers.DeleteUser(id);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}