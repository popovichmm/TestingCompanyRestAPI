using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyREST.Controllers;
using CompanyREST.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using RestSharp;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using System.Net;
using Newtonsoft.Json;

namespace UnitTestProject
{
    [TestClass]
    public class TestsCompany
    {
        private RestClient client;
        private RestRequest request;
        
        public TestsCompany()
        {
            client = new RestClient("https://localhost:44329/api/");
            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, sslPolicyErrors) => true;
        }

        #region Get_tests
        [TestMethod]
        public void GetAllCompanies_ShouldReturnOk()
        {            
            request = new RestRequest("companies/", Method.GET);
            var response = client.Execute(request);
            
            Assert.NotNull(response.Content);
            Assert.AreEqual(true, response.IsSuccessful);
        }

        [TestMethod]
        public void GetOneCompany_ShouldReturnOk()
        {
            request = new RestRequest("companies/1", Method.GET);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);            
            Assert.AreEqual(true, response.IsSuccessful);
            //Assert.AreEqual(true, response.IsSuccessful);
            //if (!response.StatusCode.Equals(HttpStatusCode.OK))
            //{
            //    Assert.Fail(response.Content);
            //}
        }

        [TestMethod]
        public void GetOneCompany_ShouldReturnBad()
        {
            request = new RestRequest("companies/99999", Method.GET);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }
        #endregion

        #region Add_tests
        [TestMethod]
        public void AddNewCompany_ShouldReturnOK()
        {
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo1", Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(true, response.IsSuccessful);
        }

        [TestMethod]
        public void AddNewCompanyWithoutAddress_ShouldReturnBad()
        {
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo1" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }

        [TestMethod]
        public void AddNewEmptyCompany_ShouldReturnBad()
        {
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }

        [TestMethod]
        public void AddNewCompanyWithAllDetails_ShouldReturnOK()
        {
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo1", 
                                            INN = "12312312312", 
                                            CPP = "989898987", 
                                            Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(true, response.IsSuccessful);
        }
        #endregion

        #region Update_tests
        [TestMethod]
        public void UpdateCompany_ShouldReturnOK()
        {
            // add new company
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo_for_update", Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);
            Company addedCompany = JsonConvert.DeserializeObject<Company>(response.Content);            

            // update added company
            var newCompanyForUpdate = new Company { Id = addedCompany.Id,
                                                    Name = "Demo_updated", 
                                                    INN = "updated and added", 
                                                    Address = "address_updated"
                                                    };
            request = new RestRequest("companies/" + addedCompany.Id.ToString(), Method.PUT);
            request.AddJsonBody(newCompanyForUpdate);
            response = client.Execute(request);

            // get updated company
            request = new RestRequest("companies/" + addedCompany.Id.ToString(), Method.GET);
            response = client.Execute(request);
            Company updatedCompany = JsonConvert.DeserializeObject<Company>(response.Content);

            if (!(updatedCompany.Name == "Demo_updated" && 
                updatedCompany.INN == "updated and added" &&
                updatedCompany.Address == "address_updated" &&
                updatedCompany.Id == addedCompany.Id))
            {
                Assert.Fail();
            }

            // delete added and update company
            request = new RestRequest("companies/" + addedCompany.Id.ToString(), Method.DELETE);
            response = client.Execute(request);
        }

        [TestMethod]
        public void UpdateCompanyWithoutName_ShouldReturnBad()
        {
            // add new company
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo_for_update", Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);
            Company addedCompany = JsonConvert.DeserializeObject<Company>(response.Content);

            // update added company
            var newCompanyForUpdate = new Company
            {
                Id = addedCompany.Id,
                INN = "updated and added",
                Address = "address_updated"
            };
            request = new RestRequest("companies/" + addedCompany.Id.ToString(), Method.PUT);
            request.AddJsonBody(newCompanyForUpdate);
            response = client.Execute(request);

            // delete added and update company
            request = new RestRequest("companies/" + addedCompany.Id.ToString(), Method.DELETE);
            var responseDelete = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);            
        }
        #endregion

        #region Delete_tests
        [TestMethod]
        public void DeleteCompany_ShouldReturnOK()
        {
            // add new company
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo_for_delete", Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);
            Company addedCompany = JsonConvert.DeserializeObject<Company>(response.Content);

            request = new RestRequest("companies/" + addedCompany.Id.ToString(), Method.DELETE);
            response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(true, response.IsSuccessful);
        }

        [TestMethod]
        public void DeleteCompanyByNullId_ShouldReturnBad()
        {
            var request = new RestRequest("companies/", Method.DELETE);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }

        [TestMethod]
        public void DeleteCompanyByNonexistentIndex_ShouldReturnBad()
        {
            var request = new RestRequest("companies/9999", Method.DELETE);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }

        #endregion

    }

    [TestClass]
    public class TestsDepartments
    {
        private RestClient client;
        private RestRequest request;

        public TestsDepartments()
        {
            client = new RestClient("https://localhost:44329/api/");
            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, sslPolicyErrors) => true;
        }

        #region Get_tests
        [TestMethod]
        public void GetAllDepartments_ShouldReturnOk()
        {
            request = new RestRequest("departments/", Method.GET);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(true, response.IsSuccessful);
        }

        [TestMethod]
        public void GetOneDepartment_ShouldReturnOk()
        {
            request = new RestRequest("departments/1", Method.GET);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(true, response.IsSuccessful);
            //Assert.AreEqual(true, response.IsSuccessful);
            //if (!response.StatusCode.Equals(HttpStatusCode.OK))
            //{
            //    Assert.Fail(response.Content);
            //}
        }

        [TestMethod]
        public void GetOneDepartment_ShouldReturnBad()
        {
            request = new RestRequest("departments/99999", Method.GET);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }
        #endregion

        #region Add_tests
        [TestMethod]
        public void AddNewDepartment_ShouldReturnOK()
        {
            // add new company
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo_for_update", Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);
            Company addedCompany = JsonConvert.DeserializeObject<Company>(response.Content);

            // add new department
            request = new RestRequest("departments/", Method.POST);
            var newDepartment = new Department { Name = "Demo1", CountEmployees = 222, CompanyId = addedCompany.Id };
            request.AddJsonBody(newDepartment);
            response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(true, response.IsSuccessful);
        }

        [TestMethod]
        public void AddNewDepartmentWithoutCompanyId_ShouldReturnBad()
        {
            request = new RestRequest("departments/", Method.POST);
            var newDepartment = new Department { Name = "Demo1", CountEmployees = 222 };
            request.AddJsonBody(newDepartment);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }

        [TestMethod]
        public void AddNewEmptyDepartment_ShouldReturnBad()
        {
            request = new RestRequest("departments/", Method.POST);
            var newDepartment = new Department { };
            request.AddJsonBody(newDepartment);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }
                
        #endregion

        #region Update_tests
        [TestMethod]
        public void UpdateDepartment_ShouldReturnOK()
        {
            // add new company
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo_for_update", Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);
            Company addedCompany = JsonConvert.DeserializeObject<Company>(response.Content);

            // add new department
            request = new RestRequest("departments/", Method.POST);
            var newDepartment = new Department { Name = "Demo1", CountEmployees = 222, CompanyId = addedCompany.Id };
            request.AddJsonBody(newDepartment);
            response = client.Execute(request);
            Department addedDepartment = JsonConvert.DeserializeObject<Department>(response.Content);

            // update added department
            var newDepartmentForUpdate = new Department
            {
                Id = addedDepartment.Id,
                Name = "Demo_updated",
                CountEmployees = 777,
                CompanyId = addedCompany.Id
            };
            request = new RestRequest("departments/" + addedDepartment.Id.ToString(), Method.PUT);
            request.AddJsonBody(newDepartmentForUpdate);
            response = client.Execute(request);

            // get updated department
            request = new RestRequest("departments/" + addedDepartment.Id.ToString(), Method.GET);
            response = client.Execute(request);
            Department updatedDepartment = JsonConvert.DeserializeObject<Department>(response.Content);

            if (!(updatedDepartment.Name == "Demo_updated" &&
                updatedDepartment.CountEmployees == 777 &&
                updatedDepartment.Id == addedDepartment.Id))
            {
                Assert.Fail();
            }

            // delete added and update department and company
            request = new RestRequest("departments/" + addedDepartment.Id.ToString(), Method.DELETE);
            response = client.Execute(request);

            request = new RestRequest("companies/" + addedCompany.Id.ToString(), Method.DELETE);
            response = client.Execute(request);
        }

        [TestMethod]
        public void UpdateDepartmentWithoutName_ShouldReturnBad()
        {
            // add new company
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo_for_update", Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);
            Company addedCompany = JsonConvert.DeserializeObject<Company>(response.Content);

            // add new department
            request = new RestRequest("departments/", Method.POST);
            var newDepartment = new Department { Name = "Demo1", CountEmployees = 222, CompanyId = addedCompany.Id };
            request.AddJsonBody(newDepartment);
            response = client.Execute(request);
            Department addedDepartment = JsonConvert.DeserializeObject<Department>(response.Content);

            // update added department
            var newDepartmentForUpdate = new Department
            {
                Id = addedDepartment.Id,
                CountEmployees = 777
            };
            request = new RestRequest("departments/" + addedDepartment.Id.ToString(), Method.PUT);
            request.AddJsonBody(newDepartmentForUpdate);
            response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }
        #endregion

        #region Delete_tests
        [TestMethod]
        public void DeleteDepartment_ShouldReturnOK()
        {
            // add new company
            request = new RestRequest("companies/", Method.POST);
            var newCompany = new Company { Name = "Demo_for_update", Address = "2222" };
            request.AddJsonBody(newCompany);
            var response = client.Execute(request);
            Company addedCompany = JsonConvert.DeserializeObject<Company>(response.Content);

            // add new department
            request = new RestRequest("departments/", Method.POST);
            var newDepartment = new Department { Name = "Demo1", CountEmployees = 222, CompanyId = addedCompany.Id };
            request.AddJsonBody(newDepartment);
            response = client.Execute(request);
            Department addedDepartment = JsonConvert.DeserializeObject<Department>(response.Content);

            request = new RestRequest("departments/" + addedDepartment.Id.ToString(), Method.DELETE);
            response = client.Execute(request);

            request = new RestRequest("companies/" + addedCompany.Id.ToString(), Method.DELETE);
            response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(true, response.IsSuccessful);
        }

        [TestMethod]
        public void DeleteDepartmentByNullId_ShouldReturnBad()
        {
            var request = new RestRequest("departments/", Method.DELETE);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }

        [TestMethod]
        public void DeleteDepartmentByNonexistentIndex_ShouldReturnBad()
        {
            var request = new RestRequest("departments/9999", Method.DELETE);
            var response = client.Execute(request);

            Assert.NotNull(response.Content);
            Assert.AreEqual(false, response.IsSuccessful);
        }
        #endregion

    }
}
