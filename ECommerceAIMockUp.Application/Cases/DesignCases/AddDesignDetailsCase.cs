using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Design;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Cases.DesignCases
{
    public class AddDesignDetailsCase
    {
        private readonly IBaseRepository<DesignDetails> _designDetailsRepo;

        public AddDesignDetailsCase(IBaseRepository<DesignDetails> designDetailsRepo)
        {
            _designDetailsRepo = designDetailsRepo;
        }

        public async Task<Response<int>> AddDesignDetailsAsync(DesignDetailsDTO designDetailsDTO)
        {
            DesignDetails designDetails = new DesignDetails
            {
                DesignId = designDetailsDTO.DesignId,
                ScaleX = designDetailsDTO.ScaleX,
                ScaleY = designDetailsDTO.ScaleY,
                XAxis = designDetailsDTO.XAxis,
                YAxis = designDetailsDTO.YAxis,
                Opacity = designDetailsDTO.Opacity,
                Rotation = designDetailsDTO.Rotation,
                Position = designDetailsDTO.Position
            };
            try
            {
                designDetails = await _designDetailsRepo.CreateAsync(designDetails);
                await _designDetailsRepo.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("can not add to database");
            }
            return new Response<int> { IsSucceeded = true, Data = designDetails.Id };
        }
    }
}
