using Drive.BAL;
using Drive.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GoogleDriveReplica.Controllers
{
    public class FoldersDataController : ApiController
    {
        [HttpGet]
        public List<FolderDTO> GetAllFolders(int uid, int pid)
        {
            return FolderBO.GetAllMainFoldersByUser(uid,pid);
        }

        [HttpGet]
        public List<FileDTO> GetAllFiles(int uid, int pid)
        {
            return FileBO.GetAllFiles(uid, pid);
        }

        [HttpGet]
        public String CreateFolder(String fname, int uid, int pid)
        {
            FolderDTO folder = FolderBO.FindFolderName(fname, uid, pid);
            if(folder.Id == 0)
            {
                // Insert Folder into DB
                FolderBO.SaveFolder(fname, uid, pid);
                return "Folder Created!";
            }
            else
                return "Folder Exits Already!";
        }
        [HttpGet]
        public int DeleteFolder(int fid)
        {
            return FolderBO.DeleteFolder(fid);
        }

    }
}