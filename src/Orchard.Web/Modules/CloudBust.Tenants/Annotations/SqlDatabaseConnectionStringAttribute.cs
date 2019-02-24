using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace CloudBust.Tenants.Annotations
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
    public class SqlDatabaseConnectionStringAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var s = value as string;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(s);

                //TODO: Should the keys be checked here to ensure that a valid combination was entered? Needs investigation.
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}