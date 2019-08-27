using CROPS.Configuration;
using CROPS.PowerBI.Configuration;
using CROPS.Projects;
using CROPS.Reports;
using CROPS.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace CROPS.PowerBI
{
    public class PowerBIProvider : IReportsProvider
    {
        private readonly string ApiUrl;
        private readonly string GroupId;
        private readonly string ClientId;
        private readonly string Username;
        private readonly string Password;
        private readonly string ResourceUrl;
        private readonly string AuthorityUrl;
        private const string ReportType = "report";
        private const string DashboardType = "dashboard";
        private const string DecryptionKey = "E546C8DF278CD5931069B522E695D4F2";

        public PowerBIProvider(IConfiguration configuration)
        {
            Username = configuration["Configurations:pbiUsername"];
            Password = configuration["Configurations:pbiPassword"];
            AuthorityUrl = configuration["Configurations:authorityUrl"];
            ResourceUrl = configuration["Configurations:resourceUrl"];
            ClientId = configuration["Configurations:clientId"];
            ApiUrl = configuration["Configurations:apiUrl"];
            GroupId = configuration["Configurations:groupId"];
        }
        private PowerBIClient GetClient()
        {
            string accesstoken = GetAccessToken();
            TokenCredentials tokenCredentials = new TokenCredentials(accesstoken, "Bearer");
            PowerBIClient client = new PowerBIClient(tokenCredentials);
            return client;
        }
        public void GetReports()
        {
            throw new NotImplementedException();
        }
        public async Task<Group> CreateWorkspace(string name)
        {
            Group group = null;
            string accesstoken = GetAccessToken();
            TokenCredentials tokenCredentials = new TokenCredentials(accesstoken, "Bearer");
            using (PowerBIClient client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
            {
                group = await client.Groups.CreateGroupAsync(new GroupCreationRequest(name)).ConfigureAwait(false);
                List<string> ext = new List<string> { ".pbix" };

                //TODO: Need to read this from configuration
                string folderPath = @"\\10.1.22.46\MasterReports";

                //TODO: Need to replace Directory access with StorageManager
                IEnumerable<string> reportsFilesPathes = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s)));
                foreach (string filePath in reportsFilesPathes)
                {
                    using (FileStream fileStream = File.OpenRead(filePath.Trim('"')))
                    {
                        Import import = await client.Imports.PostImportWithFileAsync(group.Id, fileStream, Path.GetFileName(filePath)).ConfigureAwait(false);

                        // Example of polling the import to check when the import has succeeded.
                        while (import.ImportState != "Succeeded" && import.ImportState != "Failed")
                        {
                            import = await client.Imports.GetImportByIdAsync(group.Id, import.Id).ConfigureAwait(false);

                            //TODO: Use Logger
                            Console.WriteLine("Checking import state... {0}", import.ImportState);
                        }
                    }
                }
            }
            return group;
        }
        public async Task<bool> CheckWorkspaceExist(string name)
        {
            string accesstoken = GetAccessToken();
            TokenCredentials tokenCredentials = new TokenCredentials(accesstoken, "Bearer");
            PowerBIClient client = new PowerBIClient(tokenCredentials);
            ODataResponseListGroup workspace = await client.Groups.GetGroupsAsync().ConfigureAwait(false);
            return workspace.Value.Any(x => x.Name == name);
        }
        public string GetAccessToken()
        {
            HttpWebRequest request = WebRequest.CreateHttp(AuthorityUrl);
            //POST web request to create a datasource.
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentLength = 0;
            request.ContentType = "application/x-www-form-urlencoded";

            //Add token to the request header
            request.Headers.Add("Authorization", string.Format(CultureInfo.InvariantCulture, "Bearer {0}", ""));

            NameValueCollection parsedQueryString = HttpUtility.ParseQueryString(string.Empty);
            parsedQueryString.Add("client_id", ClientId);
            parsedQueryString.Add("grant_type", "password");
            parsedQueryString.Add("resource", ResourceUrl);
            parsedQueryString.Add("username", Username);
            parsedQueryString.Add("password", Password);
            string postdata = parsedQueryString.ToString();


            //POST web request
            byte[] dataByteArray = System.Text.Encoding.ASCII.GetBytes(postdata);
            request.ContentLength = dataByteArray.Length;

            //Write JSON byte[] into a Stream
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(dataByteArray, 0, dataByteArray.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                dynamic responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                return responseJson["access_token"];
            }
        }

        public List<Group> GetAllWorkspaces()
        {
            var accesstoken = GetAccessToken();

            TokenCredentials tokenCredentials = new TokenCredentials(accesstoken, "Bearer");

            using (PowerBIClient client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
            {
                var groups = client.Groups.GetGroups().Value.ToList();

                return groups;
            }
        }

        public async Task<string> GetReportUrlAsync(string groupId, string reportId)
        {
            var client = GetClient();
            ODataResponseListReport reports = await client.Reports.GetReportsInGroupAsync(groupId).ConfigureAwait(false);

            Microsoft.PowerBI.Api.V2.Models.Report report = reports.Value.FirstOrDefault(r => r.Id == reportId);

            return report.EmbedUrl;
        }

        public async Task<Microsoft.PowerBI.Api.V2.Models.Dashboard> GetDashboardAsync(string groupId, string dashboardId)
        {
            var client = GetClient();
            var dashboards = await client.Dashboards.GetDashboardsInGroupAsync(groupId).ConfigureAwait(false);

            Microsoft.PowerBI.Api.V2.Models.Dashboard dashboard = dashboards.Value.FirstOrDefault(r => r.Id == dashboardId);

            return dashboard;
        }

        public List<Microsoft.PowerBI.Api.V2.Models.Report> GetAllReportsInWorkspace(string workspaceId)
        {
            var accesstoken = GetAccessToken();

            TokenCredentials tokenCredentials = new TokenCredentials(accesstoken, "Bearer");

            using (PowerBIClient client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
            {
                var reports = client.Reports.GetReportsInGroupAsync(workspaceId).Result.Value.ToList();

                return reports;
            }
        }

        public List<Microsoft.PowerBI.Api.V2.Models.Dashboard> GetAllDashboardsInWorkspace(string workspaceId)
        {
            var accesstoken = GetAccessToken();

            TokenCredentials tokenCredentials = new TokenCredentials(accesstoken, "Bearer");

            using (PowerBIClient client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
            {
                var dashboards = client.Dashboards.GetDashboardsInGroupAsync(workspaceId).Result.Value.ToList();

                return dashboards;
            }
        }

        public async Task CheckMasterReportsWithProjectsWorkspaces(Project projectWorkspace, List<Reports.Report> reports)
        {
            var ext = new List<string> { ".pbix" };
            var folderPath = @"\\EGY-VPI-TFSBI1\MasterReports";
            var reportsFilesPathes = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s))).ToList();

            var accesstoken = GetAccessToken();

            TokenCredentials tokenCredentials = new TokenCredentials(accesstoken, "Bearer");

            using (PowerBIClient client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
            {                
                foreach (var filePath in reportsFilesPathes)
                {
                    var masterReportExists = reports.Any(report => report.Name.ToLower().Equals(Path.GetFileNameWithoutExtension(filePath).ToLower()));
                    if (masterReportExists) continue;
                    {
                        using (var fileStream = File.OpenRead(filePath.Trim('"')))
                        {
                            var import = await client.Imports.PostImportWithFileAsync(projectWorkspace.WorkspaceId, fileStream, Path.GetFileName(filePath));

                            // Example of polling the import to check when the import has succeeded.
                            while (import.ImportState != "Succeeded" && import.ImportState != "Failed")
                            {
                                import = await client.Imports.GetImportByIdAsync(projectWorkspace.WorkspaceId, import.Id);
                                Console.WriteLine("Checking import state... {0}", import.ImportState);
                            }
                        }
                    }
                }


            }
        }
    }
}
