using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ECommerceAIMockUp.Application.Cases
{
    public class GetDesignsCase
    {
        private readonly IDesignRepository _designRepository;
        private readonly IConfiguration _config;

        public GetDesignsCase(IDesignRepository designRepository, IConfiguration config)
        {
            _designRepository = designRepository;
            _config = config;
        }

        public async Task<Response<List<DesignDTO>>> GetDesignsAsync(string userId)
        {
            List<DesignDTO> designDTOs = new List<DesignDTO>();
            IEnumerable<Design> designs = await _designRepository.GetAllDesignByUserIdAsync(userId);
            string imageBaseUrl = _config["ImageUrlSetting:BaseUrl"]!;
            foreach (Design design in designs)
            {
                DesignDTO designDTO = new DesignDTO();
                designDTO.Id = design.Id;
                designDTO.ImageUrl = $"{imageBaseUrl.TrimEnd('/')}/designs/{design.ImageUrl.TrimStart('/')}";
                designDTOs.Add(designDTO);
            }
            var respone = new Response<List<DesignDTO>> { IsSucceeded = true, Data = designDTOs };
            return respone;
        }
    }
}
