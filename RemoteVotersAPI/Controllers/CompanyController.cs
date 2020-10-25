﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using RemoteVotersAPI.Application.Services;
using RemoteVotersAPI.Application.ViewModel;
using RemoteVotersAPI.Infra.ModelSettings;
using RemoteVotersAPI.Utils;

namespace RemoteVotersAPI.Controllers
{
    /// <summary>
    /// Company Controller
    ///
    /// Author: FStrony
    /// </summary>
    [ApiController]
    [Route("company")]
    public class CompanyController : ControllerBase
    {
        /// <value>company service</value>
        private CompanyService companyService;

        /// <value>MongoDB configs</value>
        IOptions<MongoDBConfig> mongoDBConfig;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="mongoDBConfig"></param>
        public CompanyController(IOptions<MongoDBConfig> mongoDBConfig)
        {
            this.mongoDBConfig = mongoDBConfig;
            this.companyService = new CompanyService(mongoDBConfig);
        }

        /// <summary>
        /// POST Create Company
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModelState]
        public async Task Create([FromBody] CompanyViewModel companyModel)
        {
            await companyService.CreateCompany(companyModel);
        }

        /// <summary>
        /// PUT Update Company
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateModelState]
        public async Task Update([FromBody] CompanyViewModel companyModel)
        {
            await companyService.UpdateCompany(companyModel);
        }

        /// <summary>
        /// DELETE delete company By company ID, it also deletes all the campaigns and votes from the company
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] string id)
        {
            await companyService.DeleteCompany(new ObjectId(id));
        }

        /// <summary>
        /// GET Retrieve company information
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Company Information</returns>
        [HttpGet("getCompany/{id}")]
        public async Task<CompanyViewModel> RetrieveByCompanyId([FromRoute] string id)
        {
            return await companyService.RetrieveCompany(new ObjectId(id));
        }
    }
}