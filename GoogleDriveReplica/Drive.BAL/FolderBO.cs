using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DAL;
using Drive.Entities;

namespace Drive.BAL
{
   public  class FolderBO
    {
        

        public static int DeleteUser(int fid)
        {
            return FolderDAO.DeleteUser(fid);
        }

        public static FolderDTO FindFolderName(String fname, int uid, int pid)
        {
            return FolderDAO.FindFolderName(fname,uid,pid);
        }

        public static List<FolderDTO> GetAllMainFoldersByUser(int uid, int pid)
        {
            return Drive.DAL.FolderDAO.GetAllMainFoldersByUser(uid,pid);
        }

        public static int SaveFolder(string fname, int uid, int pid)
        {
            FolderDTO folder = new FolderDTO();
            folder.Name = fname;
            folder.CreatedBy = uid;
            folder.CreatedOn = DateTime.Now;
            folder.ParentFolderId = pid;
            return Drive.DAL.FolderDAO.SaveFolder(folder);
        }

        public static int DeleteFolder(int id)
        {
            return FolderDAO.DeleteFolder(id);
        }

        public static FolderDTO GetFolder(int uid, int pid)
        {
            return Drive.DAL.FolderDAO.GetFolders(uid, pid);
        }
    }
}
