var pid = -1;
var uid = '';
var fid = -1;     
var fname = '';

$(document).ready(function () {
    
    uid = $('#uid').val();
    //Hide Upload button on main screen
    $('#btnUploadFile').hide();

    //Getting all folders on dom ready
    GetAllFolder();

    // Displaying option on single click
    $('#container').delegate('button', 'click', function () {
        fid = $(this).attr('id');
        fname = $(this).text();
    });

    // Openning folder with double click
    $('#container').delegate('button', 'dblclick', function () {
        fid = $(this).attr('id');
        pid = fid;
        fname = $(this).text();
        var btn = $('<button>').attr('id', fid);
        btn.text(fname + " > ");
        btn.addClass("btn btn-default");
        btn.click(function () {
            lastPid = pid;
            pid = $(this).attr('id');
            $('#container').empty();
            $("#files").empty();
            GetAllFolder(); 
            GetAllFiles();
            fid = -1;
            fname = '';
            while(pid != lastPid)
            {
                lastPid = $('#clickPath').find("button:last").attr('id');
                if (pid != lastPid) {
                    $('#clickPath').find("button:last").remove();
                }
            }
        });
        btn.appendTo('#clickPath');
        $('#container').empty();
        $("#files").empty();
        $('#btnUploadFile').show();
        GetAllFolder();
        GetAllFiles();
        fid = -1;
        fname = '';
    });

    //Anonymous functions for different handlers
    $(function () {
        $("#btnCreateFolder").click(function () {
            $("#foldername").val('');
            $("#folderCreationForm").show(400);
            $("#folderCreationForm #valueFromMyButton").text($(this).val());
            $("#folderCreationForm input[type=text]").val();
            $("#valueFromMyModal").val('');
            $("#folderCreationForm").show(400);
            $("#foldername").select();
        });
        // Folder Creat Button
        $("#btnCreateOK").click(function () {
            var foldername = $("#folderCreationForm input[type=text]").val();
            $("#folderCreationForm").hide(50);
            var customURL = "http://localhost:18696/api/FoldersData/CreateFolder?fname=" + foldername + "&uid=" + uid + "&pid=" + pid;
            CreateFolder(customURL);
            fid = -1;
            fname = '';
        });
        // File Upload Button
        $("#btnFileOK").click(function () {
            var data = new FormData();
            var file = $("#fileSelectionForm input[type=file]")[0].files[0];
            data.append("NewFile", file);
            $.ajax({
                type: "POST",
                url: "http://localhost:18696/api/FileData/UploadFile?uid=" + uid + "&pid=" + pid,
                contentType: false,
                processData: false,
                data:data,
                success: function (response) {
                    $("#fileSelectionForm").hide(400);
                    alert("File Uploaded Successfully!");
                    $("#files").empty();
                    GetAllFiles();
                }
            });
        });

        // Delete Forlder Handler
        $("#btnDeleteFolder").click(function () {
            if (fid == -1)
                alert("Select any folder");
            else if (confirm("Are you sure to delete the Entire Folder?") == true) {
                DeleteFolder(fid);
                fid = -1; // Reset Value
            }
            else
                fid = -1;
        });
    });

    // Upload a file
    $("#btnUploadFile").click(function () {
        $("#fileSelectionForm").show(400);
    });

    // Cancel options
    $("#btnFileCancelOK").click(function () {
        $("#fileSelectionForm").hide(400);
    });
    $("#btnFolderCancelOK").click(function () {
        $("#folderCreationForm").hide(400);
    });

    // Meta Data
    $("#btnMetaData").click(function () {
        GetMetaData();
    });
});

// Get All folders in a folder
function GetAllFolder() {
    var folderID = "";
    var folderName = "";
    $.ajax({
        dataType: 'json',
        type: "GET",
        url: "http://localhost:18696/api/FoldersData/GetAllFolders?uid="+uid+"&pid="+pid,   
        contentType: false,
        processData: false,
        success: function (JSONObject) {
            for (var key in JSONObject) {
                if (JSONObject.hasOwnProperty(key)) {
                    folderID = JSONObject[key]['Id'];

                    // Creating Image element
                    var img = $("<img id='ficon" + folderID + "' style='border: none; padding: 0; background: none'> "); //Equivalent: $(document.createElement('img'))
                    img.attr('src', 'http://localhost:18696/Content/Image/icon.png');
                    img.attr('width', 44);
                    img.attr('height', 44);
                    img.text(JSONObject[key]['Name']);
                    img.appendTo('#container');

                    // wrapping link arround image
                    $("#ficon" + folderID).wrap($('<button>', {
                        id: folderID,
                        name:JSONObject[key]['Name'],
                        style: 'border: none; padding: 0; background: none',
                        class: 'flink',
                    }));


                    // Creating folder named link
                    var namedlink = $('<button>');
                    namedlink.text(JSONObject[key]['Name']);
                    namedlink.attr('style', 'border:none');
                    namedlink.attr('id', folderID);
                    namedlink.attr('class', 'flink');
                    namedlink.attr('style', 'border: none; padding: 0; background: none')
                    namedlink.appendTo('#container');

                    var nextLine = $('<br />');
                    nextLine.appendTo('#container');
                }
            }
        }
    });
    fid = -1;
}

// Display All files in folder
function GetAllFiles() {
    var fileID = "";
    var fileName = "";
    $.ajax({
        dataType: 'json',
        type: "GET",
        url: "http://localhost:18696/api/FoldersData/GetAllFiles?uid=" + uid + "&pid=" + pid,
        contentType: false,
        processData: false,
        success: function (JSONObject) {
            $('#files').append('<thead><tr><td>Name</td> <td>Type</td> <td>Size</td> <td>Download Option</td> <td>Delete Option</td> <td>Preview</td></tr></thead>');
            var table = $("files");
            for (var key in JSONObject) {

                if (JSONObject.hasOwnProperty(key)) {
                    fileID = JSONObject[key]['Id'];
                    fileName = JSONObject[key]['Name'];
                    fileExt = JSONObject[key]['FileExt'];
                    fileType = JSONObject[key]['ContentType'];
                    filesize = JSONObject[key]['FileSizeInKB'];
                    uname = JSONObject[key]['UniqueName'];
                    // Creating row
                    var nameTypeSize = "<tr> <td>" + fileName + "</td> <td>" + fileType + "</td> <td>" + filesize + " KB</td>";
                    var download = "<td ><a class='flink' id='" + uname + "' onclick='DownloadFile(this.id)'>Click to Download</a></td>";
                    var delet = "<td> <a class='flink' id='" + fileID + "' onclick='DeleteFile(this.id)'>Click to Delete</a></td>";
                    var thumbnail = "<td><img alt='Not Found' src='http://localhost:18696/api/FileData/GetThumbnail?uniqueName=" + uname + "' width='39px' height='39px' /> </tr>";
                    var row = nameTypeSize + download + delet + thumbnail;
                    $('#files').append(row);
                }
            }
        }
    });
}

// Download File

function DownloadFile(uname)
{
    var url = "http://localhost:18696/api/FileData/DownloadFile?uname=" + uname;
    window.open(url);
}

// Creating a new folder 
function CreateFolder(customURL) {
    $.ajax({
        dataType: 'json',
        type: "GET",
        url: customURL,
        contentType: false,
        processData: false,
        success: function (response) {
            alert(response);
            $("#container").empty();
            GetAllFolder();
            fid = '';
            fname = '';
        }
    });
}

// Delete folder 
function DeleteFolder(folderid) {
    var customURL = "http://localhost:18696/api/FoldersData/DeleteFolder?fid=" + folderid;
    $.ajax({
        dataType: 'json',
        type: "GET",
        url: customURL,
        contentType: false,
        processData: false,
        success: function (response) {
            alert("Folder Deleted");
            $("#container").empty();
            $("#files").empty();
            GetAllFolder();
            GetAllFiles();
        }
    });
}


// Delete file
function DeleteFile(fileId) {
    if (confirm("Are you sure to delete the file?") == true) {
        var customURL = "http://localhost:18696/api/FileData/DeleteFile?fid=" + fileId;
        $.ajax({
            dataType: 'json',
            type: "GET",
            url: customURL,
            contentType: false,
            processData: false,
            success: function (response) {
                alert("File Deleted");
                $("#files").empty();
                GetAllFiles();
            }
        });
    }
}

// Display Main Screen Folders
function ShowMainScreen()
{
    while (pid != -1) {
        pid = $('#clickPath').find("button:last").attr('id');
        if (pid != -1) {
            $('#clickPath').find("button:last").remove();
        }
    }

    $('#btnUploadFile').hide();
    $('#container').empty();
    $("#files").empty();
    GetAllFolder();
    fid = -1;
    fname = '';
}

// Download Meta Data
function  GetMetaData()
{
    if(fid != -1)
    {
        var setting = {
            type: "POST",
            dataType: "JSON",
            url: "http://localhost:18696/api/MetaData/GetMetaData?uid=" + uid + "&pid=" + fid,
            success: function (result) {
                if (result != null) {
                    var url = "http://localhost:18696/api/FoldersData/api/DownloadMetaData";
                    window.open(url);
                }
            },
            error: function (result) {
                alert("Error occured while downloading Meta Information");
            }
        }
        $.ajax(setting);
        fid = -1;
        return false;
    }
    else
    {
        alert("Select any folder!");
    }
}