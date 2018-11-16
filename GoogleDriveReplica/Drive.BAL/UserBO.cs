using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.Entities;
using Drive.DAL;

namespace Drive.BAL
{
    public class UserBO
    {
        public static int Save(UserDTO dto)
        {
            return UserDAO.Save(dto);
        }

        public static int UpdatePassword(UserDTO dto)
        {
            return UserDAO.UpdatePassword(dto);
        }

        public static UserDTO ValidateUser(String pLogin, String pPassword)
        {
            return UserDAO.ValidateUser(pLogin, pPassword);
        }
        public static UserDTO GetUserById(int pid)
        {
            return UserDAO.GetUserById(pid);
        }
        public static List<UserDTO> GetAllUsers()
        {
            return DAL.UserDAO.GetAllUsers();
        }

        public static int DeleteUser(int pid)
        {
            return DAL.UserDAO.DeleteUser(pid);
        }

        public static UserDTO ValidateEmail(String email)
        {
            return DAL.UserDAO.ValidateEmail(email);
        }


        public static int ResetPassword(string email, string passwd)
        {
            return DAL.UserDAO.ResetPassword(email, passwd);
        }

        public static UserDTO GetUserByLogin(string pid)
        {
            return UserDAO.GetUserByLogin(pid);
        }

    }
}
