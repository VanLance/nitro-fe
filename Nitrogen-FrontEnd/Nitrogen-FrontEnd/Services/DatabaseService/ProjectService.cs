using DocumentFormat.OpenXml.Wordprocessing;
using Nitrogen_FrontEnd.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Nitrogen_FrontEnd.Services.DatabaseService
{
    public class ProjectService
    {
        private readonly string connectionString;

        public ProjectService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //public Project GetProjectById(int id)
        //{
        //    // Implementation remains the same
        //}

        //public Project GetProjectByProjectNumber(string projectNumber)
        //{
        //    // Implementation remains the same
        //}

        // Other project-related methods...
        //Project-related methods:

        //    GetProjectById
        // GetProjectByProjectNumber
        // GetAllProjects
        // AddProject
        // EditProject
    }

}

