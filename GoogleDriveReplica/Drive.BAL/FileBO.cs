using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DAL;
using Drive.Entities;

namespace Drive.BAL
{
    public class FileBO
    {
         public static int Save(FileDTO dto)
        {
            return FileDAO.Save(dto);
        }

         public static int DeleteUser(int fid)
         {
             return FileDAO.DeleteUser(fid);
         }

         public static List<FileDTO> GetAllFiles(int uid, int pid)
         {
             return FileDAO.GetAllFiles(uid, pid);
         }

         public static FileDTO GetFileByUniqueName(string uniqueName)
         {
             return FileDAO.GetFileByUniqueName(uniqueName);
         }

         public static int DeleteFile(int fid)
         {
             return FileDAO.DeleteFile(fid);
         }
    }
}
