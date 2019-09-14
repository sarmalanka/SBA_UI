using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectManager.Models;

namespace ProjectManager.BusinessLayer
{
    public class UsersBL
    {
        private ProjectManagerEntities1 db = new ProjectManagerEntities1();
        public List<UserEntity> GetUsers()
        {
            List<UserEntity> users = new List<UserEntity>();
            foreach(var user in db.Users)
            {
                UserEntity objUser = new UserEntity();
                objUser = RetUserEntity(user);
                users.Add(objUser);
            }
            return users;
        }
        public UserEntity GetUser(int id)
        {
            User user = db.Users.Find(id);
            return RetUserEntity(user);
        }
        public bool PutUser(int id, UserEntity user)
        {
            User objUser = new User();
            objUser = RetUser(user);
            db.Entry(objUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
        public UserEntity PostUser(UserEntity usr)
        {
            User user = new User();
            user = RetUser(usr);
            db.Users.Add(user);
            db.SaveChanges();
            return usr;
        }
        public bool DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return false;
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return true;
        }
        private UserEntity RetUserEntity(User user)
        {
            UserEntity objUser = new UserEntity();
            if (user != null)
            {
                objUser.EmployeeID = user.EmployeeID;
                objUser.FirstName = user.FirstName;
                objUser.LastName = user.LastName;
                objUser.UserID = user.UserID;
            }
            return objUser;
        }
        private User RetUser(UserEntity usersdbobj)
        {
            User objUser = new User();
            if (usersdbobj != null)
            {
                objUser.EmployeeID = usersdbobj.EmployeeID;
                objUser.FirstName = usersdbobj.FirstName;
                objUser.LastName = usersdbobj.LastName;
                objUser.UserID = usersdbobj.UserID;
            }
            return objUser;
        }
    }
}