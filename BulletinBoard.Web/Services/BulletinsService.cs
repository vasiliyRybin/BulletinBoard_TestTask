using BulletinBoard.DB.Models;
using BulletinBoard.Interfaces;
using BulletinBoard.Web.CustomModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Web.Services
{
    public class BulletinsService
    {
        private IRepository<Bulletin> BulletinRepo { get; }

        public BulletinsService(IRepository<Bulletin> repository)
        {
            BulletinRepo = repository;
        }


        public async Task<List<BulletinExternalModel>> GetAllBulletins(PageParams pageParams, bool test = false)
        {
            try
            {
                AssignDefaultParamsToEmptyInputFields(pageParams);
                var bulletins = BulletinRepo.Get();
                var orderBy = typeof(Bulletin).GetProperties()
                    .Where(x => !x.Name.ToLower().StartsWith("id"))
                    .Where(x => !x.Name.ToLower().EndsWith("id"))
                    .FirstOrDefault(x => x.Name.ToLower() == pageParams.Attribute.ToLower());

                var propName = orderBy?.Name ?? "Name";
                

                if (test)
                {
                    bulletins = pageParams.Order == "desc" ?
                        bulletins.OrderByDescending(x => x.GetType().GetProperty(propName).GetValue(x)) :
                        bulletins.OrderBy(x => x.GetType().GetProperty(propName).GetValue(x));
                }
                else
                {
                    bulletins = pageParams.Order == "desc" ?
                        bulletins.OrderByDescending(x => EF.Property<object>(x, propName)) :
                        bulletins.OrderBy(x => EF.Property<object>(x, propName));
                }

                List<Bulletin> preResult = test ? bulletins.ToList() : await bulletins.ToListAsync();
                List<BulletinExternalModel> result = new();

                foreach (var bulletin in preResult)
                {
                    result.Add(new BulletinExternalModel
                    {
                        Name = bulletin.Name,
                        PhotoLinks = bulletin.PhotoLinks,
                        Price = bulletin.Price
                    });
                }

                return result;
            }
            catch (Exception)
            {
                return new List<BulletinExternalModel>();
                throw;
            }
        }

        public async Task<Bulletin> GetFirstBulletinOrDefault(QueryParams queryParams, string name)
        {
            var bulletin = await Task.Run(() => BulletinRepo.Get().OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.Name == name));

            try
            {
                if (bulletin != null)
                {
                    var result = new Bulletin()
                    {
                        Name = bulletin.Name,
                        PhotoLinks = queryParams.ShowPhotos ? bulletin.PhotoLinks : bulletin.PhotoLinks.Split(", ").FirstOrDefault(),
                        Price = bulletin.Price,
                        Description = queryParams.ShowDescription ? bulletin.Description : string.Empty                        
                    };

                    return result;
                }

                return null;
            }
            catch (Exception)
            {
                return new Bulletin();
                throw;
            }
        }

        public async Task<Bulletin> CreateBulletin(Bulletin bulletin)
        {
            var check = CheckInput(bulletin);

            try
            {
                if (check)
                {
                    var result = await BulletinRepo.CreateAsync(bulletin);
                    if (result) return bulletin;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }            

        }

        private static bool CheckInput(Bulletin bulletin)
        {
            var photoCheckArr = bulletin.PhotoLinks.Split(", ");
            if (bulletin.Name.Length > 1000 || bulletin.Name.Length < 1) return false;
            if (bulletin.Description.Length > 1000 || bulletin.Description.Length < 10) return false;
            if (bulletin.Price < 1 || bulletin.Price > 999999999) return false;
            if (photoCheckArr.Length > 3) return false;

            foreach (var item in photoCheckArr)
            {
                if (!item.EndsWith("jpg") && !item.EndsWith("jpeg") && !item.EndsWith("png")) return false;
            }

            return true;
        }

        private static void AssignDefaultParamsToEmptyInputFields(PageParams pageParams)
        {
            if (string.IsNullOrWhiteSpace(pageParams.Attribute)) pageParams.Attribute = "name";
            if (string.IsNullOrWhiteSpace(pageParams.Order)) pageParams.Order = "asc";
        }
    }
}
