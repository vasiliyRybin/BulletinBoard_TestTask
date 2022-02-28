using BulletinBoard.DB.Models;
using BulletinBoard.Web;
using BulletinBoard.Web.CustomModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BulletinBoard.Tests
{
    public static class BulletinTestMethods
    {
        public static IEnumerable<Bulletin> Setup()
        {
            Guid firstID = Guid.Parse("F9F61F25-BA4E-4A35-3544-08D9D2356056");
            Guid secondID = Guid.Parse("68F6A880-4743-43DB-3545-08D9D2356056");
            Guid thirdID = Guid.Parse("584560BF-8446-48D6-8F6E-4BEAC9EFB646");

            return new List<Bulletin>
            {
                new Bulletin { ID = firstID, CreationDate = DateTime.Now, Description = "First test bulletin description", Name = "Test bulletin N1", Price = 10 },
                new Bulletin { ID = secondID, CreationDate = DateTime.Now, Description = "Second test bulletin description", Name = "Test bulletin N2", Price = 20 },
                new Bulletin { ID = thirdID, CreationDate = DateTime.Now, Description = "First test bulletin description", Name = "Test bulletin N3", Price = 30 }
            };
        }

        public static List<BulletinExternalModel> GetOrderedListByAttribute(IEnumerable<Bulletin> bulletins, PageParams pageParams)
        {
            var orderBy = typeof(Bulletin).GetProperties()
                    .Where(x => !x.Name.ToLower().StartsWith("id"))
                    .Where(x => !x.Name.ToLower().EndsWith("id"))
                    .FirstOrDefault(x => x.Name.ToLower() == pageParams.Attribute.ToLower());

            var propName = orderBy?.Name ?? "Name";

            List<BulletinExternalModel> orderedBulletins = new();

            foreach (var bulletin in bulletins)
            {
                orderedBulletins.Add(new BulletinExternalModel
                {
                    Name = bulletin.Name,
                    Price = bulletin.Price,
                    PhotoLinks = bulletin.PhotoLinks
                });
            }

            IOrderedEnumerable<BulletinExternalModel> preResult = default;

            if (pageParams.Order == "desc")preResult = orderedBulletins
                    .OrderByDescending(x => typeof(BulletinExternalModel).GetProperty(propName).GetValue(x));
            else preResult = orderedBulletins
                    .OrderBy(x => typeof(BulletinExternalModel).GetProperty(propName).GetValue(x));

            return preResult.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                    .Take(pageParams.PageSize)
                    .ToList();
        }

        public static bool ObjectsComparer(List<BulletinExternalModel> input, List<BulletinExternalModel> output)
        {
            int counter = 0;

            for (int i = 0; i < input.Count; i++)
            {
                if (
                    input[i].PhotoLinks == output[i].PhotoLinks &&
                    input[i].Name == output[i].Name &&
                    input[i].Price == output[i].Price
                  ) counter++;
            }

            return counter == input.Count;
        }
    }
}
