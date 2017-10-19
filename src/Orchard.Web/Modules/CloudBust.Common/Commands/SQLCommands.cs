using Orchard;
using Orchard.Alias;
using Orchard.Commands;
using System;
using Orchard.Environment;
using System.Linq;
using System.Data.SqlClient;
using Orchard.Environment.Configuration;
using Orchard.MediaLibrary.Services;
using System.Web.Hosting;
using System.IO;

namespace CloudBust.Common.Commands
{
    public class SQLCommands : DefaultOrchardCommandHandler
    {
        private readonly Work<WorkContext> _workContext;
        //private readonly IWorkContextAccessor _workContextAccesor;
        public IOrchardServices Services { get; private set; }
        private readonly ShellSettings _thisShellSettings;


        public SQLCommands(
            Work<WorkContext> workContext, 
            IOrchardServices orchardServices,
            ShellSettings shellSettings
            )
        {
            _workContext = workContext;
            Services = orchardServices;
            _thisShellSettings = shellSettings;
        }

        [CommandName("sql backup")]
        [CommandHelp("sql backup \r\n\t" + "Create backup file from SQL")]
        public void Backup()
        {
            SqlConnection con = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();

            Context.Output.WriteLine(_thisShellSettings.DataProvider);
            Context.Output.WriteLine(_thisShellSettings.DataTablePrefix);
            Context.Output.WriteLine(_thisShellSettings.DataConnectionString);


            //if (String.IsNullOrWhiteSpace(_thisShellSettings.DataConnectionString))
            //{
            //    Context.Output.WriteLine(T("Could not acquire data connection string"));
            //    return;
            //}

            //con.ConnectionString = _thisShellSettings.DataConnectionString;
            
            //string _backupPath = HostingEnvironment.MapPath("~/Modules/CloudBust.Common/SQLBackup/");
            //string _filename = _backupPath + GetUniqueID() + ".bak";

            //if (!Directory.Exists(_backupPath))
            //{
            //    Directory.CreateDirectory(_backupPath);
            //}
            //if (File.Exists(_filename))
            //{
            //    Context.Output.WriteLine(T("Data backup file, already exists {0}.", _filename));
            //    return;
            //}
            //try
            //{
            //    con.Open();
            //    sqlcmd = new SqlCommand("backup database test to disk='" + _filename + "'", con);
            //    sqlcmd.ExecuteNonQuery();
            //    con.Close();
            //}
            //catch (Exception ex)
            //{
            //    Services.TransactionManager.Cancel();
            //    Context.Output.WriteLine(T("An error occured while creating the backup: {0}.", ex.Message));
            //    return;
            //}
            //Context.Output.WriteLine(T("Backup {0} created.", _filename));
        }
        private string GetUniqueID()
        {
            return Guid.NewGuid().ToString("N");
        }
        //private string GetMediaFolder()
        //{
        //    string uniqueID = GetUniqueID();
        //    if (string.IsNullOrWhiteSpace(uniqueID)) return null;

        //    string mediafolder = _mediaLibraryService.Combine(@"sqlbackup", uniqueID);
        //    _mediaLibraryService.CreateFolder(null, mediafolder);
        //    return mediafolder;
        //}
    }
}