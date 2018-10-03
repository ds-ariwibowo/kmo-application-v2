using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kmo.Constant
{
    public static class SysWebApiUrl
    {
        #region UsersController

        /// <summary>
        /// sysapi/users/GetUserCompany
        /// </summary>
        public static string GetUserCompany { get { return "sysapi/users/GetUserCompany"; } }

        /// <summary>
        /// sysapi/users/DeleteUser
        /// </summary>
        public static string DeleteUser { get { return "sysapi/users/DeleteUser"; } }

        /// <summary>
        /// sysapi/users/UpdateUserRole
        /// </summary>
        public static string UpdateUserRole { get { return "sysapi/users/UpdateUserRole"; } }


        /// <summary>
        /// sysapi/users/UpdateUserModuleAccess
        /// </summary>
        public static string UpdateUserModuleAccess { get { return "sysapi/users/UpdateUserModuleAccess"; } }

        /// <summary>
        /// sysapi/users/UpdateUserCompanyRole
        /// </summary>
        public static string UpdateUserCompanyRole { get { return "sysapi/users/UpdateUserCompanyRole"; } }

        /// <summary>
        /// sysapi/users/UpdateUser
        /// </summary>
        public static string UpdateUser { get { return "sysapi/users/UpdateUser"; } }

        /// <summary>
        /// sysapi/users/PostNewUser
        /// </summary>
        public static string PostNewUser { get { return "sysapi/users/PostNewUser"; } }

        /// <summary>
        /// sysapi/users/GetUserDbView
        /// </summary>
        public static string GetUserDbView { get { return "sysapi/users/GetUserDbView"; } }

        /// <summary>
        /// sysapi/users/GetUsersDbView
        /// </summary>
        public static string GetUsersDbView { get { return "sysapi/users/GetUsersDbView"; } }

        /// <summary>
        /// sysapi/users/RequestConnStrDictionary
        /// </summary>
        public static string RequestConnStrDictionary { get { return "sysapi/users/RequestConnStrDictionary"; } }

        /// <summary>
        /// sysapi/users/ping
        /// </summary>
        public static string Ping { get { return "sysapi/users/ping"; } }

        /// <summary>
        /// sysapi/users/RequestPermanentToken
        /// </summary>
        public static string RequestPermanentToken { get { return "sysapi/users/RequestPermanentToken"; } }

        /// <summary>
        /// sysapi/users/ListOfUsers
        /// </summary>
        public static string ListOfUsers { get { return "sysapi/users/ListOfUsers"; } }

        /// <summary>
        /// sysapi/users/ValidateTokenLife
        /// </summary>
        public static string ValidateTokenLife { get { return "sysapi/users/ValidateTokenLife"; } }

        /// <summary>
        /// sysapi/users/GetUser
        /// </summary>
        public static string GetUser { get { return "sysapi/users/GetUser"; } }

        /// <summary>
        /// sysapi/users/GetUserDb
        /// </summary>
        public static string GetUserDb { get { return "sysapi/users/GetUserDb"; } }


        /// <summary>
        /// sysapi/users/GetUserModules
        /// </summary>
        public static string GetUserModules { get { return "sysapi/users/GetUserModules"; } }

        #endregion

        #region SystemParametersController

        /// <summary>
        /// sysapi/systemparameters/GetAllCompanyData
        /// </summary>
        public static string GetAllCompanyData { get { return "sysapi/systemparameters/GetAllCompanyData"; } }

        /// <summary>
        /// sysapi/systemparameters/GetAllGlobalOption
        /// </summary>
        public static string GetAllGlobalOption { get { return "sysapi/systemparameters/GetAllGlobalOption"; } }

        /// <summary>
        /// sysapi/systemparameters/GetServerTime
        /// </summary>
        public static string GetServerTime { get { return "sysapi/systemparameters/GetServerTime"; } }

        /// <summary>
        /// sysapi/systemparameters/GlobalOption
        /// </summary>
        public static string GlobalOptions { get { return "sysapi/systemparameters/GlobalOption"; } }

        /// <summary>
        /// sysapi/systemparameters/GlobalOption_Update
        /// </summary>
        public static string GlobalOption_Update { get { return "sysapi/systemparameters/GlobalOption_Update"; } }

        /// <summary>
        /// sysapi/systemparameters/GlobalOption_New
        /// </summary>
        public static string GlobalOption_New { get { return "sysapi/systemparameters/GlobalOption_New"; } }

        #endregion

        #region DataStorageController
        /// <summary>
        /// sysapi/datastorage/UploadBinary
        /// </summary>
        public static string UploadBinary { get { return "sysapi/datastorage/UploadBinary"; } }

        /// <summary>
        /// sysapi/datastorage/DownloadBinary
        /// </summary>
        public static string DownloadBinary { get { return "sysapi/datastorage/DownloadBinary"; } }

        /// <summary>
        /// sysapi/datastorage/PostLog
        /// </summary>
        public static string PostLog { get { return "sysapi/datastorage/PostLog"; } }


        /// <summary>
        /// sysapi/datastorage/BuildReport
        /// </summary>
        public static string BuildReport { get { return "sysapi/datastorage/BuildReport"; } }
        #endregion

        #region ModulesController

        /// <summary>
        /// sysapi/modules/GetModule
        /// </summary>
        public static string GetModuleRole { get { return "sysapi/modules/GetModuleRole"; } }

        /// <summary>
        /// sysapi/modules/GetModule
        /// </summary>
        public static string GetModule { get { return "sysapi/modules/GetModule"; } }

        /// <summary>
        /// sysapi/modules/GetModules
        /// </summary>
        public static string GetModules { get { return "sysapi/modules/GetModules"; } }

        /// <summary>
        /// sysapi/modules/PostModuleRevision
        /// </summary>
        public static string PostModuleRevision { get { return "sysapi/modules/PostModuleRevision"; } }

        /// <summary>
        /// sysapi/modules/GetLastModuleRevision
        /// </summary>
        public static string GetLastModuleRevision { get { return "sysapi/modules/GetLastModuleRevision"; } }

        /// <summary>
        /// sysapi/modules/DeleteModule
        /// </summary>
        public static string DeleteModule { get { return "sysapi/modules/DeleteModule"; } }

        #endregion

        #region DocumentAndReportController

        /// <summary>
        /// sysapi/documentandreport/PostNewDocument
        /// </summary>
        public static string PostNewDocument { get { return "sysapi/documentandreport/PostNewDocument"; } }

        /// <summary>
        /// sysapi/documentandreport/PostUpdateDocument
        /// </summary>
        public static string PostUpdateDocument { get { return "sysapi/documentandreport/PostUpdateDocument"; } }

        /// <summary>
        /// sysapi/documentandreport/PostNewCrystalReportRevision
        /// </summary>
        public static string PostNewCrystalReportRevision { get { return "sysapi/documentandreport/PostNewCrystalReportRevision"; } }

        /// <summary>
        /// sysapi/documentandreport/PostUpdateCrystalReportRevision
        /// </summary>
        public static string PostUpdateCrystalReportRevision { get { return "sysapi/documentandreport/PostUpdateCrystalReportRevision"; } }


        /// <summary>
        /// sysapi/documentandreport/GetAllDocuments
        /// </summary>
        public static string GetAllDocuments { get { return "sysapi/documentandreport/GetAllDocuments"; } }

        /// <summary>
        /// sysapi/documentandreport/GetDocument
        /// </summary>
        public static string GetDocument { get { return "sysapi/documentandreport/GetDocument"; } }

        /// <summary>
        /// sysapi/documentandreport/GetLastReportRevision
        /// </summary>
        public static string GetLastReportRevision { get { return "sysapi/documentandreport/GetLastReportRevision"; } }




        #endregion

        #region LibraryManagement

        /// <summary>
        /// sysapi/LibraryManagement/PostLibrary
        /// </summary>
        public static string PostLibrary { get { return "sysapi/LibraryManagement/PostLibrary"; } }


        /// <summary>
        /// sysapi/LibraryManagement/PostLibrary
        /// </summary>
        public static string PostLibraryData { get { return "sysapi/LibraryManagement/PostLibraryData"; } }


        /// <summary>
        /// sysapi/LibraryManagement/GetLibs
        /// </summary>
        public static string GetLibs { get { return "sysapi/LibraryManagement/GetLibs"; } }


        /// <summary>
        /// sysapi/LibraryManagement/GetLastLibraryRevision
        /// </summary>
        public static string GetLastLibraryRevision { get { return "sysapi/LibraryManagement/GetLastLibraryRevision"; } }


        /// <summary>
        /// sysapi/LibraryManagement/GetCurrentLibraries
        /// </summary>
        public static string GetCurrentLibraries { get { return "sysapi/LibraryManagement/GetCurrentLibraries"; } }


        /// <summary>
        /// sysapi/LibraryManagement/GetLibraryBinary
        /// </summary>
        public static string GetLibraryBinary { get { return "sysapi/LibraryManagement/GetLibraryBinary"; } }
        #endregion

    }
}
